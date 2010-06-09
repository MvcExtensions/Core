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
    using System.Data.Metadata.Edm;
    using System.Data.Objects;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines an generic class of repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
    {
        private static readonly IDictionary<Type, EntityType> entityMapping = new Dictionary<Type, EntityType>();
        private static readonly object entityMappingSyncLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TEntity, TId&gt;"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        public Repository(ObjectContext database)
        {
            Invariant.IsNotNull(database, "database");

            Database = database;
        }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>The database.</value>
        protected ObjectContext Database
        {
            get; private set;
        }

        /// <summary>
        /// Adds the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public virtual void Add(TEntity instance)
        {
            Database.CreateObjectSet<TEntity>().AddObject(instance);
        }

        /// <summary>
        /// Removes the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public virtual void Remove(TEntity instance)
        {
            Database.CreateObjectSet<TEntity>().DeleteObject(instance);
        }

        /// <summary>
        /// Gets a single instance  for the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual TEntity One(TId id)
        {
            EntityType entityType = GetEntityType();
            string keyName = entityType.KeyMembers[0].Name;

            string queryString = string.Format(CultureInfo.InvariantCulture, "it.{0} = @{0}", keyName);

            ObjectQuery<TEntity> query = Database.CreateQuery<TEntity>(queryString);

            TEntity entity = query.Where(queryString, new ObjectParameter(keyName, id)).FirstOrDefault();

            return entity;
        }

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> All()
        {
            return Database.CreateObjectSet<TEntity>();
        }

        /// <summary>
        /// Gets the total count .
        /// </summary>
        /// <returns></returns>
        public virtual int Count()
        {
            return Database.CreateObjectSet<TEntity>().Count();
        }

        /// <summary>
        /// Finds  a single instance for the specified criteria.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> predicate)
        {
            return Database.CreateObjectSet<TEntity>().SingleOrDefault(predicate);
        }

        /// <summary>
        /// Finds all matching instances for the specified .criteria
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return Database.CreateObjectSet<TEntity>().Where(predicate);
        }

        /// <summary>
        /// Gets the Counts for the specified .criteria
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Database.CreateObjectSet<TEntity>().Count(predicate);
        }

        /// <summary>
        /// Checks whether instances exists for the specified .criteria
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return Database.CreateObjectSet<TEntity>().Any(predicate);
        }

        private EntityType GetEntityType()
        {
            Type type = typeof(TEntity);
            EntityType entityType;

            if (!entityMapping.TryGetValue(type, out entityType))
            {
                lock (entityMappingSyncLock)
                {
                    if (!entityMapping.TryGetValue(type, out entityType))
                    {
                        MetadataWorkspace workspace = Database.MetadataWorkspace;
                        workspace.LoadFromAssembly(type.Assembly);
                        EntityType ospaceEntityType;

                        if (workspace.TryGetItem(type.FullName, DataSpace.OSpace, out ospaceEntityType))
                        {
                            StructuralType cspaceEntityType;

                            if (workspace.TryGetEdmSpaceType(ospaceEntityType, out cspaceEntityType))
                            {
                                entityType = cspaceEntityType as EntityType;
                                entityMapping.Add(type, entityType);
                            }
                        }

                        if (entityType == null)
                        {
                            throw new InvalidProgramException(string.Format(CultureInfo.CurrentCulture, ExceptionMessages.UnableToFindAMatchingEntityTypeFor0, type.FullName));
                        }
                    }
                }
            }

            return entityType;
        }
    }
}