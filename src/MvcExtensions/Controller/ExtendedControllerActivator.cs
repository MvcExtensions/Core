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
        /// <param name="controllerActivatorRegistry">The controller activator registry.</param>
        /// <param name="actionInvokerRegistry">The action invoker registry.</param>
        public ExtendedControllerActivator(ContainerAdapter container, TypeMappingRegistry<Controller, IControllerActivator> controllerActivatorRegistry, TypeMappingRegistry<Controller, IActionInvoker> actionInvokerRegistry)
        {
            Invariant.IsNotNull(container, "container");
            Invariant.IsNotNull(controllerActivatorRegistry, "controllerActivatorRegistry");
            Invariant.IsNotNull(actionInvokerRegistry, "actionInvokerRegistry");

            Container = container;
            ControllerActivatorRegistry = controllerActivatorRegistry;
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
        /// Gets or sets the controller activator registry.
        /// </summary>
        /// <value>The controller activator registry.</value>
        protected TypeMappingRegistry<Controller, IControllerActivator> ControllerActivatorRegistry
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the action invoker registry.
        /// </summary>
        /// <value>The action invoker registry.</value>
        protected TypeMappingRegistry<Controller, IActionInvoker> ActionInvokerRegistry
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

            Type activatorType = ControllerActivatorRegistry.Matching(controllerType);

            IControllerActivator activator = activatorType != null ?
                                             (IControllerActivator)Container.GetServices(activatorType) :
                                             null;

            Controller controller = activator != null ?
                                    activator.Create(requestContext, controllerType) as Controller :
                                    (Container.GetService(controllerType) ?? Activator.CreateInstance(controllerType, true)) as Controller;

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
                                        KnownTypes.SyncActionInvokerType;
                }

                IActionInvoker actionInvoker = Container.GetService(actionInvokerType) as IActionInvoker;

                if (actionInvoker != null)
                {
                    controller.ActionInvoker = actionInvoker;
                }
            }

            return controller;
        }
    }
}