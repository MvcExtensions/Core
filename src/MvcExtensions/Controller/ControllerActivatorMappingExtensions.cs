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
    /// Defines an static class which contains extension methods of <see cref="TypeMappingRegistry{Controller, IControllerActivator}"/>.
    /// </summary>
    public static class ControllerActivatorMappingExtensions
    {
        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TControllerActivator">The type of the controller activator.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static TypeMappingRegistry<Controller, IControllerActivator> Register<TController, TControllerActivator>(this TypeMappingRegistry<Controller, IControllerActivator> instance)
            where TController : Controller
            where TControllerActivator : IControllerActivator
        {
            Invariant.IsNotNull(instance, "instance");

            instance.Register(typeof(TController), typeof(TControllerActivator));

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TController1">The type of the controller1.</typeparam>
        /// <typeparam name="TController2">The type of the controller2.</typeparam>
        /// <typeparam name="TControllerActivator">The type of the controller activator.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static TypeMappingRegistry<Controller, IControllerActivator> Register<TController1, TController2, TControllerActivator>(this TypeMappingRegistry<Controller, IControllerActivator> instance)
            where TController1 : Controller
            where TController2 : Controller
            where TControllerActivator : IControllerActivator
        {
            Invariant.IsNotNull(instance, "instance");

            Type controllerActivatorType = typeof(TControllerActivator);

            instance.Register(typeof(TController1), controllerActivatorType);
            instance.Register(typeof(TController2), controllerActivatorType);

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TController1">The type of the controller1.</typeparam>
        /// <typeparam name="TController2">The type of the controller2.</typeparam>
        /// <typeparam name="TController3">The type of the controller3.</typeparam>
        /// <typeparam name="TControllerActivator">The type of the action invoker.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static TypeMappingRegistry<Controller, IControllerActivator> Register<TController1, TController2, TController3, TControllerActivator>(this TypeMappingRegistry<Controller, IControllerActivator> instance)
            where TController1 : Controller
            where TController2 : Controller
            where TController3 : Controller
            where TControllerActivator : IControllerActivator
        {
            Invariant.IsNotNull(instance, "instance");

            Type controllerActivatorType = typeof(TControllerActivator);

            instance.Register(typeof(TController1), controllerActivatorType);
            instance.Register(typeof(TController2), controllerActivatorType);
            instance.Register(typeof(TController3), controllerActivatorType);

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TController1">The type of the controller1.</typeparam>
        /// <typeparam name="TController2">The type of the controller2.</typeparam>
        /// <typeparam name="TController3">The type of the controller3.</typeparam>
        /// <typeparam name="TController4">The type of the controller4.</typeparam>
        /// <typeparam name="TControllerActivator">The type of the controller activator.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static TypeMappingRegistry<Controller, IControllerActivator> Register<TController1, TController2, TController3, TController4, TControllerActivator>(this TypeMappingRegistry<Controller, IControllerActivator> instance)
            where TController1 : Controller
            where TController2 : Controller
            where TController3 : Controller
            where TController4 : Controller
            where TControllerActivator : IControllerActivator
        {
            Invariant.IsNotNull(instance, "instance");

            Type controllerActivatorType = typeof(TControllerActivator);

            instance.Register(typeof(TController1), controllerActivatorType);
            instance.Register(typeof(TController2), controllerActivatorType);
            instance.Register(typeof(TController3), controllerActivatorType);
            instance.Register(typeof(TController4), controllerActivatorType);

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TControllerActivator">The type of the action invoker.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="typeCatalog">The type catalog.</param>
        /// <returns></returns>
        public static TypeMappingRegistry<Controller, IControllerActivator> Register<TControllerActivator>(this TypeMappingRegistry<Controller, IControllerActivator> instance, TypeCatalog typeCatalog) where TControllerActivator : IControllerActivator
        {
            Invariant.IsNotNull(instance, "instance");
            Invariant.IsNotNull(typeCatalog, "typeCatalog");

            IList<Type> controllerTypes = typeCatalog.ToList();

            EnsureControllerTypes(controllerTypes);

            Type controllerActivatorType = typeof(TControllerActivator);

            foreach (Type controllerType in controllerTypes)
            {
                instance.Register(controllerType, controllerActivatorType);
            }

            return instance;
        }

        private static void EnsureControllerTypes(IEnumerable<Type> typeCatalog)
        {
            foreach (Type type in typeCatalog.Where(controllerType => !KnownTypes.ControllerType.IsAssignableFrom(controllerType)))
            {
                throw new ArgumentException(string.Format(Culture.CurrentUI, ExceptionMessages.IsNotATargetType, type.FullName, KnownTypes.ControllerType.FullName));
            }
        }
    }
}