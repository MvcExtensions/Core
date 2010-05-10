namespace Demo.Web
{
    using System;
    using System.Collections.Generic;

    public class InMemoryDatabasae : IDatabase
    {
        private static readonly IDictionary<Type, object> dataStore = BuildDatabase();

        public IList<TEntity> GetObjectSet<TEntity>() where TEntity : EntityBase
        {
            return dataStore[typeof(TEntity)] as IList<TEntity>;
        }

        private static IDictionary<Type, object> BuildDatabase()
        {
            IList<Category> categories = BuildCategories();
            IList<Supplier> suppliers = BuildSuppliers();
            IList<Product> products = BuildProducts(categories, suppliers);

            return new Dictionary<Type, object>
                       {
                           { typeof(Category), categories },
                           { typeof(Supplier), suppliers },
                           { typeof(Product), products }
                       };
        }

        private static IList<Category> BuildCategories()
        {
            IList<Category> categories = new List<Category>();

            for (int i = 1; i <= 5; i ++)
            {
                categories.Add(new Category
                                   {
                                       Id = i,
                                       Name = string.Format("Category {0}", i)
                                   });
            }

            return categories;
        }

        private static IList<Supplier> BuildSuppliers()
        {
            IList<Supplier> suppliers = new List<Supplier>();

            for (int i = 1; i <= 7; i++)
            {
                suppliers.Add(new Supplier
                                  {
                                      Id = i,
                                      CompanyName = string.Format("Supplier {0}", i)
                                  });
            }

            return suppliers;
        }

        private static IList<Product> BuildProducts(IList<Category> categoris, IList<Supplier> suppliers)
        {
            Random rnd = new Random();
            IList<Product> products = new List<Product>();

            for (int i = 1; i <= 20; i++)
            {
                products.Add(new Product
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
    }
}