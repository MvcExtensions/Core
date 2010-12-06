#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Diagnostics;
    using System.Web;

    /// <summary>
    /// Defines a base class to manage application life cycle.
    /// </summary>
    public abstract class ExtendedMvcApplication : HttpApplication
    {
        private static readonly object syncLock = new object();
        private static IBootstrapper bootstrapper;

        /// <summary>
        /// Gets the bootstrapper.
        /// </summary>
        /// <value>The bootstrapper.</value>
        public IBootstrapper Bootstrapper
        {
            [DebuggerStepThrough]
            get
            {
                if (bootstrapper == null)
                {
                    lock (syncLock)
                    {
                        if (bootstrapper == null)
                        {
                            bootstrapper = CreateBootstrapper();
                        }
                    }
                }

                return bootstrapper;
            }
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public ContainerAdapter Adapter
        {
            [DebuggerStepThrough]
            get
            {
                return Bootstrapper.Adapter;
            }
        }

        /// <summary>
        /// Executes custom initialization code after all event handler modules have been added.
        /// </summary>
        public override void Init()
        {
            base.Init();

            BeginRequest += HandleBeginRequest;
            EndRequest += HandleEndRequest;
        }

        /// <summary>
        /// Fires when the application starts.
        /// </summary>
        public void Application_Start()
        {
            Bootstrapper.ExecuteBootstrapperTasks();
            OnStart();
        }

        /// <summary>
        /// Fires when the application ends.
        /// </summary>
        public void Application_End()
        {
            Bootstrapper.DisposeBootstrapperTasks();
            OnEnd();
        }

        /// <summary>
        /// Creates the bootstrapper.
        /// </summary>
        /// <returns></returns>
        protected abstract IBootstrapper CreateBootstrapper();

        /// <summary>
        /// Executes when the application starts.
        /// </summary>
        protected virtual void OnStart()
        {
        }

        /// <summary>
        /// Called when request starts.
        /// </summary>
        protected virtual void OnBeginRequest(HttpContextBase context)
        {
            OnPerRequestTasksExecuting();
            Bootstrapper.ExecutePerRequestTasks();
            OnPerRequestTasksExecuted();
        }

        /// <summary>
        /// Executes before the registered <see cref="PerRequestTask"/> executes.
        /// </summary>
        protected virtual void OnPerRequestTasksExecuting()
        {
        }

        /// <summary>
        /// Executes after the registered <see cref="PerRequestTask"/> executes.
        /// </summary>
        protected virtual void OnPerRequestTasksExecuted()
        {
        }

        /// <summary>
        /// Executes when the request ends.
        /// </summary>
        protected virtual void OnEndRequest(HttpContextBase context)
        {
            OnPerRequestTasksDisposing();
            Bootstrapper.DisposePerRequestTasks();
            OnPerRequestTasksDisposed();
        }

        /// <summary>
        /// Executes before the registered <see cref="PerRequestTask"/> disposes.
        /// </summary>
        protected virtual void OnPerRequestTasksDisposing()
        {
        }

        /// <summary>
        /// Executes after the registered <see cref="PerRequestTask"/> disposes.
        /// </summary>
        protected virtual void OnPerRequestTasksDisposed()
        {
        }

        /// <summary>
        /// Executes when the application ends.
        /// </summary>
        protected virtual void OnEnd()
        {
        }

        /// <summary>
        /// Gets the current adapter.
        /// </summary>
        /// <returns></returns>
        protected virtual ContainerAdapter GetCurrentAdapter()
        {
            return Adapter;
        }

        private void HandleBeginRequest(object sender, EventArgs e)
        {
            OnBeginRequest(new HttpContextWrapper(Context));
        }

        private void HandleEndRequest(object sender, EventArgs e)
        {
            OnEndRequest(new HttpContextWrapper(Context));
        }
    }
}