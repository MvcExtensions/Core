namespace Demo.Web
{
    using System.Data.Objects;

    using StructureMap.Configuration.DSL;

    using MvcExtensions.Scaffolding.EntityFramework;

    using Scaffolding.EntityFramework.Models;

    public class RegisterServices : Registry
    {
        public RegisterServices()
        {
            For<ObjectContext>().HttpContextScoped().Use(() => new Northwind());
            For<IEntityFrameworkMetadataProvider>().Singleton().Use<EntityFrameworkMetadataProvider>();
            For(typeof(ScaffoldedController<,>)).Use(typeof(ScaffoldedController<,>));
        }
    }
}