#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Defines an abstract class for data responder.
    /// </summary>
    public abstract class SerializableResponder : Responder
    {
        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        protected string ContentType { get; set; }

        /// <summary>
        /// Responds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Respond(ResponderContext context)
        {
            Invariant.IsNotNull(context, "context");

            ControllerContext controllerContext = context.ControllerContext;
            string action = controllerContext.RouteData.ActionName().ToLower(CultureInfo.CurrentCulture);
            ModelStateDictionary modelState = controllerContext.Controller.ViewData.ModelState;
            HttpResponseBase httpResponse = controllerContext.HttpContext.Response;

            if (KnownActionNames.Destroy.Equals(action))
            {
                BuildResponse(null, (int)HttpStatusCode.OK, httpResponse);
                return;
            }

            if (!modelState.IsValid && KnownActionNames.CreateAndUpdate().Contains(action))
            {
                IEnumerable<ModelStateError> errors = modelState.Select(ms => new { key = ms.Key, errors = ms.Value.Errors.Select(error => (error.Exception == null) ? error.ErrorMessage : error.Exception.Message) })
                                                                .Where(ms => ms.errors.Any()) // No need to include model state that does not have  error
                                                                .Select(e => new ModelStateError { Key = e.key, Messages = e.errors.ToList() })
                                                                .ToList();

                BuildResponse(errors, 422, httpResponse); // No equivalent  .NET HttpStatusCode, Unprocessable Entity, ref http://www.iana.org/assignments/http-status-codes
                return;
            }

            BuildResponse(context.Model, (int)(KnownActionNames.Create.Equals(action) ? HttpStatusCode.Created : HttpStatusCode.OK), controllerContext.HttpContext.Response);
        }

        /// <summary>
        /// Writes to response output.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="output">The output.</param>
        protected abstract void WriteTo(object model, TextWriter output);

        private void BuildResponse(object model, int httpStatusCode, HttpResponseBase response)
        {
            response.ContentType = ContentType;
            response.StatusCode = httpStatusCode;
            WriteTo(model, response.Output);
        }
    }
}