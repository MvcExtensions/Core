#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Demo.Web
{
    public class Product : EntityBase
    {
        public Category Category { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }
        public Supplier Supplier { get; set; }
    }
}
