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
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Defines an static class which contains extension methods of <see cref="IActionInvokerRegistry"/>.
    /// </summary>
    public static class ActionInvokerRegistryExtensions
    {
        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TActionInvoker">The type of the action invoker.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IActionInvokerRegistry Register<TController, TActionInvoker>(this IActionInvokerRegistry instance) where TController : Controller where TActionInvoker : IActionInvoker
        {
            Invariant.IsNotNull(instance, "instance");

            instance.Register(typeof(TController), typeof(TActionInvoker));

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TController1">The type of the controller1.</typeparam>
        /// <typeparam name="TController2">The type of the controller2.</typeparam>
        /// <typeparam name="TActionInvoker">The type of the action invoker.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IActionInvokerRegistry Register<TController1, TController2, TActionInvoker>(this IActionInvokerRegistry instance)
            where TController1 : Controller
            where TController2 : Controller
            where TActionInvoker : IActionInvoker
        {
            Invariant.IsNotNull(instance, "instance");

            Type actionInvokerType = typeof(TActionInvoker);

            instance.Register(typeof(TController1), actionInvokerType);
            instance.Register(typeof(TController2), actionInvokerType);

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TController1">The type of the controller1.</typeparam>
        /// <typeparam name="TController2">The type of the controller2.</typeparam>
        /// <typeparam name="TController3">The type of the controller3.</typeparam>
        /// <typeparam name="TActionInvoker">The type of the action invoker.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IActionInvokerRegistry Register<TController1, TController2, TController3, TActionInvoker>(this IActionInvokerRegistry instance)
            where TController1 : Controller
            where TController2 : Controller
            where TController3 : Controller
            where TActionInvoker : IActionInvoker
        {
            Invariant.IsNotNull(instance, "instance");

            Type actionInvokerType = typeof(TActionInvoker);

            instance.Register(typeof(TController1), actionInvokerType);
            instance.Register(typeof(TController2), actionInvokerType);
            instance.Register(typeof(TController3), actionInvokerType);

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TController1">The type of the controller1.</typeparam>
        /// <typeparam name="TController2">The type of the controller2.</typeparam>
        /// <typeparam name="TController3">The type of the controller3.</typeparam>
        /// <typeparam name="TController4">The type of the controller4.</typeparam>
        /// <typeparam name="TActionInvoker">The type of the action invoker.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IActionInvokerRegistry Register<TController1, TController2, TController3, TController4, TActionInvoker>(this IActionInvokerRegistry instance)
            where TController1 : Controller
            where TController2 : Controller
            where TController3 : Controller
            where TController4 : Controller
            where TActionInvoker : IActionInvoker
        {
            Invariant.IsNotNull(instance, "instance");

            Type actionInvokerType = typeof(TActionInvoker);

            instance.Register(typeof(TController1), actionInvokerType);
            instance.Register(typeof(TController2), actionInvokerType);
            instance.Register(typeof(TController3), actionInvokerType);
            instance.Register(typeof(TController4), actionInvokerType);

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TActionInvoker">The type of the action invoker.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="typeCatalog">The type catalog.</param>
        /// <returns></returns>
        public static IActionInvokerRegistry Register<TActionInvoker>(this IActionInvokerRegistry instance, TypeCatalog typeCatalog)
        {
            Invariant.IsNotNull(instance, "instance");
            Invariant.IsNotNull(typeCatalog, "typeCatalog");

            IList<Type> controllerTypes = typeCatalog.ToList();
            EnsureControllerTypes(controllerTypes);

            Type actionInvokerType = typeof(TActionInvoker);

            foreach (Type controllerType in controllerTypes)
            {
                instance.Register(controllerType, actionInvokerType);
            }

            return instance;
        }

        private static void EnsureControllerTypes(IEnumerable<Type> typeCatalog)
        {
            foreach (Type controllerType in typeCatalog.Where(controllerType => !KnownTypes.ControllerType.IsAssignableFrom(controllerType)))
            {
                throw new ArgumentException(string.Format(Culture.CurrentUI, ExceptionMessages.IsNotATargetType, controllerType.FullName, KnownTypes.ControllerType.FullName));
            }
        }
    }
}