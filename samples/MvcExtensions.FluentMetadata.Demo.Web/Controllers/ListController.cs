#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Demo.Web
{
    using System.Web.Mvc;

    [ChildActionOnly, SelectListAction]
    public class ListController : Controller
    {
        public ActionResult Categories(int? selected)
        {
            var repository = new Repository<Category>(MvcApplication.Database);
            var model = new SelectList(repository.All(), "Id", "Name", selected);
            return PartialView("DropDownList", model);
        }

        public ActionResult Suppliers(int? selected)
        {
            var repository = new Repository<Supplier>(MvcApplication.Database);
            var model = new SelectList(repository.All(), "Id", "CompanyName", selected);
            return PartialView("ListBox", model);
        }
    }
}
