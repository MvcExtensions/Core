namespace Demo.Web
{
    using System;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class PopulateCategoriesAttribute : PopulateModelAttribute
    {
        private readonly IRepository<Category> repository;

        public PopulateCategoriesAttribute(IRepository<Category> repository)
        {
            this.repository = repository;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ProductEditModel editModel = filterContext.Controller.ViewData.Model as ProductEditModel;

            if (editModel != null)
            {
                filterContext.Controller.ViewData["categories"] = new SelectList(repository.All(), "Id", "Name", editModel.Category);
            }
        }
    }
}