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
    using System.Web.Mvc.Async;
    using System.Web.Routing;

    /// <summary>
    /// The Default IoC backed <seealso cref="IControllerFactory"/>.
    /// </summary>
    public class ExtendedControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedControllerFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="actionInvokerRegistry">The action invoker registry.</param>
        public ExtendedControllerFactory(ContainerAdapter container, IActionInvokerRegistry actionInvokerRegistry)
        {
            Invariant.IsNotNull(container, "container");
            Invariant.IsNotNull(actionInvokerRegistry, "actionInvokerRegistry");

            ActionInvokerRegistry = actionInvokerRegistry;
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
        /// Gets the action invoker registry.
        /// </summary>
        /// <value>The action invoker registry.</value>
        protected IActionInvokerRegistry ActionInvokerRegistry
        {
            get;
            private set;
        }

        /// <summary>
        /// Retrieves the controller instance for the specified request context and controller type.
        /// </summary>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <returns>The controller instance.</returns>
        /// <exception cref="T:System.Web.HttpException">
        /// <paramref name="controllerType"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="controllerType"/> cannot be assigned.</exception>
        /// <exception cref="T:System.InvalidOperationException">An instance of <paramref name="controllerType"/> cannot be created.</exception>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return CreateController(controllerType) ?? base.GetControllerInstance(requestContext, controllerType);
        }

        private Controller CreateController(Type controllerType)
        {
            Controller controller = null;

            if (controllerType != null)
            {
                controller = Container.GetService(controllerType) as Controller;

                if (controller != null)
                {
                    Type actionInvokerType;

                    if (ActionInvokerRegistry.IsRegistered(controllerType))
                    {
                        actionInvokerType = ActionInvokerRegistry.Matching(controllerType);
                    }
                    else
                    {
                        actionInvokerType = controller is IAsyncController ?
                                            KnownTypes.AsyncActionInvokerType :
                                            KnownTypes.DefaultActionInvokerType;
                    }

                    controller.ActionInvoker = (IActionInvoker)Container.GetService(actionInvokerType);
                }
            }

            return controller;
        }
    }
}