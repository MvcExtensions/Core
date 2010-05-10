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

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// The default filter registry which supports fluent registration.
    /// </summary>
    public class FilterRegistry : IFilterRegistry
    {
        private readonly IList<FilterRegistryItem> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegistry"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        public FilterRegistry(IServiceLocator serviceLocator)
        {
            Invariant.IsNotNull(serviceLocator, "serviceLocator");

            ServiceLocator = serviceLocator;
            items = new List<FilterRegistryItem>();
        }

        /// <summary>
        /// Gets the service locator.
        /// </summary>
        /// <value>The service locator.</value>
        public IServiceLocator ServiceLocator
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
            where TController : Controller where TFilter : FilterAttribute
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
            where TFilter : FilterAttribute
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

            IList<FilterAttribute> authorizationFilters = new List<FilterAttribute>();
            IList<FilterAttribute> actionFilters = new List<FilterAttribute>();
            IList<FilterAttribute> resultFilters = new List<FilterAttribute>();
            IList<FilterAttribute> exceptionFiltes = new List<FilterAttribute>();

            foreach (IEnumerable<FilterAttribute> filters in Items.Where(item => item.IsMatching(controllerContext, actionDescriptor))
                                                                  .Select(item => item.Filters.Select(filter => filter())))
            {
                filters.OfType<IAuthorizationFilter>()
                    .Cast<FilterAttribute>()
                    .Each(authorizationFilters.Add);

                filters.OfType<IActionFilter>()
                    .Cast<FilterAttribute>()
                    .Each(actionFilters.Add);

                filters.OfType<IResultFilter>()
                    .Cast<FilterAttribute>()
                    .Each(resultFilters.Add);

                filters.OfType<IExceptionFilter>()
                    .Cast<FilterAttribute>()
                    .Each(exceptionFiltes.Add);
            }

            FilterInfo filterInfo = new FilterInfo();

            authorizationFilters.OrderBy(filter => filter.Order)
                                .Cast<IAuthorizationFilter>()
                                .Each(filter => filterInfo.AuthorizationFilters.Add(filter));

            actionFilters.OrderBy(filter => filter.Order)
                         .Cast<IActionFilter>()
                         .Each(filter => filterInfo.ActionFilters.Add(filter));

            resultFilters.OrderBy(filter => filter.Order)
                         .Cast<IResultFilter>()
                         .Each(filter => filterInfo.ResultFilters.Add(filter));

            exceptionFiltes.OrderBy(filter => filter.Order)
                           .Cast<IExceptionFilter>()
                           .Each(filter => filterInfo.ExceptionFilters.Add(filter));

            return filterInfo;
        }

        private static IEnumerable<Func<FilterAttribute>> ConvertFilters<TFilter>(IEnumerable<Func<TFilter>> filters)
            where TFilter : FilterAttribute
        {
            return filters.Select(filter => new Func<FilterAttribute>(() => filter()));
        }
    }
}