#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    /// <summary>
    /// Defines an interface which is used to persist the changes in database.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Commits tall the changes.
        /// </summary>
        void Commit();
    }
}