#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Web;
    using System.Web.Routing;

    /// <summary>
    /// Defines a class which is used to ensure the URL parameter value is  between the specified range.
    /// </summary>
    [CLSCompliant(false)]
    public class RangeConstraint<TValue> : IRouteConstraint where TValue : IComparable, IConvertible
    {
        private readonly TValue min;
        private readonly TValue max;
        private readonly bool optional;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeConstraint&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        public RangeConstraint(TValue min, TValue max) : this(min, max, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeConstraint&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <param name="optional">if set to <c>true</c> [optional].</param>
        public RangeConstraint(TValue min, TValue max, bool optional)
        {
            this.min = min;
            this.max = max;
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

            if (values.ContainsKey(parameterName))
            {
                if (values[parameterName] == null)
                {
                    return optional;
                }

                TValue value;

                try
                {
                    value = (TValue)Convert.ChangeType(values[parameterName], typeof(TValue), Culture.Current);
                }
                catch
                {
                    return false;
                }

                bool matched = (min.CompareTo(value) <= 0) && (max.CompareTo(value) >= 0);

                return matched;
            }

            return optional;
        }
    }
}