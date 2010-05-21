#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;

    /// <summary>
    /// Defines an interface which is used to hold the action invoker mapping.
    /// </summary>
    public interface IActionInvokerRegistry
    {
        /// <summary>
        /// Registers the specified mapping
        /// </summary>
        /// <param name="controllerType">Type of the controller.</param>
        /// <param name="actionInvokerType">Type of the controller factory.</param>
        void Register(Type controllerType, Type actionInvokerType);

        /// <summary>
        /// Determines whether the specified controller action invoker  is registered.
        /// </summary>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns>
        /// <c>true</c> if the specified controller action invoker type is registered; otherwise, <c>false</c>.
        /// </returns>
        bool IsRegistered(Type controllerType);

        /// <summary>
        /// Returns the matching action invoker type for the specified controller type.
        /// </summary>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns></returns>
        Type Matching(Type controllerType);
    }
}