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
    using System.Linq;
    using System.Web;

    using Microsoft.Practices.ServiceLocation;

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
            Bootstrapper.Execute();
            OnStart();
        }

        /// <summary>
        /// Fires when the application ends.
        /// </summary>
        public void Application_End()
        {
            OnEnd();
            Bootstrapper.Dispose();
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
        /// Called when [begin request].
        /// </summary>
        protected virtual void OnBeginRequest(HttpContextBase context)
        {
            OnPerRequestTasksExecuting();

            IServiceLocator serviceLocator = Bootstrapper.ServiceLocator;

            PerRequestExecutionContext perRequestExecutionContext = new PerRequestExecutionContext(context, serviceLocator);

            bool shouldSkip = false;

            foreach (IPerRequestTask task in serviceLocator.GetAllInstances<IPerRequestTask>().OrderBy(task => task.Order))
            {
                if (shouldSkip)
                {
                    shouldSkip = false;
                    continue;
                }

                TaskContinuation continuation = task.Execute(perRequestExecutionContext);

                if (continuation == TaskContinuation.Break)
                {
                    break;
                }

                shouldSkip = continuation == TaskContinuation.Skip;
            }

            OnPerRequestTasksExecuted();
        }

        /// <summary>
        /// Executes before the registered <see cref="IPerRequestTask"/> executes.
        /// </summary>
        protected virtual void OnPerRequestTasksExecuting()
        {
        }

        /// <summary>
        /// Executes after the registered <see cref="IPerRequestTask"/> executes.
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

            Bootstrapper.ServiceLocator
                        .GetAllInstances<IPerRequestTask>()
                        .OrderByDescending(task => task.Order)
                        .Each(task => task.Dispose());

            OnPerRequestTasksDisposed();
        }

        /// <summary>
        /// Executes before the registered <see cref="IPerRequestTask"/> disposes.
        /// </summary>
        protected virtual void OnPerRequestTasksDisposing()
        {
        }

        /// <summary>
        /// Executes after the registered <see cref="IPerRequestTask"/> disposes.
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