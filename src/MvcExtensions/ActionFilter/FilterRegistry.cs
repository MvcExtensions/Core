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
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    /// <summary>
    /// The default filter registry which supports fluent registration.
    /// </summary>
    public class FilterRegistry : IFilterRegistry, IFilterProvider
    {
        private readonly IList<FilterRegistryItem> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegistry"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public FilterRegistry(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");

            Container = container;
            items = new List<FilterRegistryItem>();
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public ContainerAdapter Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the registered items.
        /// </summary>
        /// <value>The items.</value>
        public virtual IList<FilterRegistryItem> Items
        {
            [DebuggerStepThrough]
            get
            {
                return items;
            }
        }

        /// <summary>
        /// Registers the specified filters for the given controller.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <param name="filters">The filters.</param>
        /// <returns></returns>
        public virtual IFilterRegistry Register<TController, TFilter>(IEnumerable<Func<TFilter>> filters)
            where TController : Controller where TFilter : IMvcFilter
        {
            Invariant.IsNotNull(filters, "filters");

            if (filters.Any())
            {
                Items.Add(new FilterRegistryControllerItem<TController>(ConvertFilters(filters)));
            }

            return this;
        }

        /// <summary>
        /// Registers the specified filters for the given controller action.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="filters">The filters.</param>
        /// <returns></returns>
        public virtual IFilterRegistry Register<TController, TFilter>(Expression<Action<TController>> action, IEnumerable<Func<TFilter>> filters)
            where TController : Controller
            where TFilter : IMvcFilter
        {
            Invariant.IsNotNull(action, "action");
            Invariant.IsNotNull(filters, "filters");

            if (filters.Any())
            {
                Items.Add(new FilterRegistryActionItem<TController>(action, ConvertFilters(filters)));
            }

            return this;
        }

        /// <summary>
        /// Returns the matching filters for the given controller action.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns></returns>
        public virtual FilterInfo Matching(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            Invariant.IsNotNull(controllerContext, "controllerContext");
            Invariant.IsNotNull(actionDescriptor, "actionDescriptor");

            var filters = GetFilters(controllerContext, actionDescriptor);

            return new FilterInfo(filters);
        }

        /// <summary>
        /// Returns an enumerator that contains all the <see cref="T:System.Web.Mvc.IFilterProvider"/> instances in the service locator.
        /// </summary>
        /// <returns>
        /// The enumerator that contains all the <see cref="T:System.Web.Mvc.IFilterProvider"/> instances in the service locator.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="actionDescriptor">The action descriptor.</param>
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = Items.Where(item => item.IsMatching(controllerContext, actionDescriptor))
                .SelectMany(item => item.GetFilters())
                .OrderBy(x => x.Order)
                .ToArray();

            filters.Each(Container.Inject);

            return filters;
        }

        private static IEnumerable<Func<IMvcFilter>> ConvertFilters<TFilter>(IEnumerable<Func<TFilter>> filters)
            where TFilter : IMvcFilter
        {
            return filters.Select(filter => new Func<IMvcFilter>(() => filter() as IMvcFilter));
        }
    }
}