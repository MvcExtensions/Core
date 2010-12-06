namespace Demo.Web.StructureMap
{
    using MvcExtensions;
    using MvcExtensions.StructureMap;

    public class MvcApplication : StructureMapMvcApplication
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