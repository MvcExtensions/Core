namespace Demo.Web.Ninject
{
    using MvcExtensions;
    using MvcExtensions.Ninject;

    public class MvcApplication : NinjectMvcApplication
    {
        public MvcApplication()
        {
            Bootstrapper.BootstrapperTasks
                        .Include<RegisterModelBinders>()
                        .Include<RegisterModelMetadata>()
                        .Include<ConfigureFilterAttributes>()
                        .Include<RegisterRoutes>();
        }
    }
}