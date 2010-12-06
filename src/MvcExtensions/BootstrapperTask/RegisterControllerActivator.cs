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
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class which is used to register the default <seealso cref="IControllerActivator"/>.
    /// </summary>
    public class RegisterControllerActivator : BootstrapperTask
    {
        private Type controllerActivatorType = typeof(ExtendedControllerActivator);

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterControllerActivator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RegisterControllerActivator(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");

            Container = container;
        }

        /// <summary>
        /// Gets or sets the type of the controller activator.
        /// </summary>
        /// <value>The type of the controller activator.</value>
        public Type ControllerActivatorType
        {
            [DebuggerStepThrough]
            get
            {
                return controllerActivatorType;
            }

            [DebuggerStepThrough]
            set
            {
                controllerActivatorType = value;
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
            Container.RegisterAsSingleton(typeof(IControllerActivator), ControllerActivatorType);

            return TaskContinuation.Continue;
        }
    }
}