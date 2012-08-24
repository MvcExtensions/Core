#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Demo.Web
{
    using System.Collections.Generic;

    public interface IRepository<TEntity>
        where TEntity : EntityBase
    {
        void Add(TEntity entity);

        IEnumerable<TEntity> All();

        void Delete(int id);

        TEntity Get(int id);

        void Update(TEntity entity);
    }
}
