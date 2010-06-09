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
    /// Defines an interface for storing scaffolded controller types
    /// </summary>
    public interface IControllerTypeRegistry
    {
        /// <summary>
        /// Gets the  scaffolded controller type for specified controller name.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns></returns>
        Type GetControllerType(string controllerName);
    }
}