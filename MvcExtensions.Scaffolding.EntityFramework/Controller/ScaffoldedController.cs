#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a <see cref="Controller"/> class which supports scaffolding.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public class ScaffoldedController<TEntity, TViewModel, TId> : Controller where TEntity : class where TViewModel : IViewModel, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScaffoldedController&lt;TEntity, TViewModel, TId&gt;"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public ScaffoldedController(IRepository<TEntity, TId> repository, IMapper<TEntity, TViewModel> mapper, IUnitOfWork unitOfWork)
        {
            Invariant.IsNotNull(repository, "repository");
            Invariant.IsNotNull(mapper, "mapper");

            Repository = repository;
            Mapper = mapper;
            UnitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <value>The repository.</value>
        protected IRepository<TEntity, TId> Repository
        {
            get; private set;
        }

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        protected IMapper<TEntity, TViewModel> Mapper
        {
             get; private set;
        }

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <value>The unit of work.</value>
        protected IUnitOfWork UnitOfWork
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
            IEnumerable<TEntity> entities = Repository.All();
            IEnumerable<TViewModel> viewModels = entities.Select(e => Mapper.Map(e));

            return View(viewModels);
        }

        /// <summary>
        /// Renders the details view.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual ActionResult Details(TId id)
        {
            TEntity entity = Repository.One(id);
            TViewModel viewModel = Mapper.Map(entity);

            return View(viewModel);
        }

        /// <summary>
        /// Renders the create view
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Create()
        {
            return View(new TViewModel());
        }

        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Create(TViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                TEntity entity = Mapper.CreateFrom(viewModel);

                Repository.Add(entity);
                UnitOfWork.Commit();

                return RedirectToList();
            }

            return View();
        }

        /// <summary>
        /// Renders the edit view
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual ActionResult Edit(TId id)
        {
            TEntity entity = Repository.One(id);
            TViewModel viewModel = Mapper.Map(entity);

            return View(viewModel);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Edit(TId id, TViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                TEntity entity = Repository.One(id);
                Mapper.Copy(viewModel, entity);

                UnitOfWork.Commit();

                return RedirectToList();
            }

            return View();
        }

        /// <summary>
        /// Renders the delete view.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual ActionResult Delete(TId id)
        {
            TEntity entity = Repository.One(id);
            TViewModel viewModel = Mapper.Map(entity);

            return View(viewModel);
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="confirm">The confirm.</param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Delete(TId id, bool? confirm)
        {
            TEntity entity = Repository.One(id);

            Repository.Remove(entity);
            UnitOfWork.Commit();

            return RedirectToList();
        }

        /// <summary>
        /// Redirects to list view.
        /// </summary>
        /// <returns></returns>
        protected virtual RedirectToRouteResult RedirectToList()
        {
            return RedirectToAction("Index");
        }
    }
}