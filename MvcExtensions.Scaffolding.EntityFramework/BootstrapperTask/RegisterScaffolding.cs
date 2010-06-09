#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    /// <summary>
    /// Defines a class which is used to register the required services for scaffolding.
    /// </summary>
    public class RegisterScaffolding : BootstrapperTask
    {
        private readonly ContainerAdapter container;

        static RegisterScaffolding()
        {
            RegisterControllerFactory.ControllerFactoryType = typeof(ScaffoldedControllerFactory);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterScaffolding"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RegisterScaffolding(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");

            this.container = container;

            Order = DefaultOrder - 1;
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            container.RegisterAsSingleton<IEntityFrameworkMetadataProvider, EntityFrameworkMetadataProvider>()
                     .RegisterAsSingleton<IViewModelTypeFactory, ViewModelTypeFactory>()
                     .RegisterAsSingleton<IViewModelTypeRegistry, ViewModelTypeRegistry>()
                     .RegisterAsSingleton<IControllerTypeRegistry, ControllerTypeRegistry>()
                     .RegisterAsPerRequest<IUnitOfWork, UnitOfWork>()
                     .RegisterAsPerRequest(typeof(IRepository<,>), typeof(Repository<,>))
                     .RegisterAsTransient(typeof(ScaffoldedController<,,>));

            return TaskContinuation.Continue;
        }
    }
}