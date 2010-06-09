#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System;
    using System.Web.Routing;

    /// <summary>
    /// Defines a controller factory which creates scaffolded controller.
    /// </summary>
    public class ScaffoldedControllerFactory : ExtendedControllerFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScaffoldedControllerFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="actionInvokerRegistry">The action invoker registry.</param>
        /// <param name="controllerTypeRegistry">The controller type registry.</param>
        public ScaffoldedControllerFactory(ContainerAdapter container, IActionInvokerRegistry actionInvokerRegistry, IControllerTypeRegistry controllerTypeRegistry) : base(container, actionInvokerRegistry)
        {
            Invariant.IsNotNull(controllerTypeRegistry, "controllerTypeRegistry");

            ControllerTypeRegistry = controllerTypeRegistry;
        }

        /// <summary>
        /// Gets or sets the controller type registry.
        /// </summary>
        /// <value>The controller type registry.</value>
        protected IControllerTypeRegistry ControllerTypeRegistry
        {
            get; private set;
        }

        /// <summary>
        /// Retrieves the controller type for the specified name and request context.
        /// </summary>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>The controller type.</returns>
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            Type controllerType = ControllerTypeRegistry.GetControllerType(controllerName);

            return controllerType ?? base.GetControllerType(requestContext, controllerName); // Regular controller
        }
    }
}