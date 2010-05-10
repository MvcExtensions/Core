namespace Demo.Web
{
    using System.Collections.Generic;

    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(int id);

        TEntity Get(int id);

        IEnumerable<TEntity> All();
    }
}