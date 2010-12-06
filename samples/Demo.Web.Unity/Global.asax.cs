namespace Demo.Web.Unity
{
    using MvcExtensions;
    using MvcExtensions.Unity;

    public class MvcApplication : UnityMvcApplication
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