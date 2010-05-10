#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Routing;

    /// <summary>
    /// Defines a class which is used to ensure the URL parameter value is  a matching enum value.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    [CLSCompliant(false)]
    public class EnumConstraint<TEnum> : IRouteConstraint where TEnum : IComparable, IFormattable, IConvertible
    {
        private readonly bool optional;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumConstraint&lt;TEnum&gt;"/> class.
        /// </summary>
        public EnumConstraint() : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumConstraint&lt;TEnum&gt;"/> class.
        /// </summary>
        /// <param name="optional">if set to <c>true</c> [optional].</param>
        public EnumConstraint(bool optional)
        {
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

            if (value == null)
            {
                return optional;
            }

            bool matched = Enum.GetNames(typeof(TEnum)).Contains(value.ToString(), StringComparer.OrdinalIgnoreCase);

            return matched;
        }
    }
}