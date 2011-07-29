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
    /// Defines an interface to create resource in RESTFul way.
    /// </summary>
    public interface IRESTFulCreate
    {
        /// <summary>
        /// Shows the form to create new resource.
        /// </summary>
        /// <returns></returns>
        ActionResult New();

        /// <summary>
        /// Creates the specified fields.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        ActionResult Create(FormCollection fields);
    }
}