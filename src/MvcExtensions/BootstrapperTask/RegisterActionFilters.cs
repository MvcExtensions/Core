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

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Defines a class which is used to register available <seealso cref="FilterAttribute"/>.
    /// </summary>
    public class RegisterActionFilters : BootstrapperTask
    {
        private static readonly IList<Type> ignoredTypes = new List<Type>();

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RegisterActionFilters"/> should be excluded.
        /// </summary>
        /// <value><c>true</c> if excluded; otherwise, <c>false</c>.</value>
        public static bool Excluded
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the ignored filter attribute types.
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
                    Func<Type, bool> filter = type => KnownTypes.FilterAttributeType.IsAssignableFrom(type) &&
                                                       type.Assembly != KnownAssembly.AspNetMvcAssembly &&
                                                       type.Assembly != KnownAssembly.AspNetMvcExtensibilityAssembly &&
                                                       !type.Assembly.GetName().Name.Equals(KnownAssembly.AspNetMvcFutureAssemblyName, StringComparison.OrdinalIgnoreCase) &&
                                                       !IgnoredTypes.Any(ignoredType => ignoredType == type);

                    serviceLocator.GetInstance<IBuildManager>()
                                  .ConcreteTypes
                                  .Where(filter)
                                  .Each(type => serviceRegistrar.RegisterAsTransient(type));
                }
            }

            return TaskContinuation.Continue;
        }
    }
}