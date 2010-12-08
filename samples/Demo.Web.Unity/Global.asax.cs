namespace Demo.Web.Unity
{
    using MvcExtensions;
    using MvcExtensions.Unity;

    public class MvcApplication : UnityMvcApplication
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