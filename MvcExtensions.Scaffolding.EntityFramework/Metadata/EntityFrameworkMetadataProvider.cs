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

    /// <summary>
    /// Defines a a class to store the metadata of a given <seealso cref="ObjectContext"/>.
    /// </summary>
    public class EntityFrameworkMetadataProvider : IEntityFrameworkMetadataProvider
    {
        private readonly IDictionary<string, EntitySetMapping> entitySetMappings = new Dictionary<string, EntitySetMapping>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes the provider with given database.
        /// </summary>
        /// <param name="database">The database.</param>
        public void Initialize(ObjectContext database)
        {
            Invariant.IsNotNull(database, "database");

            LoadEntitySetMappings(database);
        }

        /// <summary>
        /// Gets the entity set mapping.
        /// </summary>
        /// <param name="entitySetName">Name of the entity set.</param>
        /// <returns></returns>
        public EntitySetMapping GetEntitySetMapping(string entitySetName)
        {
            EntitySetMapping mapping;

            return entitySetMappings.TryGetValue(entitySetName, out mapping) ? mapping : null;
        }

        private void LoadEntitySetMappings(ObjectContext database)
        {
            database.MetadataWorkspace.LoadFromAssembly(database.GetType().Assembly);

            EntityContainer container = database.MetadataWorkspace.GetEntityContainer(database.DefaultContainerName, DataSpace.CSpace);
            ObjectItemCollection objectSpaceItems = (ObjectItemCollection)database.MetadataWorkspace.GetItemCollection(DataSpace.OSpace);

            // We will only scaffold if entity has only one key
            foreach (EntitySet entitySet in container.BaseEntitySets.OfType<EntitySet>().Where(es => es.ElementType.KeyMembers.Count == 1))
            {
                EntityType entityType = (EntityType)database.MetadataWorkspace.GetObjectSpaceType(entitySet.ElementType);
                Type entityClrType = objectSpaceItems.GetClrType(entityType);
                Type keyClrType = ((PrimitiveType)entitySet.ElementType.KeyMembers.First().TypeUsage.EdmType).ClrEquivalentType;

                EntitySetMapping mapping = new EntitySetMapping(entityClrType, keyClrType);

                entitySetMappings.Add(entitySet.Name, mapping);
            }
        }
    }
}