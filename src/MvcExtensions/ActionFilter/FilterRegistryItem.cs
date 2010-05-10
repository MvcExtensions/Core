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
    using System.Web.Mvc;

    /// <summary>
    /// Defines a base class to store the <see cref="FilterAttribute"/> factories.
    /// </summary>
    public abstract class FilterRegistryItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegistryItem"/> class.
        /// </summary>
        /// <param name="filters">The filters.</param>
        protected FilterRegistryItem(IEnumerable<Func<FilterAttribute>> filters)
        {
            Invariant.IsNotNull(filters, "filters");

            Filters = filters;
        }

        /// <summary>
        /// Gets or sets the filter factories.
        /// </summary>
        /// <value>The filters.</value>
        public IEnumerable<Func<FilterAttribute>> Filters
        {
            get;
            protected set;
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
    }
}