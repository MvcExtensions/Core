namespace Demo.Web
{
    using System.Web.Mvc;

    public abstract class PopulateModelAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        public abstract void OnActionExecuted(ActionExecutedContext filterContext);
    }
}