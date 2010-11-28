#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web;
    using System.Web.Mvc;
    using System.Xml.Serialization;

    /// <summary>
    /// Defines a class which is used to return xml result.
    /// </summary>
    public class XmlResult : ActionResult
    {
        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value></value>
        /// <returns>The type of the content.</returns>
        public string ContentType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value></value>
        /// <returns>The data.</returns>
        public object Data
        {
            get;
            set;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            Invariant.IsNotNull(context, "context");

            HttpResponseBase httpResponse = context.HttpContext.Response;

            httpResponse.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/xml";

            if (Data == null)
            {
                return;
            }

            XmlSerializer serializer = new XmlSerializer(Data.GetType());
            serializer.Serialize(httpResponse.Output, Data);
        }
    }
}