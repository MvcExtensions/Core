namespace Demo.Web
{
    public static class ModelExtensions
    {
        public static ProductDisplayModel AsDisplayModel(this Product product)
        {
            return new ProductDisplayModel
                       {
                           Id = product.Id,
                           Name = product.Name,
                           CategoryName = (product.Category != null) ? product.Category.Name : string.Empty,
                           SupplierName = (product.Supplier != null) ? product.Supplier.CompanyName : string.Empty,
                           Price = product.Price
                       };
        }

        public static ProductEditModel AsEditModel(this Product product)
        {
            return new ProductEditModel
                       {
                           Id = product.Id,
                           Name = product.Name,
                           Category = product.Category,
                           Supplier = product.Supplier,
                           Price = product.Price
                       };
        }

        public static Product AsProduct(this ProductEditModel model)
        {
            return new Product
            {
                Id = model.Id,
                Name = model.Name,
                Category = model.Category,
                Supplier = model.Supplier,
                Price = model.Price
            };
        }
    }
}