#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Represents an interface which is executed when <see cref="ExtendedMvcApplication"/> starts and ends.
    /// </summary>
    public interface IBootstrapperTask : IOrderableTask, IDisposable
    {
        /// <summary>
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <returns></returns>
        TaskContinuation Execute(IServiceLocator serviceLocator);
    }
}