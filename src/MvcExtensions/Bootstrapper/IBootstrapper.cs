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
    /// Represents an interface which is used to execute <seealso cref="IBootstrapperTask"/>.
    /// </summary>
    public interface IBootstrapper : IDisposable
    {
        /// <summary>
        /// Gets the service locator.
        /// </summary>
        /// <value>The service locator.</value>
        IServiceLocator ServiceLocator
        {
            get;
        }

        /// <summary>
        /// Executes the <seealso cref="IBootstrapperTask"/>.
        /// </summary>
        void Execute();
    }
}