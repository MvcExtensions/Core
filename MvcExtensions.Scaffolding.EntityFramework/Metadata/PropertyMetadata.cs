#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System;

    /// <summary>
    /// Defines a class which is used to store metadata of an entity's property.
    /// </summary>
    public class PropertyMetadata
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is key (Primary Key).
        /// </summary>
        /// <value><c>true</c> if this instance is key; otherwise, <c>false</c>.</value>
        public bool IsKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is reference (Foreign key).
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is reference; otherwise, <c>false</c>.
        /// </value>
        public bool IsReference { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is generated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is generated; otherwise, <c>false</c>.
        /// </value>
        public bool IsGenerated { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type PropertyType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is nullable; otherwise, <c>false</c>.
        /// </value>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        public int Length { get; set; }
    }
}