#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    /// <summary>
    /// Defines an abstract controller which contains  CRUD actions in RESTFul way.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public abstract class ActionController<TKey> : Controller, IHasResponders, IRESTFulCreate, IRESTFulUpdate<TKey>, IRESTFulDestroy<TKey>, IRESTFulDetails<TKey>, IRESTFulList
    {
        private static readonly Func<ResponderCollection> defaultRespondersFactory = () => new ResponderCollection
                                                                                               {
                                                                                                   new XmlResponder(),
                                                                                                   new JsonResponder(),
                                                                                                   new HtmlResponder()
                                                                                               };

        private static Func<ResponderCollection> respondersFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionController&lt;TKey&gt;"/> class.
        /// </summary>
        protected ActionController()
        {
            Responders = RespondersFactory();
        }

        /// <summary>
        /// Gets or sets the responders factory.
        /// </summary>
        /// <value>The responders factory.</value>
        public static Func<ResponderCollection> RespondersFactory
        {
            [DebuggerStepThrough]
            get { return respondersFactory ?? defaultRespondersFactory; }

            [DebuggerStepThrough]
            set { respondersFactory = value; }
        }

        /// <summary>
        /// Gets the responders.
        /// </summary>
        /// <value>The responders.</value>
        public ResponderCollection Responders { get; private set; }

        /// <summary>
        /// Shows the resource list.
        /// </summary>
        /// <returns></returns>
        public abstract ActionResult Index();

        /// <summary>
        /// Shows the individual resource.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public abstract ActionResult Show(TKey id);

        /// <summary>
        /// Shows the form for creating resource.
        /// </summary>
        /// <returns></returns>
        public abstract ActionResult New();

        /// <summary>
        /// Creates the resource.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        public abstract ActionResult Create(FormCollection fields);

        /// <summary>
        /// Shows the edit form to update resource.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public abstract ActionResult Edit(TKey id);

        /// <summary>
        /// Updates the resource.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        public abstract ActionResult Update(TKey id, FormCollection fields);

        /// <summary>
        /// Destroys the resource.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public abstract ActionResult Destroy(TKey id);

        /// <summary>
        /// Responds the with.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected virtual RespondWithResult RespondWith(object model)
        {
            return new RespondWithResult(model);
        }

        /// <summary>
        /// Called when a request matches this controller, but no method with the specified action name is found in the controller.
        /// </summary>
        /// <param name="actionName">The name of the attempted action.</param>
        protected override void HandleUnknownAction(string actionName)
        {
            if (!string.IsNullOrWhiteSpace(actionName))
            {
                // We have to set the correct HttpStatus code if the 
                // unknown action is one of our golden 7 actions.
                if (KnownActionNames.All().Contains(actionName, StringComparer.OrdinalIgnoreCase))
                {
                    // if we know the action, the obvious reason it does not match due to 
                    // the incorrect http method, so lets set the correct http status code
                    // to notify the client.
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                    return;
                }
            }

            base.HandleUnknownAction(actionName);
        }
    }
}