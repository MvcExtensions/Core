#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Routing;

    /// <summary>
    /// Defines a class which is used to ensure the URL parameter value must match the provided expression.
    /// </summary>
    public class RegexConstraint : IRouteConstraint
    {
        private readonly Regex expression;
        private readonly bool optional;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexConstraint"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public RegexConstraint(string expression) : this(expression, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexConstraint"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="optional">if set to <c>true</c> [optional].</param>
        public RegexConstraint(string expression, bool optional)
        {
            Invariant.IsNotNull(expression, "expression");

            string pattern = "^(" + expression + ")$";

            this.expression = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            this.optional = optional;
        }

        /// <summary>
        /// Matches the specified HTTP context.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="route">The route.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="values">The values.</param>
        /// <param name="routeDirection">The route direction.</param>
        /// <returns></returns>
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            Invariant.IsNotNull(values, "values");

            object value = values[parameterName];

            return value == null ? optional : expression.IsMatch(value.ToString());
        }
    }
}