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
    /// Defines a class to store the <see cref="FilterAttribute"/> factories of <seealso cref="Controller"/>.
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    public class FilterRegistryControllerItem<TController> : FilterRegistryItem where TController : Controller
    {
        private readonly Type controllerType = typeof(TController);

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegistryControllerItem&lt;TController&gt;"/> class.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public FilterRegistryControllerItem(IEnumerable<Func<IMvcFilter>> filters) 
            : base(filters, FilterScope.Controller)
        {
        }

        /// <summary>
        /// Determines whether the specified controller context is matching.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>
        /// <c>true</c> if the specified controller context is matching; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsMatching(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return (controllerContext != null) && controllerType.IsAssignableFrom(controllerContext.Controller.GetType());
        }
    }
}