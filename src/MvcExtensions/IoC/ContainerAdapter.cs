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

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Defines a base class which acts as an adapter for IoC Container.
    /// </summary>
    public abstract class ContainerAdapter : ServiceLocatorImplBase, IServiceRegistrar, IServiceInjector, IDisposable
    {
        private bool disposed;

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ContainerAdapter"/> is reclaimed by garbage collection.
        /// </summary>
        [DebuggerStepThrough]
        ~ContainerAdapter()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Registers the service and its implementation with the lifetime behavior.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns></returns>
        public abstract IServiceRegistrar RegisterType(string key, Type serviceType, Type implementationType, LifetimeType lifetime);

        /// <summary>
        /// Registers the instance as singleton.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public abstract IServiceRegistrar RegisterInstance(string key, Type serviceType, object instance);

        /// <summary>
        /// Injects the matching dependences.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public abstract void Inject(object instance);

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        protected virtual void DisposeCore()
        {
        }

        [DebuggerStepThrough]
        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                disposed = true;
                DisposeCore();
            }

            disposed = true;
        }
    }
}