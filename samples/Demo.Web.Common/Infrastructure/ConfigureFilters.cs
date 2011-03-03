namespace Demo.Web
{
    using MvcExtensions;

    public class ConfigureFilters : ConfigureFiltersBase
    {
        public ConfigureFilters(IFilterRegistry registry) : base(registry)
        {
        }

        protected override void Configure()
        {
            Registry.Register<ProductController, PopulateCategories, PopulateSuppliers>(c => c.Create())
                    .Register<ProductController, PopulateCategories, PopulateSuppliers>(c => c.Create(null))
                    .Register<ProductController, PopulateCategories, PopulateSuppliers>(c => c.Edit(0))
                    .Register<ProductController, PopulateCategories, PopulateSuppliers>(c => c.Edit(null));
        }
    }
}