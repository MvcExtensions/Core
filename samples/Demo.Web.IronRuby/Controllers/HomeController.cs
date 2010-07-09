namespace Demo.Web.IronRuby
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["message"] = "Welcome to Iron Ruby View Engine";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            if (string.IsNullOrEmpty(masterName))
            {
                masterName = "Site";
            }

            return base.View(viewName, masterName, model);
        }
    }
}