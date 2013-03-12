#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Web.Mvc;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines an attribute which is used to support select list elements
    /// <remarks>This  filter is applicable for child action only.</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    [TypeForwardedFrom(KnownAssembly.MvcExtensions)]
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
        public void OnActionExecuted([NotNull] ActionExecutedContext context)
        {
            var result = context.Result as ViewResultBase;
            if (result != null)
            {
                CopyViewDataProperties(context.ParentActionViewContext.ViewData, result.ViewData);
            }
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="context">The filter context.</param>
        public void OnActionExecuting([NotNull] ActionExecutingContext context)
        {
            context.ActionParameters[ArgumentName ?? DefaultArgumentName] = GetSelectedValue(context);
        }

        private static void CopyViewDataProperties([NotNull] ViewDataDictionary source, [NotNull] ViewDataDictionary destination)
        {
            destination.ModelMetadata = source.ModelMetadata;
            destination.TemplateInfo.FormattedModelValue = source.TemplateInfo.FormattedModelValue;
            destination.TemplateInfo.HtmlFieldPrefix = source.TemplateInfo.HtmlFieldPrefix;
        }

        [CanBeNull]
        private static Type ExtractGenericInterface([NotNull] Type queryType, Type interfaceType)
        {
            Func<Type, bool> predicate =
                t =>
                    {
                        if (t.IsGenericType)
                        {
                            return t.GetGenericTypeDefinition() == interfaceType;
                        }

                        return false;
                    };

            if (predicate(queryType))
            {
                return queryType;
            }

            return queryType.GetInterfaces().FirstOrDefault(predicate);
        }

        private static object GetAttemptedValue([NotNull] ViewDataDictionary viewData)
        {
            ModelState modelState;
            if (viewData.ModelState.TryGetValue(viewData.ModelMetadata.PropertyName, out modelState) && modelState.Value != null)
            {
                return modelState.Value.ConvertTo(GetModelType(viewData.ModelMetadata.ModelType), null);
            }

            return null;
        }

        [NotNull]
        private static Type GetModelType([NotNull] Type type)
        {
            if (type.IsArray || type == typeof(string))
            {
                return type;
            }

            var enumerableType = ExtractGenericInterface(type, typeof(IEnumerable<>));
            if (enumerableType == null)
            {
                return type;
            }

            var elementType = enumerableType.GetGenericArguments()[0];
            return elementType.MakeArrayType();
        }

        private static object GetSelectedValue([NotNull] ControllerContext context)
        {
            var viewData = context.ParentActionViewContext.ViewData;
            return GetAttemptedValue(viewData) ?? viewData.Model;
        }
    }
}
