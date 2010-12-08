namespace Demo.Web.StructureMap
{
    using MvcExtensions;
    using MvcExtensions.StructureMap;

    public class MvcApplication : StructureMapMvcApplication
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