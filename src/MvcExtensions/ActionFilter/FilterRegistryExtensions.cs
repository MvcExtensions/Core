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
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines an static class which contains extension methods of <see cref="IFilterRegistry"/>.
    /// </summary>
    public static class FilterRegistryExtensions
    {
        private static readonly Type genericControllerItemType = typeof(FilterRegistryControllerItem<>);

        /// <summary>
        /// Registers the specified filter for the given controller types.
        /// </summary>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="typeCatalog">The controller types.</param>
        /// <returns></returns>
        [NotNull]
        public static IFilterRegistry Register<TFilter>([NotNull] this IFilterRegistry instance, [NotNull] TypeCatalog typeCatalog)
            where TFilter : IMvcFilter
        {
            return Register<TFilter>(instance, typeCatalog, filter => { });
        }

        /// <summary>
        /// Registers and configures the specified filter for the given controller types.
        /// </summary>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="typeCatalog">The controller types.</param>
        /// <param name="configureFilter">The configure filter action.</param>
        /// <returns></returns>
        [NotNull]
        public static IFilterRegistry Register<TFilter>([NotNull] this IFilterRegistry instance, [NotNull] TypeCatalog typeCatalog, [NotNull] Action<TFilter> configureFilter)
            where TFilter : IMvcFilter
        {
            Invariant.IsNotNull(instance, "instance");
            Invariant.IsNotNull(typeCatalog, "typeCatalog");
            Invariant.IsNotNull(configureFilter, "configureFilter");

            IList<Type> controllerTypeList = typeCatalog.ToList();

            EnsureControllerTypes(controllerTypeList);

            foreach (Type itemType in controllerTypeList.Select(controllerType => genericControllerItemType.MakeGenericType(controllerType)))
            {
                instance.Items.Add(Activator.CreateInstance(itemType, new object[] { CreateAndConfigureFilterFactory(instance, configureFilter) }) as FilterRegistryItem);
            }

            return instance;
        }

        /// <summary>
        /// Registers the specified filters for the given controller types.
        /// </summary>
        /// <typeparam name="TFilter1">The type of the filter1.</typeparam>
        /// <typeparam name="TFilter2">The type of the filter2.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="typeCatalog">The controller types.</param>
        /// <returns></returns>
        [NotNull]
        public static IFilterRegistry Register<TFilter1, TFilter2>([NotNull] this IFilterRegistry instance, [NotNull] TypeCatalog typeCatalog)
            where TFilter1 : IMvcFilter
            where TFilter2 : IMvcFilter
        {
            Invariant.IsNotNull(instance, "instance");
            Invariant.IsNotNull(typeCatalog, "typeCatalog");

            return Register(instance, typeCatalog, typeof(TFilter1), typeof(TFilter2));
        }

        /// <summary>
        /// Registers the specified filters for the given controller types.
        /// </summary>
        /// <typeparam name="TFilter1">The type of the filter1.</typeparam>
        /// <typeparam name="TFilter2">The type of the filter2.</typeparam>
        /// <typeparam name="TFilter3">The type of the filter3.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="typeCatalog">The controller types.</param>
        /// <returns></returns>
        [NotNull]
        public static IFilterRegistry Register<TFilter1, TFilter2, TFilter3>([NotNull] this IFilterRegistry instance, [NotNull] TypeCatalog typeCatalog)
            where TFilter1 : IMvcFilter
            where TFilter2 : IMvcFilter
            where TFilter3 : IMvcFilter
        {
            Invariant.IsNotNull(instance, "instance");
            Invariant.IsNotNull(typeCatalog, "typeCatalog");

            return Register(instance, typeCatalog, typeof(TFilter1), typeof(TFilter2), typeof(TFilter3));
        }

        /// <summary>
        /// Registers the specified filters for the given controller types.
        /// </summary>
        /// <typeparam name="TFilter1">The type of the filter1.</typeparam>
        /// <typeparam name="TFilter2">The type of the filter2.</typeparam>
        /// <typeparam name="TFilter3">The type of the filter3.</typeparam>
        /// <typeparam name="TFilter4">The type of the filter4.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="typeCatalog">The controller types.</param>
        /// <returns></returns>
        [NotNull]
        public static IFilterRegistry Register<TFilter1, TFilter2, TFilter3, TFilter4>([NotNull] this IFilterRegistry instance, [NotNull] TypeCatalog typeCatalog)
            where TFilter1 : IMvcFilter
            where TFilter2 : IMvcFilter
            where TFilter3 : IMvcFilter
            where TFilter4 : IMvcFilter
        {
            Invariant.IsNotNull(instance, "instance");
            Invariant.IsNotNull(typeCatalog, "typeCatalog");

            return Register(instance, typeCatalog, typeof(TFilter1), typeof(TFilter2), typeof(TFilter3), typeof(TFilter4));
        }

        /// <summary>
        /// Registers the specified filter for the given controller.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [NotNull]
        public static IFilterRegistry Register<TController, TFilter>([NotNull] this IFilterRegistry instance)
            where TController : Controller
            where TFilter : IMvcFilter
        {
            return Register<TController, TFilter>(instance, (TFilter filter) => { });
        }

        /// <summary>
        /// Registers and configures the specified filter for the given controller.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="configureFilter">The configure filter.</param>
        /// <returns></returns>
        [NotNull]
        public static IFilterRegistry Register<TController, TFilter>([NotNull] this IFilterRegistry instance, [NotNull] Action<TFilter> configureFilter)
            where TController : Controller
            where TFilter : IMvcFilter
        {
            Invariant.IsNotNull(instance, "instance");
            Invariant.IsNotNull(configureFilter, "configureFilter");

            instance.Register<TController, IMvcFilter>(CreateAndConfigureFilterFactory(instance, configureFilter));

            return instance;
        }

        /// <summary>
        /// Registers the specified filters for the given controller.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TFilter1">The type of the filter1.</typeparam>
        /// <typeparam name="TFilter2">The type of the filter2.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IFilterRegistry Register<TController, TFilter1, TFilter2>([NotNull] this IFilterRegistry instance)
            where TController : Controller
            where TFilter1 : IMvcFilter
            where TFilter2 : IMvcFilter
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.Register<TController, IMvcFilter>(CreateFilterFactories(instance, typeof(TFilter1), typeof(TFilter2)).ToArray());
        }

        /// <summary>
        /// Registers the specified filters for the given controller.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TFilter1">The type of the filter1.</typeparam>
        /// <typeparam name="TFilter2">The type of the filter2.</typeparam>
        /// <typeparam name="TFilter3">The type of the filter3.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IFilterRegistry Register<TController, TFilter1, TFilter2, TFilter3>([NotNull] this IFilterRegistry instance)
            where TController : Controller
            where TFilter1 : IMvcFilter
            where TFilter2 : IMvcFilter
            where TFilter3 : IMvcFilter
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.Register<TController, IMvcFilter>(CreateFilterFactories(instance, typeof(TFilter1), typeof(TFilter2), typeof(TFilter3)).ToArray());
        }

        /// <summary>
        /// Registers the specified filters for the given controller.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TFilter1">The type of the filter1.</typeparam>
        /// <typeparam name="TFilter2">The type of the filter2.</typeparam>
        /// <typeparam name="TFilter3">The type of the filter3.</typeparam>
        /// <typeparam name="TFilter4">The type of the filter4.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IFilterRegistry Register<TController, TFilter1, TFilter2, TFilter3, TFilter4>([NotNull] this IFilterRegistry instance)
            where TController : Controller
            where TFilter1 : IMvcFilter
            where TFilter2 : IMvcFilter
            where TFilter3 : IMvcFilter
            where TFilter4 : IMvcFilter
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.Register<TController, IMvcFilter>(CreateFilterFactories(instance, typeof(TFilter1), typeof(TFilter2), typeof(TFilter3), typeof(TFilter4)).ToArray());
        }

        /// <summary>
        /// Registers the specified filter for the given controller action.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="action">The controller action method.</param>
        /// <returns></returns>
        public static IFilterRegistry Register<TController, TFilter>([NotNull] this IFilterRegistry instance, [NotNull] Expression<Action<TController>> action)
            where TController : Controller
            where TFilter : IMvcFilter
        {
            Invariant.IsNotNull(instance, "instance");

            return Register<TController, TFilter>(instance, action, filter => { });
        }

        /// <summary>
        /// Registers and configures the specified filter for the given controller action.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="action">The controller action method.</param>
        /// <param name="configureFilter">The configure filter.</param>
        /// <returns></returns>
        public static IFilterRegistry Register<TController, TFilter>([NotNull] this IFilterRegistry instance, [NotNull] Expression<Action<TController>> action, [NotNull] Action<TFilter> configureFilter)
            where TController : Controller
            where TFilter : IMvcFilter
        {
            Invariant.IsNotNull(instance, "instance");
            Invariant.IsNotNull(action, "action");
            Invariant.IsNotNull(configureFilter, "configureFilter");

            return instance.Register(action, CreateAndConfigureFilterFactory(instance, configureFilter));
        }

        /// <summary>
        /// Registers the specified filters for the given controller action.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TFilter1">The type of the filter1.</typeparam>
        /// <typeparam name="TFilter2">The type of the filter2.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="action">The controller action method.</param>
        /// <returns></returns>
        public static IFilterRegistry Register<TController, TFilter1, TFilter2>([NotNull] this IFilterRegistry instance, Expression<Action<TController>> action)
            where TController : Controller
            where TFilter1 : IMvcFilter
            where TFilter2 : IMvcFilter
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.Register(action, CreateFilterFactories(instance, typeof(TFilter1), typeof(TFilter2)).ToArray());
        }

        /// <summary>
        /// Registers the specified filters for the given controller action.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TFilter1">The type of the filter1.</typeparam>
        /// <typeparam name="TFilter2">The type of the filter2.</typeparam>
        /// <typeparam name="TFilter3">The type of the filter3.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="action">The controller action method.</param>
        /// <returns></returns>
        public static IFilterRegistry Register<TController, TFilter1, TFilter2, TFilter3>([NotNull] this IFilterRegistry instance, Expression<Action<TController>> action)
            where TController : Controller
            where TFilter1 : IMvcFilter
            where TFilter2 : IMvcFilter
            where TFilter3 : IMvcFilter
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.Register(action, CreateFilterFactories(instance, typeof(TFilter1), typeof(TFilter2), typeof(TFilter3)).ToArray());
        }

        /// <summary>
        /// Registers the specified filters for the given controller action.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TFilter1">The type of the filter1.</typeparam>
        /// <typeparam name="TFilter2">The type of the filter2.</typeparam>
        /// <typeparam name="TFilter3">The type of the filter3.</typeparam>
        /// <typeparam name="TFilter4">The type of the filter4.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="action">The controller action method.</param>
        /// <returns></returns>
        public static IFilterRegistry Register<TController, TFilter1, TFilter2, TFilter3, TFilter4>([NotNull] this IFilterRegistry instance, Expression<Action<TController>> action)
            where TController : Controller
            where TFilter1 : IMvcFilter
            where TFilter2 : IMvcFilter
            where TFilter3 : IMvcFilter
            where TFilter4 : IMvcFilter
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.Register(action, CreateFilterFactories(instance, typeof(TFilter1), typeof(TFilter2), typeof(TFilter3), typeof(TFilter4)).ToArray());
        }

        [NotNull]
        private static IFilterRegistry Register([NotNull] IFilterRegistry instance, [NotNull] IEnumerable<Type> typeCatalog, [NotNull] params Type[] filterTypes)
        {
            IList<Type> controllerTypes = typeCatalog.ToList();

            EnsureControllerTypes(controllerTypes);

            foreach (Type itemType in controllerTypes.Select(controllerType => genericControllerItemType.MakeGenericType(controllerType)))
            {
                instance.Items.Add(Activator.CreateInstance(itemType, new object[] { CreateFilterFactories(instance, filterTypes) }) as FilterRegistryItem);
            }

            return instance;
        }

        private static void EnsureControllerTypes([NotNull] IEnumerable<Type> typeCatalog)
        {
            foreach (Type controllerType in typeCatalog.Where(controllerType => !KnownTypes.ControllerType.IsAssignableFrom(controllerType)))
            {
                throw new ArgumentException(string.Format(Culture.CurrentUI, ExceptionMessages.IsNotATargetType, controllerType.FullName, KnownTypes.ControllerType.FullName));
            }
        }

        [NotNull]
        private static IEnumerable<Func<IMvcFilter>> CreateFilterFactories([NotNull] IFilterRegistry registry, [NotNull] params Type[] filterTypes)
        {
            return filterTypes.Select(filterType => new Func<IMvcFilter>(() => registry.Container.GetService(filterType) as IMvcFilter)).ToArray();
        }

        [NotNull]
        private static IEnumerable<Func<IMvcFilter>> CreateAndConfigureFilterFactory<TFilter>([NotNull] IFilterRegistry registry, [NotNull] Action<TFilter> configureFilter) where TFilter : IMvcFilter
        {
            return new List<Func<IMvcFilter>>
                       {
                           () =>
                           {
                               TFilter filter = registry.Container.GetService<TFilter>();

                               configureFilter(filter);

                               return filter;
                           }
                       };
        }
    }
}