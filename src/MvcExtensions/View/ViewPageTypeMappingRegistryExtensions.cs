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
    /// Defines an static class which contains extension methods of <see cref="TypeMappingRegistry{IView, IViewActivator}"/>.
    /// </summary>
    public static class ViewPageTypeMappingRegistryExtensions
    {
        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TView">The type of the view.</typeparam>
        /// <typeparam name="TViewPageActivator">The type of the view page activator.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static TypeMappingRegistry<IView, IViewPageActivator> Register<TView, TViewPageActivator>(this TypeMappingRegistry<IView, IViewPageActivator> instance)
            where TView : IView
            where TViewPageActivator : IViewPageActivator
        {
            Invariant.IsNotNull(instance, "instance");

            instance.Register(typeof(TView), typeof(TViewPageActivator));

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TView1">The type of the view1.</typeparam>
        /// <typeparam name="TView2">The type of the view2.</typeparam>
        /// <typeparam name="TViewPageActivator">The type of the view page activator.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static TypeMappingRegistry<IView, IViewPageActivator> Register<TView1, TView2, TViewPageActivator>(this TypeMappingRegistry<IView, IViewPageActivator> instance)
            where TView1 : IView
            where TView2 : IView
            where TViewPageActivator : IViewPageActivator
        {
            Invariant.IsNotNull(instance, "instance");

            Type viewPageActivatorType = typeof(TViewPageActivator);

            instance.Register(typeof(TView1), viewPageActivatorType);
            instance.Register(typeof(TView2), viewPageActivatorType);

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TView1">The type of the view1.</typeparam>
        /// <typeparam name="TView2">The type of the view2.</typeparam>
        /// <typeparam name="TView3">The type of the view3.</typeparam>
        /// <typeparam name="TViewPageActivator">The type of the view page activator.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static TypeMappingRegistry<IView, IViewPageActivator> Register<TView1, TView2, TView3, TViewPageActivator>(this TypeMappingRegistry<IView, IViewPageActivator> instance)
            where TView1 : IView
            where TView2 : IView
            where TView3 : IView
            where TViewPageActivator : IViewPageActivator
        {
            Invariant.IsNotNull(instance, "instance");

            Type viewPageActivatorType = typeof(TViewPageActivator);

            instance.Register(typeof(TView1), viewPageActivatorType);
            instance.Register(typeof(TView2), viewPageActivatorType);
            instance.Register(typeof(TView3), viewPageActivatorType);

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TView1">The type of the controller1.</typeparam>
        /// <typeparam name="TView2">The type of the controller2.</typeparam>
        /// <typeparam name="TView3">The type of the controller3.</typeparam>
        /// <typeparam name="TVIew4">The type of the controller4.</typeparam>
        /// <typeparam name="TViewPageActivator">The type of the controller activator.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static TypeMappingRegistry<IView, IViewPageActivator> Register<TView1, TView2, TView3, TVIew4, TViewPageActivator>(this TypeMappingRegistry<IView, IViewPageActivator> instance)
            where TView1 : IView
            where TView2 : IView
            where TView3 : IView
            where TVIew4 : IView
            where TViewPageActivator : IViewPageActivator
        {
            Invariant.IsNotNull(instance, "instance");

            Type viewPageActivatorType = typeof(TViewPageActivator);

            instance.Register(typeof(TView1), viewPageActivatorType);
            instance.Register(typeof(TView2), viewPageActivatorType);
            instance.Register(typeof(TView3), viewPageActivatorType);
            instance.Register(typeof(TVIew4), viewPageActivatorType);

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TViewPageActivator">The type of the view page activator.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="typeCatalog">The type catalog.</param>
        /// <returns></returns>
        public static TypeMappingRegistry<IView, IViewPageActivator> Register<TViewPageActivator>(this TypeMappingRegistry<IView, IViewPageActivator> instance, TypeCatalog typeCatalog)
        {
            Invariant.IsNotNull(instance, "instance");
            Invariant.IsNotNull(typeCatalog, "typeCatalog");

            IList<Type> viewTypes = typeCatalog.ToList();
            EnsureViewTypes(viewTypes);

            Type viewPageActivatorType = typeof(TViewPageActivator);

            foreach (Type viewType in viewTypes)
            {
                instance.Register(viewType, viewPageActivatorType);
            }

            return instance;
        }

        private static void EnsureViewTypes(IEnumerable<Type> typeCatalog)
        {
            foreach (Type type in typeCatalog.Where(viewType => !KnownTypes.ViewType.IsAssignableFrom(viewType)))
            {
                throw new ArgumentException(string.Format(Culture.CurrentUI, ExceptionMessages.IsNotATargetType, type.FullName, KnownTypes.ViewType.FullName));
            }
        }
    }
}