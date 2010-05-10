#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Defines an class which is used to redirect adaptively in PRG scenario. If the request is ajax it writes the viewdata in json string; otherwise
    /// it redirects to the given url. 
    /// </summary>
    public class AdaptivePostRedirectGetResult : RedirectResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptivePostRedirectGetResult"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="jsonConverters">The json converters.</param>
        public AdaptivePostRedirectGetResult(string url, IEnumerable<JavaScriptConverter> jsonConverters) : base(url)
        {
            JsonConverters = (jsonConverters == null) ? new List<JavaScriptConverter>() : new List<JavaScriptConverter>(jsonConverters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptivePostRedirectGetResult"/> class.
        /// </summary>
        /// <param name="url">The target URL.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="url"/> parameter is null.</exception>
        public AdaptivePostRedirectGetResult(string url) : this(url, null)
        {
        }

        /// <summary>
        /// Gets the json converters.
        /// </summary>
        /// <value>The json converters.</value>
        public IList<JavaScriptConverter> JsonConverters
        {
            get;
            private set;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context within which the result is executed.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="context"/> parameter is null.</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            Invariant.IsNotNull(context, "context");

            HttpContextBase httpContext = context.HttpContext;
            ControllerBase controller = context.Controller;

            if (httpContext.Request.IsAjaxRequest())
            {
                controller.TempData.Keep();

                string json = controller.ViewData.ToJson(JsonConverters);

                httpContext.Response.WriteJson(json);
            }
            else
            {
                base.ExecuteResult(context);
            }
        }
    }
}