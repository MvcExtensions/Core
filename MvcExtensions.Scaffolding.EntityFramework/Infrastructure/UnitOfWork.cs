#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System.Data.Objects;

    /// <summary>
    /// Defines a class which is used to persist all the changes in database.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        public UnitOfWork(ObjectContext database)
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
        /// Commits tall the changes.
        /// </summary>
        public virtual void Commit()
        {
            Database.SaveChanges();
        }
    }
}