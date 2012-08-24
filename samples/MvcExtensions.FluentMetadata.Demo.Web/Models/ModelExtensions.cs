#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Demo.Web
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
                    CategoryId = product.Category != null ? product.Category.Id : (int?)null ,
                    SupplierId = product.Supplier != null ? product.Supplier.Id : (int?)null,
                    Price = product.Price
                };
        }

        public static Product AsProduct(this ProductEditModel model)
        {
            return new Product
                {
                    Id = model.Id,
                    Name = model.Name,
                    Price = model.Price
                };
        }
    }
}
