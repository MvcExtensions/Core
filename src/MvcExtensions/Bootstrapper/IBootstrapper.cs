#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;

    /// <summary>
    /// Represents an interface which is used to execute <seealso cref="BootstrapperTask"/>.
    /// </summary>
    public interface IBootstrapper : IDisposable
    {
        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        ContainerAdapter Adapter
        {
            get;
        }

        /// <summary>
        /// Executes the <seealso cref="BootstrapperTask"/>.
        /// </summary>
        void Execute();
    }
}