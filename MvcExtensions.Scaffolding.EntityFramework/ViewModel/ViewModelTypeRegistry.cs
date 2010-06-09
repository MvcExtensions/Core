#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System;
    using System.Globalization;
    using System.Collections.Generic;

    /// <summary>
    /// Defines an interface which is used to  store view model type mapping.
    /// </summary>
    public class ViewModelTypeRegistry : IViewModelTypeRegistry
    {
        private static readonly Type viewModelInterfaceType = typeof(IViewModel);

        private readonly Dictionary<Type, Type> mapping = new Dictionary<Type, Type>();

        /// <summary>
        /// Registers the view model type for the specified entity type.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="viewModelType">Type of the view model.</param>
        public void Register(Type entityType, Type viewModelType)
        {
            Invariant.IsNotNull(entityType, "entityType");
            Invariant.IsNotNull(viewModelType, "viewModelType");

            if (!entityType.IsClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionMessages.TheSpecifiedTypeMustBeAClassType, entityType.FullName));
            }

            if (!viewModelInterfaceType.IsAssignableFrom(viewModelType))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionMessages.TheSpecifiedTypeDoesNotImplementsViewModelInterface, viewModelType.FullName, viewModelInterfaceType.FullName));
            }

            mapping[entityType] = viewModelType;
        }

        /// <summary>
        /// Gets the view model type for specified entity type.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        public Type GetViewModelType(Type entityType)
        {
            Invariant.IsNotNull(entityType, "entityType");

            Type viewModelType;

            return mapping.TryGetValue(entityType, out viewModelType) ? viewModelType : null;
        }
    }
}