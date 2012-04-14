#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class which is used to register the default <seealso cref="ModelMetadataProvider"/>.
    /// </summary>
    public class RegisterModelMetadata : BootstrapperTask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterModelMetadata"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RegisterModelMetadata(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");

            Container = container;
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        protected ContainerAdapter Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            var metadataRegistrator = new ModelMetadataRegistrar(Container.GetService<IBuildManager>());
            metadataRegistrator
                .RegisterMetadataTypes(
                    (serviceType, implementationType) => Container.RegisterAsTransient(serviceType, implementationType),
                    (serviceType, implementationType) => Container.RegisterAsSingleton(serviceType, implementationType));

            metadataRegistrator.RegisterMetadataProviders(Container);

            return TaskContinuation.Continue;
        }
    }
}