namespace Demo.Web
{
    using System.ComponentModel;

    [TypeConverter(typeof(EntityConverter))]
    public class Supplier : EntityBase
    {
        public string CompanyName { get; set; }
    }
}