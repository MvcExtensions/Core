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
    public abstract class Task : Disposable
    {
        /// <summary>
        /// Executes the task.
        /// </summary><returns></returns>
        public abstract TaskContinuation Execute();
    }
}