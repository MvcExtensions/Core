#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    /// <summary>
    /// Defines an static class which contains extension methods of <see cref="ExtendedDependencyResolver"/>.
    /// </summary>
    public static class ExtendedDependencyResolverExtensions
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static TService GetService<TService>(this ExtendedDependencyResolver instance, string key)
        {
            Invariant.IsNotNull(instance, "instance");

            return (TService)instance.GetService(typeof(TService), key);
        }
    }
}