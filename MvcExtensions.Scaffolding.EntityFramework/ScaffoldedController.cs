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

    /// <summary>
    /// Defines a <see cref="Controller"/> class which supports scaffolding.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public class ScaffoldedController<TEntity, TId> : Controller where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScaffoldedController&lt;TEntity, TId&gt;"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        public ScaffoldedController(ObjectContext database)
        {
            Invariant.IsNotNull(database, "database");

            Database = database;
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>The database.</value>
        protected ObjectContext Database
        {
            get;
            private set;
        }

        /// <summary>
        /// Renders the Index/list view.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            var entities = Database.CreateObjectSet<TEntity>().ToList();

            return View(entities);
        }

        /// <summary>
        /// Renders the details view.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual ActionResult Details(TId id)
        {
            var entity = GetObjectById(id);

            return View(entity);
        }

        /// <summary>
        /// Renders the create view
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates the object.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Create(FormCollection collection)
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

        /// <summary>
        /// Renders the edit view
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual ActionResult Edit(TId id)
        {
            var entity = GetObjectById(id);

            return View(entity);
        }

        /// <summary>
        /// Saves the edited object.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Edit(TId id, FormCollection collection)
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

        /// <summary>
        /// Renders the delete view.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual ActionResult Delete(TId id)
        {
            var entity = GetObjectById(id);

            return View(entity);
        }

        /// <summary>
        /// Deletes the object.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="confirm">The confirm.</param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Delete(TId id, bool? confirm)
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

        /// <summary>
        /// Redirects to list view.
        /// </summary>
        /// <returns></returns>
        protected virtual RedirectToRouteResult RedirectToList()
        {
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Gets the object by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        protected virtual TEntity GetObjectById(TId id)
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