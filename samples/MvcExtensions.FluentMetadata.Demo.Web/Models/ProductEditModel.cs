#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Demo.Web
{
    using System.Web.Mvc.Html;
    using Resources;

    public class ProductEditModel
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
        public int? SupplierId { get; set; }
    }

    public class ProductEditModelConfiguration : ModelMetadataConfiguration<ProductEditModel>
    {
        public ProductEditModelConfiguration()
        {
            Configure(model => model.Id).Hide();

            Configure(model => model.Name).DisplayName(() => LocalizedTexts.Name)
                .Required(() => LocalizedTexts.NameCannotBeBlank)
                .MaximumLength(64, () => LocalizedTexts.NameCannotBeMoreThanSixtyFourCharacters);

            Configure(model => model.CategoryId).DisplayName(() => LocalizedTexts.Category)
                .Required(() => LocalizedTexts.CategoryMustBeSelected)
                .RenderAction(x => x.Action("Categories", "List"))
                .NullDisplayText(LocalizedTexts.SelectCategory);

            Configure(model => model.SupplierId).DisplayName(() => LocalizedTexts.Supplier)
                .Required(() => LocalizedTexts.SupplierMustBeSelected)
                .RenderAction(x => x.Action("Suppliers", "List"));

            Configure(model => model.Price).DisplayName(() => LocalizedTexts.Price)
                .FormatAsCurrency()
                .Required(() => LocalizedTexts.PriceCannotBeBlank)
                .Range(10.00m, 1000.00m, () => LocalizedTexts.PriceMustBeBetweenTenToThousand);
        }
    }
}
