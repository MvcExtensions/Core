#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Demo.Web
{
    using System.Linq;
    using System.Web.Mvc;

    [HandleError]
    public class ProductController : ControllerBase
    {
        private readonly Repository<Product> repository;
        private readonly Repository<Supplier> supplierRepository;
        private readonly Repository<Category> categoryRepository;

        public ProductController()
        {
            repository = new Repository<Product>(MvcApplication.Database);
            supplierRepository = new Repository<Supplier>(MvcApplication.Database);
            categoryRepository = new Repository<Category>(MvcApplication.Database);
        }

        public ActionResult Index()
        {
            return View(repository.All().Select(product => product.AsDisplayModel()));
        }

        public ActionResult Details(int id)
        {
            return View(repository.Get(id).AsDisplayModel());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new ProductEditModel());
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id")] ProductEditModel model)
        {
            if (ModelState.IsValid)
            {
                var product = model.AsProduct();
                product.Id = repository.All().Last().Id + 1;
                PopulateProduct(model, product);

                repository.Add(product);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(repository.Get(id).AsEditModel());
        }

        [HttpPost]
        public ActionResult Edit(ProductEditModel model)
        {
            if (ModelState.IsValid)
            {
                var product = model.AsProduct();
                PopulateProduct(model, product);

                repository.Update(product);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        private void PopulateProduct(ProductEditModel model, Product product)
        {
            if (model.SupplierId != null)
            {
                product.Supplier = supplierRepository.Get(model.SupplierId.Value);
            }
            if (model.CategoryId != null)
            {
                product.Category = categoryRepository.Get(model.CategoryId.Value);
            }
        }

        public ActionResult Delete(int id)
        {
            return View(repository.Get(id).AsDisplayModel());
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            repository.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
