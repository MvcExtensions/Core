namespace Demo.Web.Autofac
{
    using MvcExtensions;
    using MvcExtensions.Autofac;

    public class MvcApplication : AutofacMvcApplication
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