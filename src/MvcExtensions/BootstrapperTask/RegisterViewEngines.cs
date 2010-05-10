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
    /// Defines a class which is used to register available <seealso cref="IViewEngine"/>.
    /// </summary>
    public class RegisterViewEngines : BootstrapperTask
    {
        private static readonly IList<Type> ignoredTypes = new List<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterViewEngines"/> class.
        /// </summary>
        /// <param name="viewEngines">The view engines.</param>
        public RegisterViewEngines(ViewEngineCollection viewEngines)
        {
            Invariant.IsNotNull(viewEngines, "viewEngines");

            ViewEngines = viewEngines;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RegisterViewEngines"/> should be excluded.
        /// </summary>
        /// <value><c>true</c> if excluded; otherwise, <c>false</c>.</value>
        public static bool Excluded
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the ignored model binder types.
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
        /// Gets the view engines.
        /// </summary>
        /// <value>The view engines.</value>
        protected ViewEngineCollection ViewEngines
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
                    IEnumerable<Type> viewEngineTypes = ViewEngines.Select(ve => ve.GetType()).ToList();

                    Func<Type, bool> filter = type => KnownTypes.ViewEngineType.IsAssignableFrom(type) &&
                                                      type.Assembly != KnownAssembly.AspNetMvcAssembly &&
                                                      !IgnoredTypes.Any(ignoredType => ignoredType == type) &&
                                                      !viewEngineTypes.Any(engineType => engineType == type);

                    serviceLocator.GetInstance<IBuildManager>()
                                  .ConcreteTypes
                                  .Where(filter)
                                  .Each(type => serviceRegistrar.RegisterAsSingleton(KnownTypes.ViewEngineType, type));

                    serviceLocator.GetAllInstances<IViewEngine>()
                                  .Each(engine =>
                                            {
                                                if (engine != null)
                                                {
                                                    ViewEngines.Add(engine);
                                                }
                                            });
                }
            }

            return TaskContinuation.Continue;
        }
    }
}