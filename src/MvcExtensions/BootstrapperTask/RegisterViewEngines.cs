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
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class which is used to register available <seealso cref="IViewEngine"/>.
    /// </summary>
    public class RegisterViewEngines : IgnorableTypesBootstrapperTask<RegisterViewEngines, IViewEngine>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterViewEngines"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="viewEngines">The view engines.</param>
        public RegisterViewEngines(ContainerAdapter container, ViewEngineCollection viewEngines)
        {
            Invariant.IsNotNull(container, "container");
            Invariant.IsNotNull(viewEngines, "viewEngines");

            Container = container;
            ViewEngines = viewEngines;
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
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            IEnumerable<Type> viewEngineTypes = ViewEngines.Select(ve => ve.GetType()).ToList();

            Func<Type, bool> filter = type => KnownTypes.ViewEngineType.IsAssignableFrom(type) &&
                                              type.Assembly != KnownAssembly.AspNetMvcAssembly &&
                                              !IgnoredTypes.Any(ignoredType => ignoredType == type) &&
                                              !viewEngineTypes.Any(engineType => engineType == type);

            Container.GetService<IBuildManager>()
                     .ConcreteTypes
                     .Where(filter)
                     .Each(type => Container.RegisterAsSingleton(KnownTypes.ViewEngineType, type));

            Container.GetServices<IViewEngine>()
                     .Each(engine =>
                            {
                                if (engine != null)
                                {
                                    ViewEngines.Insert(0, engine);
                                }
                            });

            return TaskContinuation.Continue;
        }
    }
}