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
    /// Defines a class for storing scaffolded controller types
    /// </summary>
    public class ControllerTypeRegistry : IControllerTypeRegistry
    {
        private static readonly Type genericControllerType = typeof(ScaffoldedController<,,>);

        private readonly Dictionary<string, Type> mapping = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        private readonly object syncLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerTypeRegistry"/> class.
        /// </summary>
        /// <param name="metadataProvider">The metadata provider.</param>
        /// <param name="viewModelTypeRegistry">The view model type registry.</param>
        public ControllerTypeRegistry(IEntityFrameworkMetadataProvider metadataProvider, IViewModelTypeRegistry viewModelTypeRegistry)
        {
            Invariant.IsNotNull(metadataProvider, "metadataProvider");
            Invariant.IsNotNull(viewModelTypeRegistry, "viewModelTypeRegistry");

            MetadataProvider = metadataProvider;
            ViewModelTypeRegistry = viewModelTypeRegistry;
        }

        /// <summary>
        /// Gets the metadata provider.
        /// </summary>
        /// <value>The metadata provider.</value>
        protected IEntityFrameworkMetadataProvider MetadataProvider
        {
            get; private set;
        }

        /// <summary>
        /// Gets the view model type registry.
        /// </summary>
        /// <value>The view model type registry.</value>
        protected IViewModelTypeRegistry ViewModelTypeRegistry
        {
            get; private set;
        }

        /// <summary>
        /// Gets the  scaffolded controller type for specified controller name.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns></returns>
        public Type GetControllerType(string controllerName)
        {
            Type controllerType;

            if (!mapping.TryGetValue(controllerName, out controllerType))
            {
                lock (syncLock)
                {
                    if (!mapping.TryGetValue(controllerName, out controllerType))
                    {
                        EntityMetadata metadata = MetadataProvider.GetMetadata(controllerName);

                        if (metadata != null)
                        {
                            Type[] keyTypes = metadata.GetKeyTypes();

                            // Does  not support multiple key entity
                            if (keyTypes.Length == 1)
                            {
                                Type viewModelType = ViewModelTypeRegistry.GetViewModelType(metadata.EntityType);

                                if (viewModelType != null)
                                {
                                    controllerType = genericControllerType.MakeGenericType(metadata.EntityType, viewModelType, keyTypes[0]);
                                    mapping.Add(controllerName, controllerType);
                                }
                            }
                        }
                    }
                }
            }

            return controllerType;
        }
    }
}