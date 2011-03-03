namespace Demo.Web.StructureMap
{
    using MvcExtensions;
    using MvcExtensions.StructureMap;

    public class MvcApplication : StructureMapMvcApplication
    {
        public MvcApplication()
        {
            Bootstrapper.BootstrapperTasks
                        .Include<RegisterModelMetadata>()
                        .Include<RegisterControllers>()
                        .Include<ConfigureFilters>()
                        .Include<ConfigureModelBinders>()
                        .Include<RegisterRoutes>();
        }
    }
}