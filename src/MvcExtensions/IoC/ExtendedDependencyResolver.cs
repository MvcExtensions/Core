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
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// This class is a helper that provides a default implementation
    /// for most of the methods of <see cref="IDependencyResolver"/>.
    /// </summary>
    public abstract class ExtendedDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// Implementation of <see cref="IServiceProvider.GetService"/>.
        /// </summary>
        /// <param name="serviceType">The requested service.</param>
        /// <returns>The requested object.</returns>
        public virtual object GetService(Type serviceType)
        {
            try
            {
                return DoGetService(serviceType, null);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                
                // Eat the exception, the ASP.NET MVC Framework expects a null service when the underlying container
                // cannot resolve.
                return null;
            }
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        public virtual IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return DoGetServices(serviceType);
            }
            catch (Exception)
            {
                // Eat the exception, the ASP.NET MVC Framework expects an empty enumerable when the underlying container
                // cannot resolve.
                return Enumerable.Empty<object>();
            }
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected abstract object DoGetService(Type serviceType, string key);

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        protected abstract IEnumerable<object> DoGetServices(Type serviceType);
    }
}