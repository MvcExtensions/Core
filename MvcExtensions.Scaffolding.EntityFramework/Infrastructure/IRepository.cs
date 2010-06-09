#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines an generic interface of repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public interface IRepository<TEntity, in TId> where TEntity : class
    {
        /// <summary>
        /// Adds the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        void Add(TEntity instance);

        /// <summary>
        /// Removes the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        void Remove(TEntity instance);

        /// <summary>
        /// Gets a single instance  for the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        TEntity One(TId id);

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> All();

        /// <summary>
        /// Gets the total count .
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// Finds  a single instance for the specified criteria.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        TEntity FindOne(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Finds all matching instances for the specified .criteria
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets the Counts for the specified .criteria
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Checks whether instances exists for the specified .criteria
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> predicate);
    }
}