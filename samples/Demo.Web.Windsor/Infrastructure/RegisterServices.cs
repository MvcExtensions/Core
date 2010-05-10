namespace Demo.Web.Windsor
{
    using Castle.Core;
    using Castle.Windsor;

    using MvcExtensions.Windsor;

    public class RegisterServices : IModule
    {
        public void Load(IWindsorContainer container)
        {
            container.AddComponentLifeStyle<IDatabase, InMemoryDatabasae>(typeof(IDatabase).FullName, LifestyleType.PerWebRequest)
                     .AddComponentLifeStyle(typeof(IRepository<>).FullName, typeof(IRepository<>), typeof(Repository<>), LifestyleType.PerWebRequest);
        }
    }
}