#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web.Mvc;
    using System.Web.Mvc.Async;

    /// <summary>
    /// The default async <seealso cref="IActionInvoker"/> which supports the fluent filter registration and dependency injection.
    /// </summary>
    public class ExtendedAsyncControllerActionInvoker : AsyncControllerActionInvoker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedAsyncControllerActionInvoker"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ExtendedAsyncControllerActionInvoker(ContainerAdapter container)
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
        /// Retrieves information about the action filters.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>Information about the action filters.</returns>
        protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            Invariant.IsNotNull(controllerContext, "controllerContext");
            Invariant.IsNotNull(actionDescriptor, "actionDescriptor");

            FilterInfo decoratedFilters = base.GetFilters(controllerContext, actionDescriptor);
            FilterInfo registeredFilters = Container.GetInstance<IFilterRegistry>().Matching(controllerContext, actionDescriptor);

            return ControllerActionInvokerHelper.Merge(Container, decoratedFilters, registeredFilters);
        }
    }
}