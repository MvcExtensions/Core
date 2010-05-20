#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class which is used to register the default <seealso cref="IControllerFactory"/>.
    /// </summary>
    public class RegisterControllerFactory : BootstrapperTask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterControllerFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="controllerBuilder">The controller builder.</param>
        public RegisterControllerFactory(ContainerAdapter container, ControllerBuilder controllerBuilder)
        {
            Invariant.IsNotNull(container, "container");
            Invariant.IsNotNull(controllerBuilder, "controllerBuilder");

            Container = container;
            ControllerBuilder = controllerBuilder;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RegisterControllerFactory"/> should be excluded.
        /// </summary>
        /// <value><c>true</c> if excluded; otherwise, <c>false</c>.</value>
        public static bool Excluded
        {
            get;
            set;
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
        /// Gets or sets the controller builder.
        /// </summary>
        /// <value>The controller builder.</value>
        protected ControllerBuilder ControllerBuilder
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
                Container.RegisterAsSingleton<IControllerFactory, ExtendedControllerFactory>();
                ControllerBuilder.SetControllerFactory(Container.GetInstance<IControllerFactory>());
            }

            return TaskContinuation.Continue;
        }
    }
}