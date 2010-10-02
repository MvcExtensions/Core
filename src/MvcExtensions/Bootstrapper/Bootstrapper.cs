#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Defines a base class which is used to execute application startup and cleanup tasks.
    /// </summary>
    public abstract class Bootstrapper : Disposable, IBootstrapper
    {
        private readonly object syncLock = new object();
        private ContainerAdapter container;

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
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public ContainerAdapter Adapter
        {
            [DebuggerStepThrough]
            get
            {
                if (container == null)
                {
                    lock (syncLock)
                    {
                        if (container == null)
                        {
                            container = CreateAndSetCurrent();
                            LoadModules();
                        }
                    }
                }

                return container;
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
        /// Executes the <seealso cref="BootstrapperTask"/>.
        /// </summary>
        public void Execute()
        {
            bool shouldSkip = false;

            foreach (BootstrapperTask task in Adapter.GetServices<BootstrapperTask>().OrderBy(task => task.Order))
            {
                if (shouldSkip)
                {
                    shouldSkip = false;
                    continue;
                }

                TaskContinuation continuation = task.Execute();

                if (continuation == TaskContinuation.Break)
                {
                    break;
                }

                shouldSkip = continuation == TaskContinuation.Skip;
            }
        }

        /// <summary>
        /// Creates the container adapter.
        /// </summary>
        /// <returns></returns>
        protected abstract ContainerAdapter CreateAdapter();

        /// <summary>
        /// Loads the container specific modules.
        /// </summary>
        protected abstract void LoadModules();

        /// <summary>
        /// Disposes the resources.
        /// </summary>
        protected override void DisposeCore()
        {
            if (container == null)
            {
                return;
            }

            container.GetServices<BootstrapperTask>()
                     .OrderByDescending(task => task.Order)
                     .Each(task => task.Dispose());

            container.Dispose();
        }

        private static void Register(IServiceRegistrar adapter, IBuildManager buildManager)
        {
            adapter.RegisterInstance<RouteCollection>(RouteTable.Routes)
                   .RegisterInstance<ControllerBuilder>(ControllerBuilder.Current)
                   .RegisterInstance<ModelBinderDictionary>(ModelBinders.Binders)
                   .RegisterInstance<ViewEngineCollection>(ViewEngines.Engines)
                   .RegisterInstance<ValueProviderFactoryCollection>(ValueProviderFactories.Factories)
                   .RegisterAsSingleton<IActionInvokerRegistry, ActionInvokerRegistry>()
                   .RegisterAsSingleton<IFilterRegistry, FilterRegistry>()
                   .RegisterAsSingleton<IModelMetadataRegistry, ModelMetadataRegistry>()
                   .RegisterAsSingleton<ModelMetadataProvider, ExtendedModelMetadataProvider>()
                   .RegisterInstance<IBuildManager>(buildManager);

            buildManager.ConcreteTypes
                        .Where(type => KnownTypes.BootstrapperTaskType.IsAssignableFrom(type))
                        .Each(type => adapter.RegisterAsSingleton(KnownTypes.BootstrapperTaskType, type));

            adapter.RegisterInstance<IServiceRegistrar>(adapter)
                   .RegisterInstance<IDependencyResolver>(adapter)
                   .RegisterInstance<IServiceInjector>(adapter)
                   .RegisterInstance<ContainerAdapter>(adapter);
        }

        private ContainerAdapter CreateAndSetCurrent()
        {
            ContainerAdapter adapter = CreateAdapter();

            Register(adapter, BuildManager);

            DependencyResolver.SetResolver(adapter);

            return adapter;
        }
    }
}