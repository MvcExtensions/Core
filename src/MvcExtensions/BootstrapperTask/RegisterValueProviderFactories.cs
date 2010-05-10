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
    /// Defines a class which is used to register available <seealso cref="ValueProviderFactory"/>.
    /// </summary>
    public class RegisterValueProviderFactories : BootstrapperTask
    {
        private static readonly IList<Type> ignoredTypes = new List<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterValueProviderFactories"/> class.
        /// </summary>
        /// <param name="factories">The factories.</param>
        public RegisterValueProviderFactories(ValueProviderFactoryCollection factories)
        {
            Invariant.IsNotNull(factories, "factories");

            Factories = factories;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RegisterValueProviderFactories"/> should be excluded.
        /// </summary>
        /// <value><c>true</c> if excluded; otherwise, <c>false</c>.</value>
        public static bool Excluded
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the ignored value provider factory types.
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
        /// Gets or sets the factories.
        /// </summary>
        /// <value>The factories.</value>
        protected ValueProviderFactoryCollection Factories
        { 
            get;
            private set;
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
                    Func<Type, bool> filter = type => KnownTypes.ValueProviderFactoryType.IsAssignableFrom(type) &&
                                                      type.Assembly != KnownAssembly.AspNetMvcAssembly &&
                                                      !IgnoredTypes.Any(ignoredType => ignoredType == type) &&
                                                      !Factories.Any(factory => factory.GetType() == type);

                    serviceLocator.GetInstance<IBuildManager>()
                                  .ConcreteTypes
                                  .Where(filter)
                                  .Each(type => serviceRegistrar.RegisterAsSingleton(KnownTypes.ValueProviderFactoryType, type));

                    serviceLocator.GetAllInstances<ValueProviderFactory>()
                                  .Each(factory => Factories.Add(factory));
                }
            }

            return TaskContinuation.Continue;
        }
    }
}