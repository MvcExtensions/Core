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
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Defines an action result which is used to find matching responder.
    /// </summary>
    public class RespondWithResult : ActionResult
    {
        private readonly ResponderContext responderContext = new ResponderContext();

        /// <summary>
        /// Initializes a new instance of the <see cref="RespondWithResult"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public RespondWithResult(object model)
        {
            responderContext.Model = model;
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>The model.</value>
        public object Model
        {
            [DebuggerStepThrough]
            get { return responderContext.Model; }
        }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>The name of the view.</value>
        public string ViewName
        {
            [DebuggerStepThrough]
            get { return responderContext.ViewName; }
        }

        /// <summary>
        /// Gets the name of the master.
        /// </summary>
        /// <value>The name of the master.</value>
        public string MasterName
        {
            [DebuggerStepThrough]
            get { return responderContext.MasterName;  }
        }

        /// <summary>
        /// Gets the redirect controller.
        /// </summary>
        /// <value>The redirect controller.</value>
        public string RedirectController
        {
            [DebuggerStepThrough]
            get { return responderContext.RedirectController; }
        }

        /// <summary>
        /// Gets the redirect action.
        /// </summary>
        /// <value>The redirect action.</value>
        public string RedirectAction
        {
            [DebuggerStepThrough]
            get { return responderContext.RedirectAction; }
        }

        /// <summary>
        /// Gets the redirect route values.
        /// </summary>
        /// <value>The redirect route values.</value>
        public RouteValueDictionary RedirectRouteValues
        {
            [DebuggerStepThrough]
            get { return responderContext.RedirectRouteValues; }
        }

        /// <summary>
        /// Gets the flash messages.
        /// </summary>
        /// <value>The flash messages.</value>
        public IDictionary<string, string> FlashMessages
        {
            [DebuggerStepThrough]
            get { return responderContext.FlashMessages; }
        }

        /// <summary>
        /// Withes the view.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns></returns>
        public RespondWithResult WithView(string viewName)
        {
            return WithView(viewName, null);
        }

        /// <summary>
        /// Withes the view.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="masterName">Name of the master.</param>
        /// <returns></returns>
        public virtual RespondWithResult WithView(string viewName, string masterName)
        {
            responderContext.ViewName = viewName;
            responderContext.MasterName = masterName;

            return this;
        }

        /// <summary>
        /// Withes the redirect.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public RespondWithResult WithRedirect(string action)
        {
            return WithRedirect(action, null, null);
        }

        /// <summary>
        /// Withes the redirect.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public RespondWithResult WithRedirect(string action, object routeValues)
        {
            return WithRedirect(action, null, routeValues);
        }

        /// <summary>
        /// Withes the redirect.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="controller">The controller.</param>
        /// <returns></returns>
        public RespondWithResult WithRedirect(string action, string controller)
        {
            return WithRedirect(action, controller, null);
        }

        /// <summary>
        /// Withes the redirect.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public virtual RespondWithResult WithRedirect(string action, string controller, object routeValues)
        {
            responderContext.RedirectAction = action;
            responderContext.RedirectController = controller;
            responderContext.RedirectRouteValues = new RouteValueDictionary(ToDictionary(routeValues));

            return this;
        }

        /// <summary>
        /// Withes the flash.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public virtual RespondWithResult WithFlash(object messages)
        {
            responderContext.FlashMessages = ToDictionary(messages).ToDictionary(d => d.Key, d => d.Value.ToString());

            return this;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            Invariant.IsNotNull(context, "context");

            IHasResponders controller = context.Controller as IHasResponders;

            if (controller == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, "Controller \"{0}\" does not support respond with.", context.RouteData.ControllerName()));
            }

            IResponder responder = controller.Responders.FindMatching(context);

            if (responder == null)
            {
                new HttpStatusCodeResult((int)HttpStatusCode.UnsupportedMediaType).ExecuteResult(context);
                return;
            }

            responderContext.ControllerContext = context;

            responder.Respond(responderContext);
        }

        private static IDictionary<string, object> ToDictionary(object arguments)
        {
            if (arguments == null)
            {
                return new Dictionary<string, object>();
            }

            return arguments.GetType()
                            .GetProperties()
                            .Where(p => p.CanRead && !p.GetIndexParameters().Any())
                            .ToDictionary(p => p.Name, p => p.GetValue(arguments, null));
        }
    }
}