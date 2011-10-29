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

    /// <summary>
    /// Defines an static class which contains extension methods of <see cref="IServiceRegistrar"/>.
    /// </summary>
    public static class ServiceRegistrarExtensions
    {
        /// <summary>
        /// Registers the instance as a singleton service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegistrar RegisterInstance<TService>(this IServiceRegistrar instance, object service)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.RegisterInstance(typeof(TService), service);
        }

        /// <summary>
        /// Registers the instance as a singleton service of the instance type.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegistrar RegisterInstance(this IServiceRegistrar instance, object service)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.RegisterInstance(service.GetType(), service);
        }

        /// <summary>
        /// Registers the service as per request.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegistrar RegisterAsPerRequest<TImplementation>(this IServiceRegistrar instance) where TImplementation : class
        {
            return RegisterAsPerRequest<TImplementation, TImplementation>(instance);
        }

        /// <summary>
        /// Registers as per request.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegistrar RegisterAsPerRequest<TService, TImplementation>(this IServiceRegistrar instance) where TImplementation : TService where TService : class
        {
            return RegisterType<TService, TImplementation>(instance, LifetimeType.PerRequest);
        }

        /// <summary>
        /// Registers the service as per request.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegistrar RegisterAsPerRequest(this IServiceRegistrar instance, Type implementationType)
        {
            return RegisterAsPerRequest(instance, implementationType, implementationType);
        }

        /// <summary>
        /// Registers the service as per request.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegistrar RegisterAsPerRequest(this IServiceRegistrar instance, Type serviceType, Type implementationType)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.RegisterType(serviceType, implementationType, LifetimeType.PerRequest);
        }

        /// <summary>
        /// Registers the service as singleton.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegistrar RegisterAsSingleton<TImplementation>(this IServiceRegistrar instance) where TImplementation : class
        {
            return RegisterAsSingleton<TImplementation, TImplementation>(instance);
        }

        /// <summary>
        /// Registers as singleton.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegistrar RegisterAsSingleton<TService, TImplementation>(this IServiceRegistrar instance) where TImplementation : TService where TService : class
        {
            return RegisterType<TService, TImplementation>(instance, LifetimeType.Singleton);
        }

        /// <summary>
        /// Registers the service as singleton.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegistrar RegisterAsSingleton(this IServiceRegistrar instance, Type implementationType)
        {
            return RegisterAsSingleton(instance, implementationType, implementationType);
        }

        /// <summary>
        /// Registers the service as singleton.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegistrar RegisterAsSingleton(this IServiceRegistrar instance, Type serviceType, Type implementationType)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.RegisterType(serviceType, implementationType, LifetimeType.Singleton);
        }

        /// <summary>
        /// Registers the service as transient.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegistrar RegisterAsTransient<TImplementation>(this IServiceRegistrar instance) where TImplementation : class
        {
            return RegisterAsTransient<TImplementation, TImplementation>(instance);
        }

        /// <summary>
        /// Registers the service as transient.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegistrar RegisterAsTransient<TService, TImplementation>(this IServiceRegistrar instance) where TImplementation : TService where TService : class
        {
            return RegisterType<TService, TImplementation>(instance, LifetimeType.Transient);
        }

        /// <summary>
        /// Registers the service as transient.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegistrar RegisterAsTransient(this IServiceRegistrar instance, Type implementationType)
        {
            return RegisterAsTransient(instance, implementationType, implementationType);
        }

        /// <summary>
        /// Registers the service as transient.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegistrar RegisterAsTransient(this IServiceRegistrar instance, Type serviceType, Type implementationType)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.RegisterType(serviceType, implementationType, LifetimeType.Transient);
        }

        private static IServiceRegistrar RegisterType<TService, TImplementation>(this IServiceRegistrar instance, LifetimeType lifetime) where TImplementation : TService where TService : class
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.RegisterType(typeof(TService), typeof(TImplementation), lifetime);
        }
    }
}