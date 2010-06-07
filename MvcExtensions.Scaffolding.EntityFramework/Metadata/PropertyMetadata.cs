#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Defines a class which is used to store metadata of an entity's property.
    /// </summary>
    [DebuggerDisplay("Name = {Name}, Type = {PropertyType}, IsKey = {IsKey}")]
    public class PropertyMetadata
    {
        private bool isForeignKey;
        private NavigationMetadata navigation;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="name">The name.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="isKey">if set to <c>true</c> [is key].</param>
        public PropertyMetadata(EntityMetadata parent, string name, Type propertyType, bool isKey)
        {
            Invariant.IsNotNull(parent, "parent");
            Invariant.IsNotNull(name, "name");
            Invariant.IsNotNull(propertyType, "propertyType");

            Parent = parent;
            Name = name;
            PropertyType = propertyType;
            IsKey = isKey;
        }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public EntityMetadata Parent { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type PropertyType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is key (Primary Key).
        /// </summary>
        /// <value><c>true</c> if this instance is key; otherwise, <c>false</c>.</value>
        public bool IsKey { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is generated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is generated; otherwise, <c>false</c>.
        /// </value>
        public bool IsGenerated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is nullable; otherwise, <c>false</c>.
        /// </value>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets the maximum length.
        /// </summary>
        /// <value>The maximum length.</value>
        public int MaximumLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is foreign key.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is foreign key; otherwise, <c>false</c>.
        /// </value>
        public bool IsForeignKey
        {
            get
            {
                return isForeignKey;
            }

            set
            {
                isForeignKey = value;

                if (isForeignKey)
                {
                    navigation = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the relation.
        /// </summary>
        /// <value>The relation.</value>
        public NavigationMetadata Navigation
        {
            get
            {
                return navigation;
            }

            set
            {
                navigation = value;

                if (navigation != null)
                {
                    isForeignKey = false;
                }
            }
        }
    }
}