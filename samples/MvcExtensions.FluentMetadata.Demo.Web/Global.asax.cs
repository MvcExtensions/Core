#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Demo.Web
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class MvcApplication : HttpApplication
    {
        public static InMemoryDatabase Database { get; set; }

        protected MvcApplication()
        {
            Database = new InMemoryDatabase();
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = false;
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*robotstxt}", new { robotstxt = @"(.*/)?robots.txt(/.*)?" });
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Default_Localizable",
                // Route name
                "{culture}/{controller}/{action}/{id}",
                // URL with parameters
                new { culture = UrlParameter.Optional, controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);         

            // ConventionSettings.ConventionsActive = true; - actomatically activated when DefaultResourceType is set
            ConventionSettings.DefaultResourceType = typeof(Resources.LocalizedTexts);
            // you can require convension attribute to apply a convensions to the class
            // ConventionSettings.RequireConventionAttribute = false;

            /*
            // To use Fluent metadata with IoC you can use the following code (for Castle.Windsor)
            var container = new WindsorContainer();
            ...
            FluentMetadataConfiguration
                .RegisterEachConfigurationWithContainer(
                    data => container.Register(
                        Component
                            .For(data.InterfaceType).ImplementedBy(data.MetadataConfigurationType)
                            .LifestyleTransient()))
                .ConstructMetadataUsing(container.ResolveAll<IModelMetadataConfiguration>)
                .Register();

            DependencyResolver.SetResolver(...);
             */

            FluentMetadataConfiguration.Register();
        }
    }
}
