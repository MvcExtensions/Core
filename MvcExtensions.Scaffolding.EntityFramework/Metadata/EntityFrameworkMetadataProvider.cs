#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Metadata.Edm;
    using System.Data.Objects;
    using System.Data.EntityClient;
    using System.Linq;

    /// <summary>
    /// Defines a a class to store the metadata of a given <seealso cref="ObjectContext"/>.
    /// </summary>
    public class EntityFrameworkMetadataProvider : IEntityFrameworkMetadataProvider
    {
        private readonly IDictionary<string, EntityMetadata> entitySetMappings;
        private readonly IDictionary<Type, EntityMetadata> entityTypeMappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkMetadataProvider"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        public EntityFrameworkMetadataProvider(ObjectContext database)
        {
            Invariant.IsNotNull(database, "database");

            IEnumerable<EntityMetadata> entities = LoadMetadata(database);

            entitySetMappings = entities.ToDictionary(e => e.EntitySetName, e => e, StringComparer.OrdinalIgnoreCase);
            entityTypeMappings = entities.ToDictionary(e => e.EntityType, e => e);
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

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<EntityMetadata> GetEnumerator()
        {
            return entitySetMappings.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static IEnumerable<EntityMetadata> LoadMetadata(ObjectContext database)
        {
            IList<EntityMetadata> entities = new List<EntityMetadata>();

            database.MetadataWorkspace.LoadFromAssembly(database.GetType().Assembly);

            LoadCore(database, entities);
            LoadExtra(database, entities);

            return entities;
        }

        private static void LoadCore(ObjectContext database, ICollection<EntityMetadata> entities)
        {
            EntityContainer container = database.MetadataWorkspace.GetEntityContainer(database.DefaultContainerName, DataSpace.CSpace);
            ObjectItemCollection objectSpaceItems = (ObjectItemCollection)database.MetadataWorkspace.GetItemCollection(DataSpace.OSpace);

            foreach (EntitySet entitySet in container.BaseEntitySets.OfType<EntitySet>().Where(es => es.ElementType.KeyMembers.Count == 1))
            {
                EntityType entityType = (EntityType)database.MetadataWorkspace.GetObjectSpaceType(entitySet.ElementType);
                Type entityClrType = objectSpaceItems.GetClrType(entityType);

                EntityMetadata entityMetadata = new EntityMetadata(entitySet.Name, entityClrType);

                entities.Add(entityMetadata);

                foreach (EdmMember member in entityType.Members)
                {
                    string memberName = member.Name;
                    Type propertyType = entityClrType.GetProperties().Single(property => property.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase)).PropertyType;
                    bool isKey = entityType.KeyMembers.Contains(member);
                    bool isReference = member is NavigationProperty;

                    PropertyMetadata propertyMetadata = new PropertyMetadata { Name = member.Name, PropertyType = propertyType, IsKey = isKey, IsReference = isReference };

                    entityMetadata.AddProperty(propertyMetadata);
                }
            }
        }

        private static void LoadExtra(ObjectContext database, IEnumerable<EntityMetadata> entities)
        {
            foreach (EntityType entity in ((EntityConnection)database.Connection).GetMetadataWorkspace().GetItems(DataSpace.SSpace).OfType<EntityType>())
            {
                string entitySetName = entity.Name;

                EntityMetadata entityMetadata = entities.SingleOrDefault(e => e.EntitySetName.Equals(entitySetName, StringComparison.OrdinalIgnoreCase));

                if (entityMetadata == null)
                {
                    continue;
                }

                foreach (EdmProperty property in entity.Properties)
                {
                    string propertyName = property.Name;

                    PropertyMetadata propertyMetadata = entityMetadata.Properties.Single(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

                    foreach (Facet facet in property.TypeUsage.Facets)
                    {
                        if (facet.Name.Equals("Nullable", StringComparison.OrdinalIgnoreCase))
                        {
                            propertyMetadata.IsNullable = (bool)facet.Value;
                        }
                        else if (facet.Name.Equals("MaxLength", StringComparison.OrdinalIgnoreCase))
                        {
                            propertyMetadata.Length = (int)facet.Value;
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