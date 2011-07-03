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
    /// Defines an interface to update resource in RESTFul way.
    /// </summary>
    public interface IRESTFullUpdate<in TKey>
    {
        /// <summary>
        /// Shows the edit form to update resource.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        ActionResult Edit(TKey id);

        /// <summary>
        /// Updates the resource.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        ActionResult Update(TKey id, FormCollection fields);
    }
}