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
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Defines a responder which is used to return html.
    /// </summary>
    public class HtmlResponder : Responder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlResponder"/> class.
        /// </summary>
        public HtmlResponder()
        {
            SupportedFormat = "html";

            foreach (string mimeType in KnownMimeTypes.HtmlTypes())
            {
                SupportedMimeTypes.Add(mimeType);
            }
        }

        /// <summary>
        /// Responds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Respond(ResponderContext context)
        {
            Invariant.IsNotNull(context, "context");

            if (RespondToExplicitView(context))
            {
                return;
            }

            if (RespondToExplicitRedirect(context))
            {
                return;
            }

            // Nothing explicit so continue with default behavior
            ControllerContext controllerContext = context.ControllerContext;
            ControllerBase controller = controllerContext.Controller;

            string action = controllerContext.RouteData.ActionName().ToLower(CultureInfo.CurrentCulture);

            if (KnownActionNames.Destroy.Equals(action) ||
               (controller.ViewData.ModelState.IsValid && KnownActionNames.CreateAndUpdate().Contains(action)))
            {
                InjectFlashMessages(context);
                new RedirectToRouteResult(new RouteValueDictionary(new { action = KnownActionNames.Index })).ExecuteResult(controllerContext);
                return;
            }

            controller.ViewData.Model = context.Model;
            string view = action;

            // If we reached here for create or update action, then the model state is not valid
            // so we have to toggle the view name
            if (KnownActionNames.Create.Equals(action))
            {
                view = KnownActionNames.New;
            }
            else if (KnownActionNames.Update.Equals(action))
            {
                view = KnownActionNames.Edit;
            }
            else
            {
                InjectFlashMessages(context);
            }

            (new ViewResult { ViewName = view, ViewData = controller.ViewData, TempData = controller.TempData }).ExecuteResult(controllerContext);
        }

        private static bool RespondToExplicitRedirect(ResponderContext context)
        {
            if (string.IsNullOrWhiteSpace(context.RedirectAction) && string.IsNullOrWhiteSpace(context.RedirectController) && !context.RedirectRouteValues.Any())
            {
                // No explicit view specified so do not continue.
                return false;
            }

            InjectFlashMessages(context);

            RouteValueDictionary mergedRouteValues = RouteValuesHelpers.MergeRouteValues(context.RedirectAction, context.RedirectController, !context.RedirectRouteValues.Any() ? null : context.ControllerContext.RouteData.Values, context.RedirectRouteValues, true);

            new RedirectToRouteResult(mergedRouteValues).ExecuteResult(context.ControllerContext);

            return false;
        }

        private static bool RespondToExplicitView(ResponderContext context)
        {
            if (string.IsNullOrWhiteSpace(context.ViewName) && string.IsNullOrWhiteSpace(context.MasterName))
            {
                // No explicit view specified so do not continue.
                return false;
            }

            InjectFlashMessages(context);

            ControllerContext controllerContext = context.ControllerContext;

            // if explicit view is specified then use it otherwise fallback to action name;
            string view = context.ViewName ?? controllerContext.RouteData.ActionName();

            ControllerBase controller = controllerContext.Controller;

            (new ViewResult { ViewName = view, MasterName = context.MasterName, ViewData = controller.ViewData, TempData = controller.TempData }).ExecuteResult(controllerContext);

            return true;
        }

        private static void InjectFlashMessages(ResponderContext context)
        {
            if (!context.FlashMessages.Any())
            {
                return;
            }

            FlashStorage flashStorage = new FlashStorage(context.ControllerContext.Controller.TempData);

            foreach (KeyValuePair<string, string> pair in context.FlashMessages)
            {
                flashStorage.Add(pair.Key, pair.Value);
            }
        }
    }
}