#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    /// <summary>
    /// The Default IoC backed <seealso cref="IHttpControllerActivator"/>.
    /// </summary>
    public class ExtendedHttpControllerActivator : IHttpControllerActivator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedControllerActivator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="controllerActivatorRegistry">The controller activator registry.</param>
        public ExtendedHttpControllerActivator(ContainerAdapter container, TypeMappingRegistry<ApiController, IHttpControllerActivator> controllerActivatorRegistry)
        {
            Invariant.IsNotNull(container, "container");
            Invariant.IsNotNull(controllerActivatorRegistry, "controllerActivatorRegistry");

            Container = container;
            ControllerActivatorRegistry = controllerActivatorRegistry;
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
        /// Gets or sets the controller activator registry.
        /// </summary>
        /// <value>The controller activator registry.</value>
        protected TypeMappingRegistry<ApiController, IHttpControllerActivator> ControllerActivatorRegistry
        {
            get;
            private set;
        }


        /// <summary>
        /// Creates the specified request context.
        /// </summary>
        /// <param name="request">The request context.</param>
        /// <param name="controllerDescriptor">Describes the HTTP controller.</param>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns></returns>
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            if (controllerType == null)
            {
                return null;
            }

            var activatorType = ControllerActivatorRegistry.Matching(controllerType);

            var activator = activatorType != null ?
                                             (IHttpControllerActivator) Container.GetServices(activatorType) :
                                             null;

            var controller = activator != null ?
                                    activator.Create(request, controllerDescriptor, controllerType) as ApiController :
                                    (Container.GetService(controllerType) ?? Activator.CreateInstance(controllerType, true)) as ApiController;


            return controller;
        }
    }
}