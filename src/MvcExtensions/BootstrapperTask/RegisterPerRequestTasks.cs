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

    /// <summary>
    /// Defines a class which is used to register available <seealso cref="PerRequestTask"/>.
    /// </summary>
    public class RegisterPerRequestTasks : BootstrapperTask
    {
        private static readonly ICollection<Type> ignoredTypes = new List<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterPerRequestTasks"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RegisterPerRequestTasks(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");

            Container = container;
        }

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
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        protected ContainerAdapter Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            if (!Excluded)
            {
                Func<Type, bool> filter = type => KnownTypes.PerRequestTaskType.IsAssignableFrom(type) &&
                                                  !IgnoredTypes.Any(ignoredType => ignoredType == type);

                Container.GetInstance<IBuildManager>()
                         .ConcreteTypes
                         .Where(filter)
                         .Each(type => Container.RegisterAsPerRequest(KnownTypes.PerRequestTaskType, type));
            }

            return TaskContinuation.Continue;
        }
    }
}