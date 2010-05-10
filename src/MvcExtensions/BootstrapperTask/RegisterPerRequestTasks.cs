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

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Defines a class which is used to register available <seealso cref="IPerRequestTask"/>.
    /// </summary>
    public class RegisterPerRequestTasks : BootstrapperTask
    {
        private static readonly IList<Type> ignoredTypes = new List<Type>();

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RegisterPerRequestTasks"/> should be excluded.
        /// </summary>
        /// <value><c>true</c> if excluded; otherwise, <c>false</c>.</value>
        public static bool Excluded
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the ignored controller types.
        /// </summary>
        /// <value>The ignored types.</value>
        public static ICollection<Type> IgnoredTypes
        {
            [DebuggerStepThrough]
            get
            {
                return ignoredTypes;
            }
        }

        /// <summary>
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <returns></returns>
        protected override TaskContinuation ExecuteCore(IServiceLocator serviceLocator)
        {
            if (!Excluded)
            {
                IServiceRegistrar serviceRegistrar = serviceLocator as IServiceRegistrar;

                if (serviceRegistrar != null)
                {
                    Func<Type, bool> filter = type => KnownTypes.PerRequestTaskType.IsAssignableFrom(type) &&
                                                      !IgnoredTypes.Any(ignoredType => ignoredType == type);

                    serviceLocator.GetInstance<IBuildManager>()
                                  .ConcreteTypes
                                  .Where(filter)
                                  .Each(type => serviceRegistrar.RegisterAsPerRequest(KnownTypes.PerRequestTaskType, type));
                }
            }

            return TaskContinuation.Continue;
        }
    }
}