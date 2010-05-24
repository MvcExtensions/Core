namespace Demo.Web.Scaffolding.EntityFramework.Models
{
    using MvcExtensions;

    public class CategoryConfiguration : ModelMetadataConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            Configure(c => c.CategoryID).DisplayName("Id");
            Configure(c => c.CategoryName).DisplayName("Name");
        }
    }
}