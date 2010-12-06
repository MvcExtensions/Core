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
        /// Gets the build manager.
        /// </summary>
        /// <value>The build manager.</value>
        IBuildManager BuildManager
        {
            get;
        }

        /// <summary>
        /// Gets the bootstrapper tasks.
        /// </summary>
        /// <value>The bootstrapper tasks.</value>
        IBootstrapperTasksRegistry BootstrapperTasks
        {
            get;
        }

        /// <summary>
        /// Gets the per request task registry.
        /// </summary>
        /// <value>The per request tasks.</value>
        IPerRequestTasksRegistry PerRequestTasks
        {
            get;
        }

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
        void ExecuteBootstrapperTasks();

        /// <summary>
        /// Dispose the <seealso cref="BootstrapperTask"/>.
        /// </summary>
        void DisposeBootstrapperTasks();

        /// <summary>
        /// Executes the <seealso cref="PerRequestTask"/>.
        /// </summary>
        void ExecutePerRequestTasks();

        /// <summary>
        /// Dispose the <seealso cref="PerRequestTask"/>.
        /// </summary>
        void DisposePerRequestTasks();
    }
}