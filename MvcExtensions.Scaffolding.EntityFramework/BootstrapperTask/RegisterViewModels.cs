#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Defines a class which is used to register the view models.
    /// </summary>
    public class RegisterViewModels : BootstrapperTask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterViewModels"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        public RegisterViewModels(IServiceLocator serviceLocator)
        {
            Invariant.IsNotNull(serviceLocator, "serviceLocator");

            MetadataProvider = serviceLocator.GetInstance<IEntityFrameworkMetadataProvider>();
            ViewModelTypeFactory = serviceLocator.GetInstance<IViewModelTypeFactory>();
            ViewModelTypeRegistry = serviceLocator.GetInstance<IViewModelTypeRegistry>();
        }

        /// <summary>
        /// Gets or sets the metadata provider.
        /// </summary>
        /// <value>The metadata provider.</value>
        protected IEntityFrameworkMetadataProvider MetadataProvider
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the view model type factory.
        /// </summary>
        /// <value>The view model type factory.</value>
        protected IViewModelTypeFactory ViewModelTypeFactory
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the view model type registry.
        /// </summary>
        /// <value>The view model type registry.</value>
        protected IViewModelTypeRegistry ViewModelTypeRegistry
        {
            get; private set;
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            foreach (EntityMetadata entityMetadata in MetadataProvider)
            {
                Type viewModelType = ViewModelTypeFactory.Create(entityMetadata.EntityType);
                ViewModelTypeRegistry.Register(entityMetadata.EntityType, viewModelType);
            }

            return TaskContinuation.Continue;
        }
    }
}