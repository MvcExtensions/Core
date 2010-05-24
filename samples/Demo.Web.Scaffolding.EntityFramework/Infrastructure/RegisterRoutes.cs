namespace Demo.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using MvcExtensions;

    public class RegisterRoutes : RegisterRoutesBase
    {
        public RegisterRoutes(RouteCollection routes) : base(routes)
        {
        }

        protected override void Register()
        {
            Routes.IgnoreRoute("favicon.ico");
            Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            Routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}