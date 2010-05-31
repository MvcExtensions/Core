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
        private static readonly Type genericControllerType = typeof(ScaffoldedController<,>);

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaffoldedControllerFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="actionInvokerRegistry">The action invoker registry.</param>
        /// <param name="metadataProvider">The metadata provider.</param>
        public ScaffoldedControllerFactory(ContainerAdapter container, IActionInvokerRegistry actionInvokerRegistry, IEntityFrameworkMetadataProvider metadataProvider) : base(container, actionInvokerRegistry)
        {
            Invariant.IsNotNull(metadataProvider, "metadataProvider");

            MetadataProvider = metadataProvider;
        }

        /// <summary>
        /// Gets or sets the metadata provider.
        /// </summary>
        /// <value>The metadata provider.</value>
        protected IEntityFrameworkMetadataProvider MetadataProvider
        {
            get;
            private set;
        }

        /// <summary>
        /// Retrieves the controller type for the specified name and request context.
        /// </summary>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>The controller type.</returns>
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            EntityMetadata metadata = MetadataProvider.GetMetadata(controllerName);

            if (metadata != null)
            {
                Type controllerType = genericControllerType.MakeGenericType(metadata.EntityType, metadata.KeyType);

                return controllerType;
            }

            // Regular controller
            return base.GetControllerType(requestContext, controllerName);
        }
    }
}