#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System;
    using System.Data;
    using System.Web.Mvc;

    /// <summary>
    /// Defines an static class which contains extension methods of <see cref="ModelMetadata"/>.
    /// </summary>
    public static class ModelMetadataExtensions
    {
        private static readonly Type entityStateType = typeof(EntityState);

        /// <summary>
        /// Determines whether this instance can be displayed.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// <c>true</c> if this instance can show the specified instance; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanShow(this ModelMetadata instance)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.ShowForDisplay && !instance.IsComplexType && !instance.ModelType.Equals(entityStateType);
        }
    }
}