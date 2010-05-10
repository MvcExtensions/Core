namespace Demo.Web
{
    using MvcExtensions;

    public class ConfigureFilters : ConfigureFiltersBase
    {
        protected override void Configure(IFilterRegistry registry)
        {
            registry.Register<ProductController, PopulateCategoriesAttribute, PopulateSuppliersAttribute>(c => c.Create())
                    .Register<ProductController, PopulateCategoriesAttribute, PopulateSuppliersAttribute>(c => c.Create(null))
                    .Register<ProductController, PopulateCategoriesAttribute, PopulateSuppliersAttribute>(c => c.Edit(0))
                    .Register<ProductController, PopulateCategoriesAttribute, PopulateSuppliersAttribute>(c => c.Edit(null));
        }
    }
}