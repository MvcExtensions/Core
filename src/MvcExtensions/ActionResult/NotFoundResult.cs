#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class which is used to return Http 404 status code.
    /// </summary>
    public class NotFoundResult : ActionResult
    {
        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            Invariant.IsNotNull(context, "context");

            HttpResponseBase response = context.HttpContext.Response;

            response.Clear();
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Status = "404 Page Not Found";
        }
    }
}