#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Defines an class which is used to render a view adaptively. If the request is ajax it writes the viewdata in json string; otherwise
    /// it  renders the specified view. 
    /// </summary>
    public class AdaptiveViewResult : ViewResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptiveViewResult"/> class.
        /// </summary>
        /// <param name="jsonConverters">The json converters.</param>
        public AdaptiveViewResult(IEnumerable<JavaScriptConverter> jsonConverters)
        {
            JsonConverters = (jsonConverters == null) ? new List<JavaScriptConverter>() : new List<JavaScriptConverter>(jsonConverters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptiveViewResult"/> class.
        /// </summary>
        public AdaptiveViewResult() : this(null)
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
        /// When called by the action invoker, renders the view to the response.
        /// </summary>
        /// <param name="context">The context that the result is executed in.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="context"/> parameter is null.</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            Invariant.IsNotNull(context, "context");

            if (context.HttpContext.Request.IsAjaxRequest())
            {
                string json = ViewData.ToJson(JsonConverters);

                context.HttpContext.Response.WriteJson(json);
            }
            else
            {
                base.ExecuteResult(context);
            }
        }
    }
}