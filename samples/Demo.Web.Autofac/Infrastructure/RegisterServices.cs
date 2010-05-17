namespace Demo.Web.Autofac
{
    using ContainerBuilder = global::Autofac.ContainerBuilder;
    using Module = global::Autofac.Module;
    using RegisterExtension = global::Autofac.RegistrationExtensions;

    public class RegisterServices : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterExtension.RegisterType<InMemoryDatabasae>(builder).As<IDatabase>();
            RegisterExtension.RegisterGeneric(builder, typeof(Repository<>)).As(typeof(IRepository<>));

            base.Load(builder);
        }
    }
}