#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class which is used  resolve model binder for a give type.
    /// </summary>
    public class ExtendedModelBinderProvider : IModelBinderProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedModelBinderProvider"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="modelBinderRegistry">The model binder registry.</param>
        public ExtendedModelBinderProvider(ContainerAdapter container, TypeMappingRegistry<object, IModelBinder> modelBinderRegistry)
        {
            Invariant.IsNotNull(container, "container");
            Invariant.IsNotNull(modelBinderRegistry, "modelBinderRegistry");

            Container = container;
            ModelBinderRegistry = modelBinderRegistry;
        }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        protected ContainerAdapter Container { get; private set; }

        /// <summary>
        /// Gets or sets the controller activator registry.
        /// </summary>
        /// <value>The controller activator registry.</value>
        protected TypeMappingRegistry<object, IModelBinder> ModelBinderRegistry
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the binder.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns></returns>
        public virtual IModelBinder GetBinder(Type modelType)
        {
            Invariant.IsNotNull(modelType, "modelType");

            Type binderType = ModelBinderRegistry.Matching(modelType);

            if (binderType != null)
            {
                return (IModelBinder)Container.GetService(binderType);
            }

            return null;
        }
    }
}