#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System;
    using System.Data.Objects;

    /// <summary>
    /// Defines an interface which is used to hold metadata information of a given <seealso cref="ObjectContext"/>
    /// </summary>
    public interface IEntityFrameworkMetadataProvider
    {
        /// <summary>
        /// Gets the entity metadata.
        /// </summary>
        /// <param name="entitySetName">Name of the entity set.</param>
        /// <returns></returns>
        EntityMetadata GetMetadata(string entitySetName);

        /// <summary>
        /// Gets the entity metadata.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        EntityMetadata GetMetadata(Type entityType);
    }
}