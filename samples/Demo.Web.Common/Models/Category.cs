namespace Demo.Web
{
    using System.ComponentModel;

    [TypeConverter(typeof(EntityConverter))]
    public class Category : EntityBase
    {
        public string Name { get; set; }
    }
}