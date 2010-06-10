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
        /// Determines whether [is view model type registered] [the specified view model type].
        /// </summary>
        /// <param name="viewModelType">Type of the view model.</param>
        /// <returns>
        /// <c>true</c> if [is view model type registered] [the specified view model type]; otherwise, <c>false</c>.
        /// </returns>
        bool IsViewModelTypeRegistered(Type viewModelType);

        /// <summary>
        /// Determines whether [is entity type registered] [the specified entity type].
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>
        /// <c>true</c> if [is entity type registered] [the specified entity type]; otherwise, <c>false</c>.
        /// </returns>
        bool IsEntityTypeRegistered(Type entityType);

        /// <summary>
        /// Gets the view model type for specified entity type.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        Type GetViewModelType(Type entityType);

        /// <summary>
        /// Gets the the entity type for the specified view model type.
        /// </summary>
        /// <param name="viewModelType">Type of the view model.</param>
        /// <returns></returns>
        Type GetEntityType(Type viewModelType);

        /// <summary>
        /// Gets the view model type mapping.
        /// </summary>
        /// <returns></returns>
        IEnumerable<KeyValuePair<Type, Type>> GetMapping();
    }
}