namespace Demo.Web.Autofac
{
    using global::Autofac;
    using global::Autofac.Integration.Mvc;

    public class RegisterServices : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryDatabase>().As<IDatabase>().InstancePerHttpRequest();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));

            base.Load(builder);
        }
    }
}