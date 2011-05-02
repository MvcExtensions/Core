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
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Defines a constraint which is used to ensure the RESTFul actions conditions are meet.
    /// </summary>
    public class RESTFulActionConstraint : IRouteConstraint
    {
        /// <summary>
        /// The Default Id parameter
        /// </summary>
        public const string IdParameterName = "id";

        private static readonly IEnumerable<HttpVerbs> knownVerbs = Enum.GetValues(typeof(HttpVerbs)).Cast<HttpVerbs>();

        private readonly IEnumerable<string> verbs;

        /// <summary>
        /// Initializes a new instance of the <see cref="RESTFulActionConstraint"/> class.
        /// </summary>
        /// <param name="httpVerb">The HTTP verb.</param>
        public RESTFulActionConstraint(HttpVerbs httpVerb) : this(httpVerb, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RESTFulActionConstraint"/> class.
        /// </summary>
        /// <param name="httpVerb">The HTTP verb.</param>
        /// <param name="requiresId">if set to <c>true</c> [requires id].</param>
        public RESTFulActionConstraint(HttpVerbs httpVerb, bool requiresId)
        {
            HttpVerbs = httpVerb;
            RequiresId = requiresId;

            verbs = ConvertToStringList(HttpVerbs);
        }

        /// <summary>
        /// Gets or sets the HTTP verbs.
        /// </summary>
        /// <value>The HTTP verbs.</value>
        public HttpVerbs HttpVerbs { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [requires id].
        /// </summary>
        /// <value><c>true</c> if [requires id]; otherwise, <c>false</c>.</value>
        public bool RequiresId { get; private set; }

        /// <summary>
        /// Determines whether the URL parameter contains a valid value for this constraint.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <param name="route">The object that this constraint belongs to.</param>
        /// <param name="parameterName">The name of the parameter that is being checked.</param>
        /// <param name="values">An object that contains the parameters for the URL.</param>
        /// <param name="routeDirection">An object that indicates whether the constraint check is being performed when an incoming request is being handled or when a URL is being generated.</param>
        /// <returns>
        /// true if the URL parameter contains a valid value; otherwise, false.
        /// </returns>
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            Invariant.IsNotNull(httpContext, "httpContext");

            if (routeDirection == RouteDirection.UrlGeneration)
            {
                // No need to check the Http verb for outbound url, we only check the id
                return !RequiresId || HasId(values);
            }

            string originalVerb = httpContext.Request.HttpMethod;

            if ((originalVerb != null) && verbs.Contains(originalVerb, StringComparer.OrdinalIgnoreCase))
            {
                return !RequiresId || HasId(values);
            }

            string overriddenVerb = httpContext.Request.GetHttpMethodOverride();

            bool matched = overriddenVerb != null && verbs.Contains(overriddenVerb, StringComparer.OrdinalIgnoreCase);

            if (matched)
            {
                return !RequiresId || HasId(values);
            }

            return false;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Join(",", verbs);
        }

        private static IEnumerable<string> ConvertToStringList(HttpVerbs verb)
        {
            IList<string> list = new List<string>();

            Action<HttpVerbs> append = matching =>
                                           {
                                               if ((verb & matching) != 0)
                                               {
                                                   list.Add(matching.ToString().ToUpperInvariant());
                                               }
                                           };

            foreach (HttpVerbs known in knownVerbs)
            {
                append(known);
            }

            return list;
        }

        private static bool HasId(IDictionary<string, object> values)
        {
            if (!values.ContainsKey(IdParameterName))
            {
                return false;
            }

            object value = values[IdParameterName];

            return value != null && !string.IsNullOrWhiteSpace(value.ToString());
        }
    }
}