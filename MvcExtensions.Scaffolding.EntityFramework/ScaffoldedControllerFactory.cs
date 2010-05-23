#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Metadata.Edm;
    using System.Data.Objects;
    using System.Linq;
    using System.Web.Routing;

    public class ScaffoldedControllerFactory : ExtendedControllerFactory
    {
        private static readonly Type genericControllerType = typeof(ScaffoldedController<,>);

        private static readonly object entityMapSyncLock = new object();
        private static IDictionary<string, EntityInfo> entityMap;

        public ScaffoldedControllerFactory(ContainerAdapter container, IActionInvokerRegistry actionInvokerRegistry, ObjectContext database) : base(container, actionInvokerRegistry)
        {
            Invariant.IsNotNull(database, "database");

            Database = database;
        }

        protected ObjectContext Database
        {
            get;
            private set;
        }

        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            LoadEntityMap(Database);

            EntityInfo entityInfo;

            if (entityMap.TryGetValue(controllerName, out entityInfo))
            {
                Type controllerType = genericControllerType.MakeGenericType(entityInfo.EntityType, entityInfo.KeyType);

                return controllerType;
            }

            return base.GetControllerType(requestContext, controllerName);
        }

        private static void LoadEntityMap(ObjectContext database)
        {
            if (entityMap == null)
            {
                lock (entityMapSyncLock)
                {
                    if (entityMap == null)
                    {
                        entityMap = BuildEntityMap(database);
                    }
                }
            }
        }

        private static IDictionary<string, EntityInfo> BuildEntityMap(ObjectContext database)
        {
            IDictionary<string, EntityInfo> map = new Dictionary<string, EntityInfo>(StringComparer.OrdinalIgnoreCase);

            database.MetadataWorkspace.LoadFromAssembly(database.GetType().Assembly);

            EntityContainer container = database.MetadataWorkspace.GetEntityContainer(database.DefaultContainerName, DataSpace.CSpace);
            ObjectItemCollection objectSpaceItems = (ObjectItemCollection)database.MetadataWorkspace.GetItemCollection(DataSpace.OSpace);

            foreach (EntitySet entitySet in container.BaseEntitySets.OfType<EntitySet>())
            {
                // We will scaffold if entity has only one key
                if (entitySet.ElementType.KeyMembers.Count == 1)
                {
                    EntityType entityType = (EntityType)database.MetadataWorkspace.GetObjectSpaceType(entitySet.ElementType);
                    Type entityClrType = objectSpaceItems.GetClrType(entityType);
                    Type keyClrType = ((PrimitiveType)entitySet.ElementType.KeyMembers.First().TypeUsage.EdmType).ClrEquivalentType;

                    EntityInfo info = new EntityInfo { EntityType = entityClrType, KeyType = keyClrType };

                    map.Add(entitySet.Name, info);
                }
            }

            return map;
        }

        private sealed class EntityInfo
        {
            public Type EntityType { get; set; }

            public Type KeyType { get; set; }
        }
    }
}