#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Demo.Web
{
    using System;
    using System.Collections.Generic;

    public class InMemoryDatabase : IDatabase
    {
        private static readonly IDictionary<Type, object> DataStore = BuildDatabase();

        public IList<TEntity> GetObjectSet<TEntity>() where TEntity : EntityBase
        {
            return DataStore[typeof(TEntity)] as IList<TEntity>;
        }

        private static IList<Category> BuildCategories()
        {
            IList<Category> categories = new List<Category>();

            for (var i = 1; i <= 5; i ++)
            {
                categories.Add(
                    new Category
                        {
                            Id = i,
                            Name = string.Format("Category {0}", i)
                        });
            }

            return categories;
        }

        private static IDictionary<Type, object> BuildDatabase()
        {
            var categories = BuildCategories();
            var suppliers = BuildSuppliers();
            var products = BuildProducts(categories, suppliers);

            return new Dictionary<Type, object>
                {
                    { typeof(Category), categories },
                    { typeof(Supplier), suppliers },
                    { typeof(Product), products }
                };
        }

        private static IList<Product> BuildProducts(IList<Category> categoris, IList<Supplier> suppliers)
        {
            var rnd = new Random();
            IList<Product> products = new List<Product>();

            for (var i = 1; i <= 20; i++)
            {
                products.Add(
                    new Product
                        {
                            Id = i,
                            Name = string.Format("Product {0}", i),
                            Category = categoris[rnd.Next(0, categoris.Count)],
                            Supplier = suppliers[rnd.Next(0, suppliers.Count)],
                            Price = rnd.Next(10, 1000)
                        });
            }

            return products;
        }

        private static IList<Supplier> BuildSuppliers()
        {
            IList<Supplier> suppliers = new List<Supplier>();

            for (var i = 1; i <= 7; i++)
            {
                suppliers.Add(
                    new Supplier
                        {
                            Id = i,
                            CompanyName = string.Format("Supplier {0}", i)
                        });
            }

            return suppliers;
        }
    }
}
