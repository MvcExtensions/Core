#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web;

    /// <summary>
    /// Defines a base class which is executed for each request. This is similar to <seealso cref="IHttpModule"/> with only begin and end support.
    /// </summary>
    public abstract class PerRequestTask : OrderableTask, IPerRequestTask
    {
        /// <summary>
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <param name="executionContext">The execution context.</param>
        public TaskContinuation Execute(PerRequestExecutionContext executionContext)
        {
            Invariant.IsNotNull(executionContext, "executionContext");

            return ExecuteCore(executionContext);
        }

        /// <summary>
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <param name="executionContext">The execution context.</param>
        protected abstract TaskContinuation ExecuteCore(PerRequestExecutionContext executionContext);
    }
}