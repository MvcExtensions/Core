namespace Demo.Web
{
    using System.Collections.Generic;
    using System.Linq;

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
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
            IList<TEntity> objectSet = database.GetObjectSet<TEntity>();

            objectSet[objectSet.IndexOf(objectSet.Single(e => e.Id == entity.Id))] = entity;
        }

        public void Delete(int id)
        {
            IList<TEntity> objectSet = database.GetObjectSet<TEntity>();

            TEntity entity = objectSet.SingleOrDefault(e => e.Id == id);
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