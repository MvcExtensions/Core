namespace Demo.Web
{
    using System.Web.Mvc;

    public sealed class PopulateSuppliers : PopulateModel
    {
        private readonly IRepository<Supplier> repository;

        public PopulateSuppliers(IRepository<Supplier> repository)
        {
            this.repository = repository;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ProductEditModel editModel = filterContext.Controller.ViewData.Model as ProductEditModel;

            if (editModel != null)
            {
                filterContext.Controller.ViewData["suppliers"] = new SelectList(repository.All(), "Id", "CompanyName", editModel.Supplier);
            }
        }
    }
}