#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Defines an attribute which is used to support select list elements
    /// <remarks>This  filter is applicable for child action only.</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SelectListActionAttribute : FilterAttribute, IActionFilter
    {
        private const string DefaultArgumentName = "selected";

        /// <summary>
        /// Get or sets the name of argument which is used to pass selected value into the action method. 
        /// </summary>
        public string ArgumentName
        {
            get;
            set;
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// </summary>
        /// <param name="context">The filter context.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var result = (ViewResultBase)context.Result;
            CopyViewDataProperties(context.ParentActionViewContext.ViewData, result.ViewData);
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="context">The filter context.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.ActionParameters[ArgumentName ?? DefaultArgumentName] = GetSelectedValue(context);
        }

        private static void CopyViewDataProperties(ViewDataDictionary source, ViewDataDictionary destination)
        {
            destination.ModelMetadata = source.ModelMetadata;
            destination.TemplateInfo.FormattedModelValue = source.TemplateInfo.FormattedModelValue;
            destination.TemplateInfo.HtmlFieldPrefix = source.TemplateInfo.HtmlFieldPrefix;
        }

        private static object GetAttemptedValue(ViewDataDictionary viewData)
        {
            ModelState modelState;
            if (viewData.ModelState.TryGetValue(viewData.ModelMetadata.PropertyName, out modelState) && modelState.Value != null)
            {
                return modelState.Value.ConvertTo(viewData.ModelMetadata.ModelType, null);
            }

            return null;
        }

        private static object GetSelectedValue(ControllerContext context)
        {
            var viewData = context.ParentActionViewContext.ViewData;
            return GetAttemptedValue(viewData) ?? viewData.Model;
        }
    }
}
