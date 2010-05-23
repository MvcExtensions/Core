#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System;
    using System.Data.Metadata.Edm;
    using System.Data.Objects;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    public class ScaffoldedController<TEntity, TId> : Controller where TEntity : class
    {
        public ScaffoldedController(ObjectContext database)
        {
            Invariant.IsNotNull(database, "database");

            Database = database;
        }

        protected ObjectContext Database
        {
            get;
            private set;
        }

        public ActionResult Index()
        {
            var entities = Database.CreateObjectSet<TEntity>();

            return View(entities);
        }

        public ActionResult Details(TId id)
        {
            var entity = GetObjectById(id);

            return View(entity);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            TEntity entity = Database.CreateObject<TEntity>();

            try
            {
                UpdateModel(entity, collection.ToValueProvider());

                Database.CreateObjectSet<TEntity>().AddObject(entity);
                Database.SaveChanges();

                return RedirectToList();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(TId id)
        {
            var entity = GetObjectById(id);

            return View(entity);
        }

        [HttpPost]
        public ActionResult Edit(TId id, FormCollection collection)
        {
            var entity = GetObjectById(id);

            try
            {
                UpdateModel(entity, collection.ToValueProvider());
                Database.SaveChanges();

                return RedirectToList();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(TId id)
        {
            var entity = GetObjectById(id);

            return View(entity);
        }

        [HttpPost]
        public ActionResult Delete(TId id, bool? confirm)
        {
            TEntity entity = GetObjectById(id);

            try
            {
                Database.CreateObjectSet<TEntity>().DeleteObject(entity);
                Database.SaveChanges();

                return RedirectToList();
            }
            catch
            {
                return View();
            }
        }

        private RedirectToRouteResult RedirectToList()
        {
            return RedirectToAction("Index");
        }

        private TEntity GetObjectById(TId id)
        {
            var keyName = Database.MetadataWorkspace
                                  .GetItem<EntityType>(typeof(TEntity).Name, DataSpace.CSpace)
                                  .KeyMembers.First().Name;

            var param = Expression.Parameter(typeof(TEntity), "x");
            var left = Expression.Property(param, keyName);
            var right = Expression.Constant(id);
            var equal = Expression.Equal(left, right);

            var predicate = Expression.Lambda<Func<TEntity, bool>>(equal, param);

            var entity = Database.CreateObjectSet<TEntity>().SingleOrDefault(predicate);

            return entity;
        }
    }
}