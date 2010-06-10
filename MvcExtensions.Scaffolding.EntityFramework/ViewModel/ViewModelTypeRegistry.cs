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

        private readonly IDictionary<Type, Type> viewModelTypeMapping = new Dictionary<Type, Type>();
        private readonly IDictionary<Type, Type> entityTypeMapping = new Dictionary<Type, Type>();

        /// <summary>
        /// Gets the view model type mapping.
        /// </summary>
        /// <value>The view model type mapping.</value>
        protected virtual IDictionary<Type, Type> ViewModelTypeMapping
        {
            get
            {
                return viewModelTypeMapping;
            }
        }

        /// <summary>
        /// Gets the entity type mapping.
        /// </summary>
        /// <value>The entity type mapping.</value>
        protected virtual IDictionary<Type, Type> EntityTypeMapping
        {
            get
            {
                return entityTypeMapping;
            }
        }

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

            ViewModelTypeMapping[entityType] = viewModelType;
            EntityTypeMapping[viewModelType] = entityType;
        }

        /// <summary>
        /// Determines whether [is view model type registered] [the specified view model type].
        /// </summary>
        /// <param name="viewModelType">Type of the view model.</param>
        /// <returns>
        /// <c>true</c> if [is view model type registered] [the specified view model type]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsViewModelTypeRegistered(Type viewModelType)
        {
            Invariant.IsNotNull(viewModelType, "viewModelType");

            return EntityTypeMapping.ContainsKey(viewModelType);
        }

        /// <summary>
        /// Determines whether [is entity type registered] [the specified entity type].
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>
        /// <c>true</c> if [is entity type registered] [the specified entity type]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEntityTypeRegistered(Type entityType)
        {
            Invariant.IsNotNull(entityType, "entityType");

            return ViewModelTypeMapping.ContainsKey(entityType);
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

            return ViewModelTypeMapping.TryGetValue(entityType, out viewModelType) ? viewModelType : null;
        }

        /// <summary>
        /// Gets the the entity type for the specified view model type.
        /// </summary>
        /// <param name="viewModelType">Type of the view model.</param>
        /// <returns></returns>
        public Type GetEntityType(Type viewModelType)
        {
            Invariant.IsNotNull(viewModelType, "viewModelType");

            Type entityType;

            return EntityTypeMapping.TryGetValue(viewModelType, out entityType) ? entityType : null;
        }

        /// <summary>
        /// Gets the view model type mapping.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<Type, Type>> GetMapping()
        {
            return viewModelTypeMapping;
        }
    }
}