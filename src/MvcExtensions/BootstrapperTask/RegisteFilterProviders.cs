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
    /// Defines a class which is used to register available <seealso cref="IFilterProvider"/>.
    /// </summary>
    public class RegisteFilterProviders: BootstrapperTask
    {
        private static readonly IList<Type> ignoredTypes = new List<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteFilterProviders"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RegisteFilterProviders(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");

            Container = container;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RegisteFilterProviders"/> should be excluded.
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
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            if (!Excluded)
            {
                Func<Type, bool> filter = type => KnownTypes.FilterProviderType.IsAssignableFrom(type) &&
                                                  type.Assembly != KnownAssembly.AspNetMvcAssembly &&
                                                  !type.Assembly.GetName().Name.Equals(KnownAssembly.AspNetMvcFutureAssemblyName, StringComparison.OrdinalIgnoreCase) &&
                                                  !IgnoredTypes.Any(ignoredType => ignoredType == type);

                Container.GetInstance<IBuildManager>()
                         .ConcreteTypes
                         .Where(filter)
                         .Each(type => Container.RegisterAsTransient(KnownTypes.FilterProviderType, type));

                Container.GetAllInstances<IFilterProvider>()
                         .Each(provider => FilterProviders.Providers.Add(provider));
            }

            return TaskContinuation.Continue;
        }
    }
}