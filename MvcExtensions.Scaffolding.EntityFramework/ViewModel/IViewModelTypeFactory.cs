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
    /// Defines an interface which is used to dynamically generate theViewModel for a given type.
    /// </summary>
    public interface IViewModelTypeFactory
    {
        /// <summary>
        /// Creates the specified model type.
        /// </summary>
        /// <param name="entityType">Type of the model.</param>
        /// <returns></returns>
        Type Create(Type entityType);
    }
}