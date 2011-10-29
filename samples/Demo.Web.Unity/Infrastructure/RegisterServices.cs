namespace Demo.Web.Unity
{
    using Microsoft.Practices.Unity;

    using MvcExtensions.Unity;

    public class RegisterServices : IModule
    {
        public void Load(IUnityContainer container)
        {
            container.RegisterType<IDatabase, InMemoryDatabase>(new PerRequestLifetimeManager())
                     .RegisterType(typeof(IRepository<>), typeof(Repository<>), new PerRequestLifetimeManager());
        }
    }
}