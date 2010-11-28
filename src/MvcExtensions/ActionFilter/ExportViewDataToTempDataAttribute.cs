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
    /// Defines an attribute which is used to copy viewdata in tempdata.
    /// <remarks>This  filter does  not execute for child action.</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ExportViewDataToTempDataAttribute : ViewDataTempDataTransferAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportViewDataToTempDataAttribute"/> class.
        /// </summary>
        public ExportViewDataToTempDataAttribute()
        {
            Order = int.MaxValue;
        }

        /// <summary>
        /// Called after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Invariant.IsNotNull(filterContext, "filterContext");

            if (filterContext.Canceled || (filterContext.Exception != null && !filterContext.ExceptionHandled))
            {
                return;
            }

            // Export if it is not an ajax request and we are redirecting
            if (filterContext.HttpContext.Request.IsAjaxRequest() || ((!(filterContext.Result is RedirectResult)) && (!(filterContext.Result is RedirectToRouteResult))))
            {
                return;
            }

            // Copy viewdata
            ViewDataDictionary viewData = new ViewDataDictionary(filterContext.Controller.ViewData);

            filterContext.Controller.TempData[Key] = viewData;
        }
    }
}