namespace Demo.Web.StructureMap
{
    using Registry = global::StructureMap.Configuration.DSL.Registry;

    public class RegisterServices : Registry
    {
        public RegisterServices()
        {
            For<IDatabase>().HttpContextScoped().Use<InMemoryDatabase>();
            For(typeof(IRepository<>)).HttpContextScoped().Use(typeof(Repository<>));
        }
    }
}