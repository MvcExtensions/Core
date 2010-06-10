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

            ServiceLocator = serviceLocator;
        }

        /// <summary>
        /// Gets the service locator.
        /// </summary>
        /// <value>The service locator.</value>
        protected IServiceLocator ServiceLocator
        {
            get;
            private set;
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            IEntityFrameworkMetadataProvider metadataProvider = ServiceLocator.GetInstance<IEntityFrameworkMetadataProvider>();
            IViewModelTypeFactory viewModelTypeFactory = ServiceLocator.GetInstance<IViewModelTypeFactory>();
            IViewModelTypeRegistry viewModelTypeRegistry = ServiceLocator.GetInstance<IViewModelTypeRegistry>();

            foreach (EntityMetadata entityMetadata in metadataProvider)
            {
                Type viewModelType = viewModelTypeFactory.Create(entityMetadata.EntityType);

                viewModelTypeRegistry.Register(entityMetadata.EntityType, viewModelType);
            }

            return TaskContinuation.Continue;
        }
    }
}