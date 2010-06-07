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
    /// Defines a class which is used to store relation details.
    /// </summary>
    [DebuggerDisplay("Entity = {EntityType}, Type = {NavigationType}")]
    public class NavigationMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationMetadata"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="navigationType">Type of the relation.</param>
        public NavigationMetadata(PropertyMetadata parent, Type entityType, NavigationType navigationType)
        {
            Invariant.IsNotNull(parent, "parent");
            Invariant.IsNotNull(entityType, "entityType");

            Parent = parent;
            EntityType = entityType;
            NavigationType = navigationType;
        }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public PropertyMetadata Parent { get; private set; }

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        public Type EntityType { get; private set; }

        /// <summary>
        /// Gets the type of the relation.
        /// </summary>
        /// <value>The type of the relation.</value>
        public NavigationType NavigationType { get; private set; }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }
    }
}