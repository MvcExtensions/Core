namespace Demo.Web
{
    using System.Linq;
    using System.Web.Mvc;

    [HandleError]
    public class ProductController : Controller
    {
        private readonly IRepository<Product> repository;

        public ProductController(IRepository<Product> repository)
        {
            this.repository = repository;
        }

        public ActionResult Index()
        {
            return View(repository.All().Select(product => product.AsDisplayModel()));
        }

        public ActionResult Details(int id)
        {
            return View(repository.Get(id).AsDisplayModel());
        }

        public ActionResult Create()
        {
            return View(new ProductEditModel());
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id")]ProductEditModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = model.AsProduct();
                product.Id = repository.All().LastOrDefault().Id + 1;

                repository.Add(product);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            return View(repository.Get(id).AsEditModel());
        }

        [HttpPost]
        public ActionResult Edit(ProductEditModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = model.AsProduct();

                repository.Update(product);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            return View(repository.Get(id).AsDisplayModel());
        }

        [HttpPost]
        public ActionResult Delete(int id, string confirm)
        {
            repository.Delete(id);

            return RedirectToAction("Index");
        }
    }
}