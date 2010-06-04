namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Defines a class which is used to hold the metadata of an entity.
    /// </summary>
    public class EntityMetadata
    {
        private readonly IList<PropertyMetadata> properties;
        private PropertyMetadata keyProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMetadata"/> class.
        /// </summary>
        /// <param name="entitySetName">Name of the entity set.</param>
        /// <param name="entityType">Type of the entity.</param>
        public EntityMetadata(string entitySetName, Type entityType)
        {
            Invariant.IsNotNull(entitySetName, "entitySetName");
            Invariant.IsNotNull(entityType, "entityType");

            EntitySetName = entitySetName;
            EntityType = entityType;
            properties = new List<PropertyMetadata>();
        }

        /// <summary>
        /// Gets or sets the name of the entity set.
        /// </summary>
        /// <value>The name of the entity set.</value>
        public string EntitySetName { get; private set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        public Type EntityType { get; private set; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public virtual IEnumerable<PropertyMetadata> Properties
        {
            [DebuggerStepThrough]
            get
            {
                return properties;
            }
        }

        /// <summary>
        /// Gets the key type.
        /// </summary>
        /// <value>The type of the key.</value>
        public virtual Type KeyType
        {
            [DebuggerStepThrough]
            get
            {
                if (keyProperty == null)
                {
                    keyProperty = properties.FirstOrDefault(p => p.IsKey);
                }

                return (keyProperty != null) ? keyProperty.PropertyType : null;
            }
        }

        /// <summary>
        /// Adds the property.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public virtual void AddProperty(PropertyMetadata metadata)
        {
            Invariant.IsNotNull(metadata, "metadata");

            properties.Add(metadata);
        }
    }
}