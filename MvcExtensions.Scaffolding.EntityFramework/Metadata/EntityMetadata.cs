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
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Defines a class which is used to hold the metadata of an entity.
    /// </summary>
    [DebuggerDisplay("Name = {EntitySetName}, Type = {EntityType}")]
    public class EntityMetadata
    {
        private readonly IDictionary<string, PropertyMetadata> properties;
        private Type[] keyTypes;

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
            properties = new Dictionary<string, PropertyMetadata>(StringComparer.OrdinalIgnoreCase);
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
                return properties.Values;
            }
        }

        /// <summary>
        /// Adds the property.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public virtual void AddProperty(PropertyMetadata metadata)
        {
            Invariant.IsNotNull(metadata, "metadata");

            properties.Add(metadata.Name, metadata);
        }

        /// <summary>
        /// Finds the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual PropertyMetadata FindProperty(string name)
        {
            PropertyMetadata metadata;

            return properties.TryGetValue(name, out metadata) ? metadata : null;
        }

        /// <summary>
        /// Gets the key types.
        /// </summary>
        /// <returns></returns>
        public virtual Type[] GetKeyTypes()
        {
            return keyTypes ?? (keyTypes = properties.Values.Where(p => p.IsKey).Select(p => p.PropertyType).ToArray());
        }
    }
}