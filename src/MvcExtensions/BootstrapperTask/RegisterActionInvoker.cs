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
    /// Defines a class which is used to register the default <seealso cref="IActionInvoker"/>.
    /// </summary>
    public class RegisterActionInvoker : BootstrapperTask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterActionInvoker"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RegisterActionInvoker(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");

            Container = container;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RegisterActionInvoker"/> should be excluded.
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
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            if (!Excluded)
            {
                Container.RegisterAsTransient<IActionInvoker, ExtendedControllerActionInvoker>();
            }

            return TaskContinuation.Continue;
        }
    }
}