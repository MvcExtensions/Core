namespace Demo.Web
{
    using System.Web.Mvc;
    using MvcExtensions;

    public class ConfigureModelBinders : ConfigureModelBindersBase
    {
        public ConfigureModelBinders(TypeMappingRegistry<object, IModelBinder> registry) : base(registry)
        {
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        protected override void Configure()
        {
            Registry.Register<ProductEditModel, ProductEditModelBinder>();
        }
    }
}