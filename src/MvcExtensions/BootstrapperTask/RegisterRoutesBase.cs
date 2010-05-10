#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web.Routing;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Defines a base class to configure <seealso cref="RouteTable"/>.
    /// </summary>
    public abstract class RegisterRoutesBase : BootstrapperTask
    {
        /// <summary>
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <returns></returns>
        protected override TaskContinuation ExecuteCore(IServiceLocator serviceLocator)
        {
            Register(serviceLocator.GetInstance<RouteCollection>());

            return TaskContinuation.Continue;
        }

        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        protected abstract void Register(RouteCollection routes);
    }
}