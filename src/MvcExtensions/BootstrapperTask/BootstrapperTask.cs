#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Diagnostics;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Defines a base class which is executed when <see cref="ExtendedMvcApplication"/> starts and ends.
    /// </summary>
    public abstract class BootstrapperTask : OrderableTask, IBootstrapperTask
    {
        /// <summary>
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public TaskContinuation Execute(IServiceLocator serviceLocator)
        {
            Invariant.IsNotNull(serviceLocator, "serviceLocator");

            return ExecuteCore(serviceLocator);
        }

        /// <summary>
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        protected abstract TaskContinuation ExecuteCore(IServiceLocator serviceLocator);
    }
}