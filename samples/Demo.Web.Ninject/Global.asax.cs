namespace Demo.Web.Ninject
{
    using MvcExtensions;
    using MvcExtensions.Ninject;

    public class MvcApplication : NinjectMvcApplication
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