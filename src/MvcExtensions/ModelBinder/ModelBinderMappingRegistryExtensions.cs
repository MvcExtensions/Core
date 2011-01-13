#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Defines an static class which contains extension methods of <see cref="TypeMappingRegistry{T, IModelBinder}"/>.
    /// </summary>
    public static class ModelBinderMappingRegistryExtensions
    {
        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TModelBinder">The type of the model binder.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static TypeMappingRegistry<object, IModelBinder> Register<TModel, TModelBinder>(this TypeMappingRegistry<object, IModelBinder> instance)
            where TModelBinder : IModelBinder
        {
            Invariant.IsNotNull(instance, "instance");

            instance.Register(typeof(TModel), typeof(TModelBinder));

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TModel1">The type of the model1.</typeparam>
        /// <typeparam name="TModel2">The type of the model2.</typeparam>
        /// <typeparam name="TModelBinder">The type of the model binder.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static TypeMappingRegistry<object, IModelBinder> Register<TModel1, TModel2, TModelBinder>(this TypeMappingRegistry<object, IModelBinder> instance)
            where TModelBinder : IModelBinder
        {
            Invariant.IsNotNull(instance, "instance");

            Type modelBinderType = typeof(TModelBinder);

            instance.Register(typeof(TModel1), modelBinderType);
            instance.Register(typeof(TModel2), modelBinderType);

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TModel1">The type of the model1.</typeparam>
        /// <typeparam name="TModel2">The type of the model2.</typeparam>
        /// <typeparam name="TModel3">The type of the model3.</typeparam>
        /// <typeparam name="TModelBinder">The type of the model binder.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static TypeMappingRegistry<object, IModelBinder> Register<TModel1, TModel2, TModel3, TModelBinder>(this TypeMappingRegistry<object, IModelBinder> instance)
            where TModelBinder : IModelBinder
        {
            Invariant.IsNotNull(instance, "instance");

            Type modelBinderType = typeof(TModelBinder);

            instance.Register(typeof(TModel1), modelBinderType);
            instance.Register(typeof(TModel2), modelBinderType);
            instance.Register(typeof(TModel3), modelBinderType);

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TModel1">The type of the model1.</typeparam>
        /// <typeparam name="TModel2">The type of the model2.</typeparam>
        /// <typeparam name="TModel3">The type of the model3.</typeparam>
        /// <typeparam name="TModel4">The type of the model4.</typeparam>
        /// <typeparam name="TModelBinder">The type of the model binder.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static TypeMappingRegistry<object, IModelBinder> Register<TModel1, TModel2, TModel3, TModel4, TModelBinder>(this TypeMappingRegistry<object, IModelBinder> instance)
            where TModelBinder : IModelBinder
        {
            Invariant.IsNotNull(instance, "instance");

            Type modelBinderType = typeof(TModelBinder);

            instance.Register(typeof(TModel1), modelBinderType);
            instance.Register(typeof(TModel2), modelBinderType);
            instance.Register(typeof(TModel3), modelBinderType);
            instance.Register(typeof(TModel4), modelBinderType);

            return instance;
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TModelBinder">The type of the action invoker.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="typeCatalog">The type catalog.</param>
        /// <returns></returns>
        public static TypeMappingRegistry<object, IModelBinder> Register<TModelBinder>(this TypeMappingRegistry<object, IModelBinder> instance, TypeCatalog typeCatalog) where TModelBinder : IModelBinder
        {
            Invariant.IsNotNull(instance, "instance");
            Invariant.IsNotNull(typeCatalog, "typeCatalog");

            Type modelBinderType = typeof(TModelBinder);

            foreach (Type modelType in typeCatalog.ToList())
            {
                instance.Register(modelType, modelBinderType);
            }

            return instance;
        }
    }
}