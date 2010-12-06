namespace Demo.Web.Windsor
{
    using MvcExtensions;
    using MvcExtensions.Windsor;

    public class MvcApplication : WindsorMvcApplication
    {
        public MvcApplication()
        {
            Bootstrapper.BootstrapperTasks
                        .Include<RegisterActionInvokers>()
                        .Include<RegisterControllerActivator>()
                        .Include<RegisterControllers>()
                        .Include<RegisterFilterAttributes>()
                        .Include<RegisterModelBinders>()
                        .Include<RegisterModelMetadata>()
                        .Include<ConfigureFilterAttributes>()
                        .Include<RegisterRoutes>();
        }
    }
}