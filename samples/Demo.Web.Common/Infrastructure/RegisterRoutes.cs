namespace Demo.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using MvcExtensions;

    public class RegisterRoutes : RegisterRoutesBase
    {
        protected override void Register(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = string.Empty }, new[] { "Demo.Web" });
        }
    }
}