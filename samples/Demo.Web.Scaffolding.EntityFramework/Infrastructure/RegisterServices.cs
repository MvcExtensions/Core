namespace Demo.Web
{
    using StructureMap.Configuration.DSL;

    using Scaffolding.EntityFramework.Models;

    public class RegisterServices : Registry
    {
        public RegisterServices()
        {
            For<Northwind>().Use<Northwind>();
        }
    }
}