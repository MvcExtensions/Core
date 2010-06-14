namespace Demo.Web
{
    using MvcExtensions;

    public class ProductDisplayModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public string SupplierName { get; set; }

        public decimal Price { get; set; }
    }

    public class ProductDisplayModelConfiguration : ModelMetadataConfiguration<ProductDisplayModel>
    {
        public ProductDisplayModelConfiguration()
        {
            Configure(model => model.Id).Hide();
            Configure(model => model.Name).DisplayName(() => LocalizedTexts.Name);
            Configure(model => model.CategoryName).DisplayName(() => LocalizedTexts.Category);
            Configure(model => model.SupplierName).DisplayName(() => LocalizedTexts.Supplier);
            Configure(model => model.Price).DisplayName(() => LocalizedTexts.Price).FormatAsCurrency();
        }
    }
}