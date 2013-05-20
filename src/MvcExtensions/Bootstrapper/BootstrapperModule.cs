#region Copyright
// Copyright (c) 2009 - 2013, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2013 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web;

    /// <summary>
    /// The bootstrapper module
    /// </summary>
    public class BootstrapperModule : IHttpModule
    {
        private static readonly object lockSync = new object();
        private static int initializeModuleCount;

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
        public void Init(HttpApplication context)
        {
            lock (lockSync)
            {
                if (initializeModuleCount++ == 0)
                {
                    Bootstrapper.Current.ExecuteBootstrapperTasks();
                }
            }

            context.BeginRequest += (sender, args) => Bootstrapper.Current.ExecutePerRequestTasks();
            context.EndRequest += (sender, args) => Bootstrapper.Current.DisposePerRequestTasks();
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
            lock (lockSync)
            {
                if (--initializeModuleCount == 0)
                {
                    Bootstrapper.Current.DisposeBootstrapperTasks();
                }
            }
        }
    }
}
