#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Defines a class which provides execution information of <seealso cref="IPerRequestTask"/>.
    /// </summary>
    public class PerRequestExecutionContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PerRequestExecutionContext"/> class.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="serviceLocator">The service locator.</param>
        public PerRequestExecutionContext(HttpContextBase httpContext, IServiceLocator serviceLocator)
        {
            Invariant.IsNotNull(httpContext, "httpContext");
            Invariant.IsNotNull(serviceLocator, "serviceLocator");

            HttpContext = httpContext;
            ServiceLocator = serviceLocator;
        }

        /// <summary>
        /// Gets or sets the HTTP context.
        /// </summary>
        /// <value>The HTTP context.</value>
        public HttpContextBase HttpContext
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the service locator.
        /// </summary>
        /// <value>The service locator.</value>
        public IServiceLocator ServiceLocator
        {
            get;
            private set;
        }
    }
}