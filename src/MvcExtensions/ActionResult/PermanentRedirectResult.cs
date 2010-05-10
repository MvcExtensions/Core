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
    /// Defines a class which is used to return  Http 301 status code.
    /// </summary>
    public class PermanentRedirectResult : RedirectResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermanentRedirectResult"/> class.
        /// </summary>
        /// <param name="url">The target URL.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="url"/> parameter is null.</exception>
        public PermanentRedirectResult(string url) : base(url)
        {
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context within which the result is executed.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="context"/> parameter is null.</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            Invariant.IsNotNull(context, "context");

            HttpResponseBase response = context.HttpContext.Response;

            response.Clear();
            response.StatusCode = (int)HttpStatusCode.MovedPermanently;
            response.Status = "301 Moved Permanently";
            response.RedirectLocation = Url;
            response.SuppressContent = true;
        }
    }
}