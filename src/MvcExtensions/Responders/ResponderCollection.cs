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
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Contains the supported responders
    /// </summary>
    public class ResponderCollection : Collection<IResponder>
    {
        private const string DefaultQueryStringFormatParameterName = "format";

        private static string queryStringFormatParameterName = DefaultQueryStringFormatParameterName;

        /// <summary>
        /// Gets or sets the name of the query string format parameter.
        /// </summary>
        /// <value>The name of the query string format parameter.</value>
        public static string QueryStringFormatParameterName
        {
            [DebuggerStepThrough]
            get
            {
                if (string.IsNullOrWhiteSpace(queryStringFormatParameterName))
                {
                    queryStringFormatParameterName = DefaultQueryStringFormatParameterName;
                }

                return queryStringFormatParameterName;
            }

            [DebuggerStepThrough]
            set
            {
                queryStringFormatParameterName = value;
            }
        }

        /// <summary>
        /// Finds the matching.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual IResponder FindMatching(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            string action = context.RouteData.ActionName();

            // Filter the responders which does not has this action in exclude list.
            IEnumerable<IResponder> filtered = Items.Where(r => r.CanRespondToAction(action)).ToList();

            string format = context.HttpContext.Request.QueryString[QueryStringFormatParameterName];

            IResponder responder = filtered.FirstOrDefault(r => r.CanRespondToFormat(format));

            // Found matched responder, no need to go further
            if (responder != null)
            {
                return responder;
            }

            // If explicit format is specified but no matching responder exist, do not need to continue
            if (!string.IsNullOrWhiteSpace(format))
            {
                return null;
            }

            // No explicit format is specified, so we have to detect the format from the 
            // accepted mime types.
            // Webkit sends wrongly ordered accept types, so we have to identify it
            // more info http://www.gethifi.com/blog/webkit-team-admits-accept-header-error
            bool isDefectiveBrowser = context.HttpContext.Request.Browser.IsBrowser("chrome") ||
                                      context.HttpContext.Request.Browser.IsBrowser("safari");

            IEnumerable<string> mimeTypes = QValueSorter.Sort(context.HttpContext.Request.AcceptTypes, isDefectiveBrowser);

            foreach (string mimeType in mimeTypes)
            {
                string type = mimeType;

                responder = filtered.FirstOrDefault(r => r.CanRespondToMimeType(type));

                if (responder != null)
                {
                    break;
                }
            }

            return responder;
        }

        /// <summary>
        /// Sets the item.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        protected override void SetItem(int index, IResponder item)
        {
            Invariant.IsNotNull(item, "item");
            Type responderType = item.GetType();

            if (Items.Select(x => x.GetType()).Any(type => type == responderType))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, ExceptionMessages.SameResponderExists, responderType.FullName), "item");
            }

            base.SetItem(index, item);
        }
    }
}