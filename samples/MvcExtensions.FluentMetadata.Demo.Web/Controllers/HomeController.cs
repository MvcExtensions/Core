#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Demo.Web
{
    using System.Web.Mvc;

    public class HomeController : ControllerBase
    {
        public ActionResult Fluent()
        {
            return View(new ConventionalFluentModel());
        }

        [HttpPost]
        public ActionResult Fluent(ConventionalFluentModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Fluent");
            }

            return View(model);
        }

        public ActionResult Index()
        {
            return View(new ConventionalDataAnnotationsModel());
        }

        [HttpPost]
        public ActionResult Index(ConventionalDataAnnotationsModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
