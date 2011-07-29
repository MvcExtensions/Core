#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web.Mvc;

    /// <summary>
    /// Defines an interface to destroy resource in RESTFul way.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IRESTFulDestroy<in TKey>
    {
        /// <summary>
        /// Destroys the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        ActionResult Destroy(TKey id);
    }
}