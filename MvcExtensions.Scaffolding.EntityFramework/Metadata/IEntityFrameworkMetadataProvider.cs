#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System.Data.Objects;

    /// <summary>
    /// Defines an interface which is used to hold metadata information of a given <seealso cref="ObjectContext"/>
    /// </summary>
    public interface IEntityFrameworkMetadataProvider
    {
        /// <summary>
        /// Initializes the provider with given database.
        /// </summary>
        /// <param name="database">The database.</param>
        void Initialize(ObjectContext database);

        /// <summary>
        /// Gets the entity set mapping.
        /// </summary>
        /// <param name="entitySetName">Name of the entity set.</param>
        /// <returns></returns>
        EntitySetMapping GetEntitySetMapping(string entitySetName);
    }
}