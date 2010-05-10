#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web.Mvc;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Defines a class to configure <seealso cref="FilterAttribute"/> for <see cref="Controller"/> or action methods.
    /// </summary>
    public abstract class ConfigureFiltersBase : BootstrapperTask
    {
        /// <summary>
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <returns></returns>
        protected override TaskContinuation ExecuteCore(IServiceLocator serviceLocator)
        {
            Configure(serviceLocator.GetInstance<IFilterRegistry>());

            return TaskContinuation.Continue;
        }

        /// <summary>
        /// Registers filters in the specified registry.
        /// </summary>
        /// <param name="registry">The registry.</param>
        protected abstract void Configure(IFilterRegistry registry);
    }
}