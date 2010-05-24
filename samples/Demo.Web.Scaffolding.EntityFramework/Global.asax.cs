namespace Demo.Web.Scaffolding.EntityFramework
{
    using MvcExtensions;
    using MvcExtensions.StructureMap;
    using MvcExtensions.Scaffolding.EntityFramework;

    public class MvcApplication : StructureMapMvcApplication
    {
        public MvcApplication()
        {
            RegisterControllerFactory.ControllerFactoryType = typeof(ScaffoldedControllerFactory);
        }
    }
}