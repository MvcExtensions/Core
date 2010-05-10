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
    using System.Web.Mvc;
    using System.Web.Routing;

    using Microsoft.Practices.ServiceLocation;
    using OriginalLocator = Microsoft.Practices.ServiceLocation.ServiceLocator;

    /// <summary>
    /// Defines a base class which is used to execute application startup and cleanup tasks.
    /// </summary>
    public abstract class Bootstrapper : Disposable, IBootstrapper
    {
        private readonly object syncLock = new object();
        private IServiceLocator serviceLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        /// <param name="buildManager">The build manager.</param>
        protected Bootstrapper(IBuildManager buildManager)
        {
            Invariant.IsNotNull(buildManager, "buildManager");

            BuildManager = buildManager;
        }

        /// <summary>
        /// Gets the service locator.
        /// </summary>
        /// <value>The service locator.</value>
        public IServiceLocator ServiceLocator
        {
            [DebuggerStepThrough]
            get
            {
                if (serviceLocator == null)
                {
                    lock (syncLock)
                    {
                        if (serviceLocator == null)
                        {
                            serviceLocator = CreateAndSetCurrent();
                            LoadModules();
                        }
                    }
                }

                return serviceLocator;
            }
        }

        /// <summary>
        /// Gets  the build manager.
        /// </summary>
        /// <value>The build manager.</value>
        protected IBuildManager BuildManager
        {
            get;
            private set;
        }

        /// <summary>
        /// Executes the <seealso cref="IBootstrapperTask"/>.
        /// </summary>
        public void Execute()
        {
            bool shouldSkip = false;

            foreach (IBootstrapperTask task in ServiceLocator.GetAllInstances<IBootstrapperTask>().OrderBy(task => task.Order).ToList())
            {
                if (shouldSkip)
                {
                    shouldSkip = false;
                    continue;
                }

                TaskContinuation continuation = task.Execute(ServiceLocator);

                if (continuation == TaskContinuation.Break)
                {
                    break;
                }

                shouldSkip = continuation == TaskContinuation.Skip;
            }
        }

        /// <summary>
        /// Creates the service locator.
        /// </summary>
        /// <returns></returns>
        protected abstract IServiceLocator CreateServiceLocator();

        /// <summary>
        /// Loads the container specific modules.
        /// </summary>
        protected abstract void LoadModules();

        /// <summary>
        /// Disposes the resources.
        /// </summary>
        protected override void DisposeCore()
        {
            if (serviceLocator != null)
            {
                serviceLocator.GetAllInstances<IBootstrapperTask>()
                              .OrderByDescending(task => task.Order)
                              .Each(task => task.Dispose());

                IDisposable disposable = serviceLocator as IDisposable;

                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        private static void Register(IServiceRegistrar serviceRegistrar, IServiceLocator serviceLocator, IBuildManager buildManager)
        {
            serviceRegistrar.RegisterInstance<RouteCollection>(RouteTable.Routes)
                            .RegisterInstance<ControllerBuilder>(ControllerBuilder.Current)
                            .RegisterInstance<ModelBinderDictionary>(ModelBinders.Binders)
                            .RegisterInstance<ViewEngineCollection>(ViewEngines.Engines)
                            .RegisterInstance<ValueProviderFactoryCollection>(ValueProviderFactories.Factories)
                            .RegisterInstance<IBuildManager>(buildManager)
                            .RegisterInstance<IServiceRegistrar>(serviceRegistrar)
                            .RegisterInstance<IServiceLocator>(serviceLocator)
                            .RegisterAsSingleton<IFilterRegistry, FilterRegistry>();

            if (serviceLocator is IServiceInjector)
            {
                serviceRegistrar.RegisterInstance<IServiceInjector>(serviceLocator);
            }

            buildManager.ConcreteTypes
                        .Where(type => KnownTypes.BootstrapperTaskType.IsAssignableFrom(type))
                        .Each(type => serviceRegistrar.RegisterAsSingleton(KnownTypes.BootstrapperTaskType, type));
        }

        private IServiceLocator CreateAndSetCurrent()
        {
            IServiceLocator locator = CreateServiceLocator();
            IServiceRegistrar serviceRegistrar = locator as IServiceRegistrar;

            if (serviceRegistrar != null)
            {
                Register(serviceRegistrar, locator, BuildManager);
            }

            OriginalLocator.SetLocatorProvider(() => locator);

            return locator;
        }
    }
}