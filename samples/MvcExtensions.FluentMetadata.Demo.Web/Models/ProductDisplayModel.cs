#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Demo.Web
{
    using Resources;

    public class ProductDisplayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public string SupplierName { get; set; }
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
