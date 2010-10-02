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
    /// The Default IoC backed <seealso cref="IControllerActivator"/>.
    /// </summary>
    public class ExtendedControllerActivator : IControllerActivator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedControllerActivator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="actionInvokerRegistry">The action invoker registry.</param>
        public ExtendedControllerActivator(ContainerAdapter container, IActionInvokerRegistry actionInvokerRegistry)
        {
            Container = container;
            ActionInvokerRegistry = actionInvokerRegistry;
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
        /// Creates the specified request context.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns></returns>
        public virtual IController Create(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return null;
            }

            Controller controller = Container.GetService(controllerType) as Controller;

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

            return controller;
        }
    }
}