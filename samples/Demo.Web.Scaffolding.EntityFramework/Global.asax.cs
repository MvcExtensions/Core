using MvcExtensions.Scaffolding.EntityFramework;

namespace Demo.Web.Scaffolding.EntityFramework
{
    using MvcExtensions;
    using MvcExtensions.StructureMap;

    public class MvcApplication : StructureMapMvcApplication
    {
        public MvcApplication()
        {
            RegisterControllerFactory.ControllerFactoryType = typeof(ScaffoldedControllerFactory);
        }
    }
}