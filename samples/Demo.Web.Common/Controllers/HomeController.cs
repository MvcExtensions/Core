namespace Demo.Web
{
    using System.Web.Mvc;

    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC Extensibility!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}