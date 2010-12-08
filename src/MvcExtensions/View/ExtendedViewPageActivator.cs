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
    /// Defines a class which is used to create view.
    /// </summary>
    public class ExtendedViewPageActivator : IViewPageActivator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedViewPageActivator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="viewActivatorRegistry">The view activator registry.</param>
        public ExtendedViewPageActivator(ContainerAdapter container, TypeMappingRegistry<IView, IViewPageActivator> viewActivatorRegistry)
        {
            Invariant.IsNotNull(container, "container");
            Invariant.IsNotNull(viewActivatorRegistry, "viewActivatorRegistry");

            Container = container;
            ViewActivatorRegistry = viewActivatorRegistry;
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
        protected TypeMappingRegistry<IView, IViewPageActivator> ViewActivatorRegistry
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public object Create(ControllerContext controllerContext, Type type)
        {
            if (type == null)
            {
                return null;
            }

            Type activatorType = ViewActivatorRegistry.Matching(type);

            IViewPageActivator activator = activatorType != null ?
                                           Container.GetServices(activatorType) as IViewPageActivator :
                                           null;

            object view = activator != null ?
                          activator.Create(controllerContext, type) :
                          Container.GetService(type) ?? Activator.CreateInstance(type);

            return view;
        }
    }
}