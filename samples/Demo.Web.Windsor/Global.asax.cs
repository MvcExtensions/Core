namespace Demo.Web.Windsor
{
    using MvcExtensions;
    using MvcExtensions.Windsor;

    public class MvcApplication : WindsorMvcApplication
    {
        public MvcApplication()
        {
            Bootstrapper.BootstrapperTasks
                        .Include<RegisterModelBinders>()
                        .Include<RegisterModelMetadata>()
                        .Include<ConfigureFilterAttributes>()
                        .Include<RegisterControllers>()
                        .Include<RegisterRoutes>();
        }
    }
}