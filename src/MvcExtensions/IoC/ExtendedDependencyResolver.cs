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
            return DoGetService(serviceType);
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        public virtual IEnumerable<object> GetServices(Type serviceType)
        {
            return DoGetServices(serviceType);
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        protected abstract object DoGetService(Type serviceType);

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        protected abstract IEnumerable<object> DoGetServices(Type serviceType);
    }
}