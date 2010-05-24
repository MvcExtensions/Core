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
    /// Defines a class which is used to store entity set mapping.
    /// </summary>
    public class EntitySetMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySetMapping"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="keyType">Type of the key.</param>
        public EntitySetMapping(Type entityType, Type keyType)
        {
            Invariant.IsNotNull(entityType, "entityType");
            Invariant.IsNotNull(keyType, "keyType");

            EntityType = entityType;
            KeyType = keyType;
        }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        public Type EntityType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the type of the key.
        /// </summary>
        /// <value>The type of the key.</value>
        public Type KeyType
        {
            get;
            private set;
        }
    }
}