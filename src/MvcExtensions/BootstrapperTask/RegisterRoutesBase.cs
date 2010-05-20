#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web.Routing;

    /// <summary>
    /// Defines a base class to configure <seealso cref="RouteTable"/>.
    /// </summary>
    public abstract class RegisterRoutesBase : BootstrapperTask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterRoutesBase"/> class.
        /// </summary>
        /// <param name="routes">The routes.</param>
        protected RegisterRoutesBase(RouteCollection routes)
        {
            Invariant.IsNotNull(routes, "routes");

            Routes = routes;
        }

        /// <summary>
        /// Gets the routes.
        /// </summary>
        /// <value>The routes.</value>
        protected RouteCollection Routes
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
            Register();

            return TaskContinuation.Continue;
        }

        /// <summary>
        /// Registers the routes.
        /// </summary>
        protected abstract void Register();
    }
}