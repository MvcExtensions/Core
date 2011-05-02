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

    /// <summary>
    /// Defines an action result which is used to wrap another action result.
    /// </summary>
    /// <typeparam name="TActionResult">The type of the action result.</typeparam>
    public class WrappedActionResultWithFlash<TActionResult> : ActionResult where TActionResult : ActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WrappedActionResultWithFlash&lt;TActionResult&gt;"/> class.
        /// </summary>
        /// <param name="wrappingResult">The wrapping result.</param>
        /// <param name="flashMessages">The flash messages.</param>
        public WrappedActionResultWithFlash(TActionResult wrappingResult, IDictionary<string, string> flashMessages)
        {
            Invariant.IsNotNull(wrappingResult, "wrappingResult");
            Invariant.IsNotNull(flashMessages, "flashMessages");

            WrappingResult = wrappingResult;
            FlashMessages = flashMessages;
        }

        /// <summary>
        /// Gets or sets the wrapping result.
        /// </summary>
        /// <value>The wrapping result.</value>
        public TActionResult WrappingResult { get; private set; }

        /// <summary>
        /// Gets or sets the flash messages.
        /// </summary>
        /// <value>The flash messages.</value>
        public IDictionary<string, string> FlashMessages { get; private set; }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            Invariant.IsNotNull(context, "context");

            FlashStorage storage = new FlashStorage(context.Controller.TempData);

            foreach (KeyValuePair<string, string> pair in FlashMessages)
            {
                storage.Add(pair.Key, pair.Value);
            }

            WrappingResult.ExecuteResult(context);
        }
    }
}