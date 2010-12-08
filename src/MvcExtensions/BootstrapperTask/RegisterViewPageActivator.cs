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
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class which is used to register the default <seealso cref="IViewPageActivator"/>.
    /// </summary>
    public class RegisterViewPageActivator : BootstrapperTask
    {
        private Type viewPageActivatorType = typeof(ExtendedViewPageActivator);

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterControllerActivator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RegisterViewPageActivator(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");

            Container = container;
        }

        /// <summary>
        /// Gets or sets the type of the controller activator.
        /// </summary>
        /// <value>The type of the controller activator.</value>
        public Type ViewPageActivatorType
        {
            [DebuggerStepThrough]
            get
            {
                return viewPageActivatorType;
            }

            [DebuggerStepThrough]
            set
            {
                viewPageActivatorType = value;
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
            Func<Type, bool> filter = type => KnownTypes.ViewPageActivatorType.IsAssignableFrom(type) &&
                                              type != ViewPageActivatorType &&
                                              type.Assembly != KnownAssembly.AspNetMvcAssembly &&
                                              !type.Assembly.GetName().Name.Equals(KnownAssembly.AspNetMvcFutureAssemblyName, StringComparison.OrdinalIgnoreCase);

            Container.GetService<IBuildManager>()
                     .ConcreteTypes
                     .Where(filter).Each(type => Container.RegisterAsSingleton(type));

            Container.RegisterAsSingleton(KnownTypes.ViewPageActivatorType, ViewPageActivatorType);

            return TaskContinuation.Continue;
        }
    }
}