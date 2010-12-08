namespace Demo.Web.Ninject
{
    using MvcExtensions;
    using MvcExtensions.Ninject;

    public class MvcApplication : NinjectMvcApplication
    {
        public MvcApplication()
        {
            Bootstrapper.BootstrapperTasks
                        .Include<RegisterModelMetadata>()
                        .Include<RegisterControllers>()
                        .Include<ConfigureFilterAttributes>()
                        .Include<ConfigureModelBinders>()
                        .Include<RegisterRoutes>();
        }
    }
}