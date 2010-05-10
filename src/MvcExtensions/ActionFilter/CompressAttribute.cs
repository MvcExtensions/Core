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
    /// Defines a class which is used to compress the response with the client supported algorithm.
    /// <remarks>This  filter does  not execute for child action.</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CompressAttribute : FilterAttribute, IResultFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompressAttribute"/> class.
        /// </summary>
        public CompressAttribute()
        {
            Order = int.MaxValue;
        }

        /// <summary>
        /// Called before an action result executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // Do nothing just sleep
        }

        /// <summary>
        /// Called after an action result executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Invariant.IsNotNull(filterContext, "filterContext");

            if (filterContext.IsChildAction)
            {
                return;
            }

            if (filterContext.Canceled || (filterContext.Exception != null && !filterContext.ExceptionHandled))
            {
                return;
            }

            filterContext.HttpContext.Compress();
        }
    }
}