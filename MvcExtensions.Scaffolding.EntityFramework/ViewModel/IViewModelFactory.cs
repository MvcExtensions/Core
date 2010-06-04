#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System;

    /// <summary>
    /// Defines an interface which is used to dynamically generate the DisplayModel and EditModel for a given <seealso cref="EntityMetadata"/>.
    /// </summary>
    public interface IViewModelFactory
    {
        /// <summary>
        /// Creates the display model.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns></returns>
        Type CreateDisplayModel(Type modelType);

        /// <summary>
        /// Creates the edit model.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns></returns>
        Type CreateEditModel(Type modelType);
    }
}