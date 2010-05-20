#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    /// <summary>
    /// Represents an interface which supports execution order.
    /// </summary>
    public interface IOrderableTask
    {
        /// <summary>
        /// Gets the order that the task would execute.
        /// </summary>
        /// <value>The order.</value>
        int Order
        {
            get;
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        TaskContinuation Execute();
    }
}