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
    /// Defines an interface which is used to register view model types.
    /// </summary>
    public interface IViewModelTypeRegistry
    {
        /// <summary>
        /// Registers the view model type for the specified entity type.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="viewModelType">Type of the view model.</param>
        void Register(Type entityType, Type viewModelType);

        /// <summary>
        /// Gets the view model type for specified entity type.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        Type GetViewModelType(Type entityType);
    }
}