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
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines a a class to store the metadata of a given <seealso cref="ObjectContext"/>.
    /// </summary>
    public class EntityFrameworkMetadataProvider : IEntityFrameworkMetadataProvider
    {
        private const StringComparison DefaultStringComparison = StringComparison.OrdinalIgnoreCase;

        private static readonly StringComparer defaultStringComparer = StringComparer.OrdinalIgnoreCase;

        private readonly IDictionary<string, EntityMetadata> entitySetMappings;
        private readonly IDictionary<Type, EntityMetadata> entityTypeMappings;

        private readonly MethodInfo createQueryMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkMetadataProvider"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        public EntityFrameworkMetadataProvider(ObjectContext database)
        {
            Invariant.IsNotNull(database, "database");

            createQueryMethod = database.GetType().GetMethod("CreateQuery");

            IEnumerable<EntityMetadata> entities = LoadMetadata(database);

            entitySetMappings = entities.ToDictionary(e => e.EntitySetName, e => e, defaultStringComparer);
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

        private static void LoadNullableMaxLengthGeneratedAndNavigation(ObjectContext database, EntityContainer container, IDictionary<string, EntityMetadata> entitySetMapping, IDictionary<string, EntityMetadata> entityTypeMapping)
        {
            foreach (EntityType entityType in database.MetadataWorkspace.GetItems(DataSpace.CSpace).OfType<EntityType>())
            {
                EntityMetadata entityMetadata = entityTypeMapping[entityType.Name];

                LoadNullableMaxLengthAndGenerated(entityType, entityMetadata);
                LoadNavigation(entityType, entityMetadata, container, entitySetMapping);
            }
        }

        private static void LoadNullableMaxLengthAndGenerated(EntityType entityType, EntityMetadata entityMetadata)
        {
            foreach (EdmProperty property in entityType.Properties)
            {
                PropertyMetadata propertyMetadata = entityMetadata.FindProperty(property.Name);

                propertyMetadata.IsNullable = property.Nullable;
                Facet maxLength = property.TypeUsage.Facets.SingleOrDefault(f => f.Name.Equals("MaxLength", DefaultStringComparison));

                if (maxLength != null)
                {
                    if (maxLength.IsUnbounded)
                    {
                        propertyMetadata.MaximumLength = int.MaxValue;
                    }
                    else if ((maxLength.Value != null) && (maxLength.Value is int))
                    {
                        propertyMetadata.MaximumLength = (int)maxLength.Value;
                    }
                }

                MetadataProperty metadataProperty;

                if (property.MetadataProperties.TryGetValue("http://schemas.microsoft.com/ado/2009/02/edm/annotation:StoreGeneratedPattern", false, out metadataProperty))
                {
                    string valueInString = metadataProperty.Value.ToString();

                    if (valueInString.Equals("Identity", DefaultStringComparison) ||
                        valueInString.Equals("Computed", DefaultStringComparison))
                    {
                        propertyMetadata.IsGenerated = true;
                    }
                }
            }
        }

        private static void LoadNavigation(EntityType entityType, EntityMetadata entityMetadata, EntityContainer container, IDictionary<string, EntityMetadata> entitySetMapping)
        {
            foreach (NavigationProperty navigationProperty in entityType.NavigationProperties)
            {
                NavigationProperty property = navigationProperty;

                AssociationSet associationSet = container.BaseEntitySets.OfType<AssociationSet>().Single(a => a.Name.Equals(property.RelationshipType.Name, DefaultStringComparison));
                AssociationSetEnd associationSetEnd = associationSet.AssociationSetEnds.Single(a => a.CorrespondingAssociationEndMember.Name.Equals(property.ToEndMember.Name, DefaultStringComparison));

                EntitySet entitySet = associationSetEnd.EntitySet;
                NavigationType navigationType = ConvertNavigationType(associationSetEnd.CorrespondingAssociationEndMember.RelationshipMultiplicity);
                Type entityClrType = entitySetMapping[entitySet.Name].EntityType;

                PropertyMetadata propertyMetadata = entityMetadata.FindProperty(navigationProperty.Name);

                propertyMetadata.Navigation = new NavigationMetadata(propertyMetadata, entityClrType, navigationType);
            }
        }

        private static void LoadForeignKeys(EntityContainer container, IDictionary<string, EntityMetadata> entitySetMapping)
        {
            foreach (AssociationSet associationSet in container.BaseEntitySets.OfType<AssociationSet>())
            {
                foreach (ReferentialConstraint constraint in associationSet.ElementType.ReferentialConstraints)
                {
                    ReferentialConstraint localConstraint = constraint;

                    EntitySet entitySet = associationSet.AssociationSetEnds.Single(ase => ase.CorrespondingAssociationEndMember.Name.Equals(localConstraint.ToRole.Name, DefaultStringComparison)).EntitySet;
                    EntityMetadata entityMetadata = entitySetMapping[entitySet.Name];

                    for (int i = 0; i < localConstraint.FromProperties.Count; i++)
                    {
                        PropertyMetadata propertyMetadata = entityMetadata.FindProperty(localConstraint.ToProperties[i].Name);

                        propertyMetadata.IsForeignKey = true;
                    }
                }
            }
        }

        private static void LoadNavigationProperty(IEnumerable<EntityMetadata> entities)
        {
            Type stringType = typeof(string);

            IDictionary<Type, EntityMetadata> mapping = entities.ToDictionary(e => e.EntityType);

            IEnumerable<NavigationMetadata> navigations = entities.SelectMany(e => e.Properties.Select(p => p.Navigation)).Where(n => n != null);

            foreach (NavigationMetadata navigation in navigations)
            {
                EntityMetadata entityMetadata = mapping[navigation.EntityType];

                PropertyMetadata propertyMetadata = entityMetadata.Properties
                                                                  .Where(p => p.PropertyType.Equals(stringType) && !p.IsKey && !p.IsForeignKey)
                                                                  .OrderBy(p => p, new PropertyMetadataComparer())
                                                                  .FirstOrDefault() ??
                                                    entityMetadata.Properties
                                                                  .Where(p => !p.IsKey && !p.IsForeignKey && !p.IsGenerated)
                                                                  .OrderByDescending(p => p.IsNullable)
                                                                  .FirstOrDefault();

                if (propertyMetadata != null)
                {
                    navigation.PropertyName = propertyMetadata.Name;
                }
            }
        }

        private static string QuoteEntitySqlIdentifier(string identifier)
        {
            return "[" + identifier.Replace("]", "]]") + "]";
        }

        private static NavigationType ConvertNavigationType(RelationshipMultiplicity type)
        {
            return (type == RelationshipMultiplicity.Many) ? NavigationType.Many : NavigationType.One;
        }

        /// <summary>
        /// Loads the metadata.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <returns></returns>
        private IEnumerable<EntityMetadata> LoadMetadata(ObjectContext database)
        {
            IList<EntityMetadata> entities = new List<EntityMetadata>();
            IDictionary<string, EntityMetadata> entitySetMapping = new Dictionary<string, EntityMetadata>(defaultStringComparer);
            IDictionary<string, EntityMetadata> entityTypeMapping = new Dictionary<string, EntityMetadata>(defaultStringComparer);
            IDictionary<NavigationProperty, PropertyMetadata> propertyMetadataMapping = new Dictionary<NavigationProperty, PropertyMetadata>();

            database.MetadataWorkspace.LoadFromAssembly(database.GetType().Assembly);

            EntityContainer container = database.MetadataWorkspace.GetEntityContainer(database.DefaultContainerName, DataSpace.CSpace);

            LoadCore(database, container, entities, entitySetMapping, entityTypeMapping, propertyMetadataMapping);
            LoadNullableMaxLengthGeneratedAndNavigation(database, container, entitySetMapping, entityTypeMapping);
            LoadForeignKeys(container, entitySetMapping);
            LoadNavigationProperty(entities);

            return entities;
        }

        private void LoadCore(ObjectContext database, EntityContainer container, ICollection<EntityMetadata> entities, IDictionary<string, EntityMetadata> entitySetMapping, IDictionary<string, EntityMetadata> entityTypeMapping, IDictionary<NavigationProperty, PropertyMetadata> propertyMetadataMapping)
        {
            ObjectItemCollection objectSpaceItems = (ObjectItemCollection)database.MetadataWorkspace.GetItemCollection(DataSpace.OSpace);

            foreach (EntitySet entitySet in container.BaseEntitySets.OfType<EntitySet>())
            {
                EntityType entityType = (EntityType)database.MetadataWorkspace.GetObjectSpaceType(entitySet.ElementType);

                Type entityClrType = objectSpaceItems.GetClrType(entityType);

                MethodInfo genericQueryMethod = createQueryMethod.MakeGenericMethod(new[] { entityClrType });
                string queryString = QuoteEntitySqlIdentifier(entitySet.EntityContainer.Name) + "." + QuoteEntitySqlIdentifier(entitySet.Name);
                object queryResult = genericQueryMethod.Invoke(database, new object[] { queryString, new ObjectParameter[0] });
                queryResult.GetType().GetMethod("ToTraceString").Invoke(queryResult, null);

                EntityMetadata entityMetadata = new EntityMetadata(entitySet.Name, entityClrType);

                entities.Add(entityMetadata);
                entitySetMapping.Add(entitySet.Name, entityMetadata);
                entityTypeMapping.Add(entitySet.ElementType.Name, entityMetadata);

                foreach (EdmMember member in entityType.Members)
                {
                    string memberName = member.Name;
                    Type propertyType = entityClrType.GetProperties().Single(p => p.Name.Equals(memberName, DefaultStringComparison)).PropertyType;
                    bool isKey = entityType.KeyMembers.Any(km => km.Name.Equals(memberName, DefaultStringComparison));

                    NavigationProperty navigationProperty = member as NavigationProperty;

                    PropertyMetadata propertyMetadata = new PropertyMetadata(entityMetadata, member.Name, propertyType, isKey);

                    if (navigationProperty != null)
                    {
                        propertyMetadataMapping.Add(navigationProperty, propertyMetadata);
                    }

                    entityMetadata.AddProperty(propertyMetadata);
                }
            }
        }

        private sealed class PropertyMetadataComparer : IComparer<PropertyMetadata>
        {
            public int Compare(PropertyMetadata x, PropertyMetadata y)
            {
                if (x.IsNullable != y.IsNullable)
                {
                    return x.IsNullable.CompareTo(y.IsNullable);
                }

                if (x.Name.Equals("Name", DefaultStringComparison) && y.Name.Equals("Name", DefaultStringComparison))
                {
                    return 0;
                }

                if (x.Name.Equals("Name", DefaultStringComparison))
                {
                    return -1;
                }

                if (y.Name.Equals("Name", DefaultStringComparison))
                {
                    return 1;
                }

                if (x.Name.Equals(x.Parent.EntityType.Name + "Name", DefaultStringComparison) && y.Name.Equals(y.Parent.EntityType.Name + "Name", DefaultStringComparison))
                {
                    return 0;
                }

                if (x.Name.Equals(x.Parent.EntityType.Name + "Name", DefaultStringComparison))
                {
                    return -1;
                }

                if (y.Name.Equals(y.Parent.EntityType.Name + "Name", DefaultStringComparison))
                {
                    return 1;
                }

                if (x.Name.EndsWith("Name", DefaultStringComparison) && y.Name.EndsWith("Name", DefaultStringComparison))
                {
                    return 0;
                }

                if (x.Name.EndsWith("Name", DefaultStringComparison))
                {
                    return -1;
                }

                if (y.Name.EndsWith("Name", DefaultStringComparison))
                {
                    return 1;
                }

                if (x.MaximumLength == y.MaximumLength)
                {
                    return string.Compare(x.Name, y.Name, DefaultStringComparison);
                }

                return x.MaximumLength.CompareTo(y.MaximumLength);
            }
        }
    }
}