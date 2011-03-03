namespace Demo.Web
{
    using System.Web.Mvc;

    public sealed class PopulateCategories : PopulateModel
    {
        private readonly IRepository<Category> repository;

        public PopulateCategories(IRepository<Category> repository)
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