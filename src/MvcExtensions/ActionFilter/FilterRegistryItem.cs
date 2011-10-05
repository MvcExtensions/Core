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
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a base class to store the <see cref="FilterAttribute"/> factories.
    /// </summary>
    public abstract class FilterRegistryItem
    {
        private readonly FilterScope filterScope;

        private readonly IEnumerable<Func<IMvcFilter>> filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegistryItem"/> class.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <param name="filterScope"></param>
        protected FilterRegistryItem(IEnumerable<Func<IMvcFilter>> filters, FilterScope filterScope)
        {
            Invariant.IsNotNull(filters, "filters");

            this.filters = filters;
            this.filterScope = filterScope;
        }

        /// <summary>
        /// Determines whether this filter matches with give controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>
        /// <c>true</c> if the specified controller context is matching; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsMatching(ControllerContext controllerContext, ActionDescriptor actionDescriptor);

        /// <summary>
        /// Get the <see cref="Filter"/> metadatas
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Filter> GetFilters()
        {
            return filters.Select(x => x())
                .Select(x => new Filter(x, filterScope, x.Order));
        }
    }
}