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
    using System.Data.EntityClient;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Defines a a class to store the metadata of a given <seealso cref="ObjectContext"/>.
    /// </summary>
    public class EntityFrameworkMetadataProvider : IEntityFrameworkMetadataProvider
    {
        private static readonly CultureInfo currentCulture = CultureInfo.CurrentCulture;

        private readonly IDictionary<string, EntityMetadata> entitySetMappings = new Dictionary<string, EntityMetadata>(StringComparer.OrdinalIgnoreCase);
        private readonly IDictionary<Type, EntityMetadata> entityTypeMappings = new Dictionary<Type, EntityMetadata>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkMetadataProvider"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        public EntityFrameworkMetadataProvider(ObjectContext database)
        {
            Invariant.IsNotNull(database, "database");

            LoadMetadata(database);
        }

        /// <summary>
        /// Gets the entity metadata.
        /// </summary>
        /// <param name="entitySetName">Name of the entity set.</param>
        /// <returns></returns>
        public EntityMetadata GetMetadata(string entitySetName)
        {
            Invariant.IsNotNull(entitySetName, "entitySetName");

            EntityMetadata metadata;

            return entitySetMappings.TryGetValue(entitySetName, out metadata) ? metadata : null;
        }

        /// <summary>
        /// Gets the entity metadata.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        public EntityMetadata GetMetadata(Type entityType)
        {
            Invariant.IsNotNull(entityType, "entityType");

            EntityMetadata metadata;

            return entityTypeMappings.TryGetValue(entityType, out metadata) ? metadata : null;
        }

        private void LoadMetadata(ObjectContext database)
        {
            entitySetMappings.Clear();
            entityTypeMappings.Clear();

            database.MetadataWorkspace.LoadFromAssembly(database.GetType().Assembly);

            EntityContainer container = database.MetadataWorkspace.GetEntityContainer(database.DefaultContainerName, DataSpace.CSpace);
            ObjectItemCollection objectSpaceItems = (ObjectItemCollection)database.MetadataWorkspace.GetItemCollection(DataSpace.OSpace);

            // We will only scaffold if entity has only one key
            foreach (EntitySet entitySet in container.BaseEntitySets.OfType<EntitySet>().Where(es => es.ElementType.KeyMembers.Count == 1))
            {
                EntityType entityType = (EntityType)database.MetadataWorkspace.GetObjectSpaceType(entitySet.ElementType);
                Type entityClrType = objectSpaceItems.GetClrType(entityType);

                EntityMetadata entityMetadata = new EntityMetadata(entitySet.Name, entityClrType);

                entitySetMappings.Add(entityMetadata.EntitySetName, entityMetadata);
                entityTypeMappings.Add(entityMetadata.EntityType, entityMetadata);

                foreach (EdmMember member in entityType.Members)
                {
                    string memberName = member.Name;
                    Type propertyType = entityClrType.GetProperties().Single(property => property.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase)).PropertyType;
                    bool isKey = entityType.KeyMembers.Contains(member);
                    bool isReference = member is NavigationProperty;

                    PropertyMetadata propertyMetadata = new PropertyMetadata { Name = member.Name, Type = propertyType, IsKey = isKey, IsReference = isReference };

                    entityMetadata.AddProperty(propertyMetadata);
                }
            }

            foreach (EntityType entity in ((EntityConnection)database.Connection).GetMetadataWorkspace().GetItems(DataSpace.SSpace).OfType<EntityType>())
            {
                EntityMetadata entityMetadata;

                if (entitySetMappings.TryGetValue(entity.Name, out entityMetadata))
                {
                    foreach (EdmProperty property in entity.Properties)
                    {
                        string propertyName = property.Name;

                        PropertyMetadata propertyMetadata = entityMetadata.Properties.Single(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

                        foreach (Facet facet in property.TypeUsage.Facets)
                        {
                            if (facet.Name.Equals("Nullable", StringComparison.OrdinalIgnoreCase))
                            {
                                propertyMetadata.IsNullable = Convert.ToBoolean(facet.Value, currentCulture);
                            }
                            else if (facet.Name.Equals("MaxLength", StringComparison.OrdinalIgnoreCase))
                            {
                                propertyMetadata.Length = Convert.ToInt64(facet.Value, currentCulture);
                            }
                            else if (facet.Name.Equals("StoreGeneratedPattern", StringComparison.OrdinalIgnoreCase) && facet.Value.ToString().Equals("Identity", StringComparison.OrdinalIgnoreCase))
                            {
                                propertyMetadata.IsGenerated = true;
                            }
                        }
                    }
                }
            }
        }
    }
}