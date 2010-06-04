namespace Demo.Web
{
    using System.Data.Objects;

    using StructureMap.Configuration.DSL;

    using Scaffolding.EntityFramework.Models;

    public class RegisterServices : Registry
    {
        public RegisterServices()
        {
            For<ObjectContext>().HttpContextScoped().Use(() => new Northwind());
        }
    }
}