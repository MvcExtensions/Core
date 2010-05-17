namespace Demo.Web
{
    using System;
    using System.ComponentModel;
    using System.Web.Mvc;

    using MvcExtensions;

    [BindingType(typeof(ProductEditModel))]
    public class ProductEditModelBinder : DefaultModelBinder
    {
        private readonly IRepository<Category> categoryRepository;
        private readonly IRepository<Supplier> supplierRepository;

        public ProductEditModelBinder(IRepository<Category> categoryRepository, IRepository<Supplier> supplierRepository)
        {
            this.categoryRepository = categoryRepository;
            this.supplierRepository = supplierRepository;
        }

        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            const string CategoryProperty = "Category";
            const string SupplierProperty = "Supplier";

            if (propertyDescriptor.Name.Equals(CategoryProperty, StringComparison.OrdinalIgnoreCase))
            {
                return GetValue(CategoryProperty, categoryRepository, bindingContext);
            }

            if (propertyDescriptor.Name.Equals(SupplierProperty, StringComparison.OrdinalIgnoreCase))
            {
                return GetValue(SupplierProperty, supplierRepository, bindingContext);
            }

            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }

        private static object GetValue<TEntity>(string propertyName, IRepository<TEntity> repository, ModelBindingContext bindingContext) where TEntity : EntityBase
        {
            ValueProviderResult result = bindingContext.ValueProvider.GetValue(propertyName);
            int? id = (result != null) ? (int?)result.ConvertTo(typeof(int?)) : null;

            return (id.HasValue && id.Value > 0) ? repository.Get(id.Value) : default(TEntity);
        }
    }
}