namespace Demo.Web.Unity
{
    using MvcExtensions;
    using MvcExtensions.Unity;

    public class MvcApplication : UnityMvcApplication
    {
        public MvcApplication()
        {
            Bootstrapper.BootstrapperTasks
                        .Include<RegisterModelMetadata>()
                        .Include<RegisterControllers>()
                        .Include<ConfigureFilters>()
                        .Include<ConfigureModelBinders>()
                        .Include<RegisterRoutes>()
                        .Include<RegisterActionInvokers>();
        }
    }
}