namespace Demo.Web
{
    using System.Web.Mvc;

    public abstract class PopulateModel : IMvcFilter, IActionFilter
    {
        public bool AllowMultiple
        {
            get { return false; }
        }

        public int Order { get; set; }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        public abstract void OnActionExecuted(ActionExecutedContext filterContext);
    }
}