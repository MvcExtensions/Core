#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Demo.Web
{
    using System.Collections.Generic;
    using System.Linq;

    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : EntityBase
    {
        private readonly IDatabase database;

        public Repository(IDatabase database)
        {
            this.database = database;
        }

        public void Add(TEntity entity)
        {
            database.GetObjectSet<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            var objectSet = database.GetObjectSet<TEntity>();

            objectSet[objectSet.IndexOf(objectSet.Single(e => e.Id == entity.Id))] = entity;
        }

        public void Delete(int id)
        {
            var objectSet = database.GetObjectSet<TEntity>();

            var entity = objectSet.SingleOrDefault(e => e.Id == id);
            objectSet.Remove(entity);
        }

        public TEntity Get(int id)
        {
            return database.GetObjectSet<TEntity>().SingleOrDefault(e => e.Id == id);
        }

        public IEnumerable<TEntity> All()
        {
            return database.GetObjectSet<TEntity>();
        }
    }
}
