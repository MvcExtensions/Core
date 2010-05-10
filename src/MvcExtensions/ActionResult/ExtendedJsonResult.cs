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
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Defines a class which is used to return json result.
    /// </summary>
    public class ExtendedJsonResult : JsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedJsonResult"/> class.
        /// </summary>
        public ExtendedJsonResult() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedJsonResult"/> class.
        /// </summary>
        /// <param name="jsonConverters">The json converters.</param>
        public ExtendedJsonResult(IEnumerable<JavaScriptConverter> jsonConverters)
        {
            JsonConverters = (jsonConverters == null) ? new List<JavaScriptConverter>() : new List<JavaScriptConverter>(jsonConverters);
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

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(ExceptionMessages.JsonGet);
            }

            HttpResponseBase httpResponse = context.HttpContext.Response;

            httpResponse.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
            {
                httpResponse.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                httpResponse.Write(Data.ToJson(JsonConverters));
            }
        }
    }
}