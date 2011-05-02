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

    /// <summary>
    /// Defines a static class which contains extension method of <see cref="ResponderCollection"/>.
    /// </summary>
    public static class ResponderCollectionExtensions
    {
        /// <summary>
        /// Gets the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static IResponder Get(this ResponderCollection instance, string format)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.FirstOrDefault(r => r.CanRespondToFormat(format));
        }

        /// <summary>
        /// Gets the specified instance.
        /// </summary>
        /// <typeparam name="TResponder">The type of the responder.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static TResponder Get<TResponder>(this ResponderCollection instance) where TResponder : IResponder
        {
            Invariant.IsNotNull(instance, "instance");

            Type responderType = typeof(TResponder);

            return instance.Select(x => new { r = x, t = x.GetType() })
                           .Where(p => p.t == responderType)
                           .Select(p => p.r)
                           .Cast<TResponder>()
                           .FirstOrDefault();
        }

        /// <summary>
        /// Includes the specified instance.
        /// </summary>
        /// <typeparam name="TResponder">The type of the responder.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static ResponderCollection Include<TResponder>(this ResponderCollection instance) where TResponder : IResponder, new()
        {
            return Include<TResponder>(instance, null);
        }

        /// <summary>
        /// Includes the specified instance.
        /// </summary>
        /// <typeparam name="TResponder">The type of the responder.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="configure">The configure.</param>
        /// <returns></returns>
        public static ResponderCollection Include<TResponder>(this ResponderCollection instance, Action<TResponder> configure) where TResponder : IResponder, new()
        {
            Invariant.IsNotNull(instance, "instance");

            TResponder responder = new TResponder();

            instance.Add(responder);

            if (configure != null)
            {
                configure(responder);
            }

            return instance;
        }
    }
}