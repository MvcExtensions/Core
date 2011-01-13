namespace MvcExtensions
{
    using System;
    using System.Diagnostics;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class which is used to register the default <seealso cref="IModelBinderProvider"/>.
    /// </summary>
    public class RegisterModelBinderProvider : BootstrapperTask
    {
        private Type modelBinderProviderType = typeof(ExtendedModelBinderProvider);

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterModelBinderProvider"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RegisterModelBinderProvider(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");

            Container = container;
        }

        /// <summary>
        /// Gets or sets the type of the model binder provider.
        /// </summary>
        /// <value>The type of the model binder provider.</value>
        public Type ModelBinderProviderType
        {
            [DebuggerStepThrough]
            get
            {
                return modelBinderProviderType;
            }

            [DebuggerStepThrough]
            set
            {
                modelBinderProviderType = value;
            }
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        protected ContainerAdapter Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Executes the task.
        /// </summary><returns></returns>
        public override TaskContinuation Execute()
        {
            Container.RegisterAsSingleton(typeof(IModelBinderProvider), ModelBinderProviderType);

            return TaskContinuation.Continue;
        }
    }
}