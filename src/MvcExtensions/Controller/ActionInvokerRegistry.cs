#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Defines a class which is used to hold the action invoker mapping.
    /// </summary>
    public class ActionInvokerRegistry : IActionInvokerRegistry
    {
        private readonly IDictionary<Type, Type> mappings = new Dictionary<Type, Type>();

        /// <summary>
        /// Gets the mappings.
        /// </summary>
        /// <value>The mappings.</value>
        protected virtual IDictionary<Type, Type> Mappings
        {
            [DebuggerStepThrough]
            get
            {
                return mappings;
            }
        }

        /// <summary>
        /// Registers the specified mapping
        /// </summary>
        /// <param name="controllerType">Type of the controller.</param>
        /// <param name="actionInvokerType">Type of the controller factory.</param>
        public virtual void Register(Type controllerType, Type actionInvokerType)
        {
            Invariant.IsNotNull(controllerType, "controllerType");
            Invariant.IsNotNull(actionInvokerType, "actionInvokerType");

            if (!KnownTypes.ControllerType.IsAssignableFrom(controllerType))
            {
                throw new ArgumentException(ExceptionMessages.InvalidControllerTypeTypeMustBeAInheritedFromController, "controllerType");
            }

            if (!KnownTypes.ActionInvokerType.IsAssignableFrom(actionInvokerType))
            {
                throw new ArgumentException(ExceptionMessages.InvalidActionInvokerTypeTypeMustImplementIActionInvoker, "actionInvokerType");
            }

            Mappings.Add(controllerType, actionInvokerType);
        }

        /// <summary>
        /// Determines whether the specified controller action invoker  is registered.
        /// </summary>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns>
        /// <c>true</c> if the specified controller action invoker type is registered; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsRegistered(Type controllerType)
        {
            Invariant.IsNotNull(controllerType, "controllerType");

            return mappings.ContainsKey(controllerType);
        }

        /// <summary>
        /// Returns the matching action invoker type for the specified controller type.
        /// </summary>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns></returns>
        public virtual Type Matching(Type controllerType)
        {
            Invariant.IsNotNull(controllerType, "controllerType");

            return mappings[controllerType];
        }
    }
}