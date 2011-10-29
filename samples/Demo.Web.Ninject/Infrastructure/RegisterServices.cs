namespace Demo.Web.Ninject
{
    public class RegisterServices : global::Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IDatabase>().To<InMemoryDatabase>().InRequestScope();
            Bind(typeof(IRepository<>)).To(typeof(Repository<>)).InRequestScope();
        }
    }
}