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
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// The context object when responders are into action.
    /// </summary>
    public class ResponderContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponderContext"/> class.
        /// </summary>
        public ResponderContext() : this(new RouteValueDictionary(), new Dictionary<string, string>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponderContext"/> class.
        /// </summary>
        /// <param name="redirectRouteValues">The redirect route values.</param>
        /// <param name="flashMessages">The flash messages.</param>
        protected ResponderContext(RouteValueDictionary redirectRouteValues, IDictionary<string, string> flashMessages)
        {
            if (redirectRouteValues == null)
            {
                throw new ArgumentNullException("redirectRouteValues");
            }

            if (flashMessages == null)
            {
                throw new ArgumentNullException("flashMessages");
            }

            RedirectRouteValues = redirectRouteValues;
            FlashMessages = flashMessages;
        }

        /// <summary>
        /// Gets or sets the controller context.
        /// </summary>
        /// <value>The controller context.</value>
        public ControllerContext ControllerContext { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public object Model { get; set; }

        /// <summary>
        /// Gets or sets the name of the view.
        /// </summary>
        /// <value>The name of the view.</value>
        public string ViewName { get; set; }

        /// <summary>
        /// Gets or sets the name of the master.
        /// </summary>
        /// <value>The name of the master.</value>
        public string MasterName { get; set; }

        /// <summary>
        /// Gets or sets the redirect controller.
        /// </summary>
        /// <value>The redirect controller.</value>
        public string RedirectController { get; set; }

        /// <summary>
        /// Gets or sets the redirect action.
        /// </summary>
        /// <value>The redirect action.</value>
        public string RedirectAction { get; set; }

        /// <summary>
        /// Gets or sets the redirect route values.
        /// </summary>
        /// <value>The redirect route values.</value>
        public RouteValueDictionary RedirectRouteValues { get; internal set; }

        /// <summary>
        /// Gets or sets the flash messages.
        /// </summary>
        /// <value>The flash messages.</value>
        public IDictionary<string, string> FlashMessages { get; internal set; }
    }
}