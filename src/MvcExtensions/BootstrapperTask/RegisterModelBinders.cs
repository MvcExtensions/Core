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
    /// Defines a class which is used to register available <seealso cref="IModelBinder"/> that is decorated with <seealso cref="BindingTypeAttribute"/>.
    /// </summary>
    public class RegisterModelBinders : IgnorableTypesBootstrapperTask<RegisterModelBinders, IModelBinder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterModelBinders"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="binders">The binders.</param>
        public RegisterModelBinders(ContainerAdapter container, ModelBinderDictionary binders)
        {
            Invariant.IsNotNull(container, "container");
            Invariant.IsNotNull(binders, "binders");

            Container = container;
            Binders = binders;
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        protected ContainerAdapter Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the binders.
        /// </summary>
        /// <value>The binders.</value>
        protected ModelBinderDictionary Binders
        {
            get;
            private set;
        }

        /// <summary>
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            Func<Type, bool> filter = type => KnownTypes.ModelBinderType.IsAssignableFrom(type) &&
                                              type.IsDefined(KnownTypes.BindingAttributeType, true) &&
                                              !IgnoredTypes.Any(ignoredType => ignoredType == type);

            Container.GetService<IBuildManager>()
                     .ConcreteTypes
                     .Where(filter)
                     .Each(type => Container.RegisterAsSingleton(KnownTypes.ModelBinderType, type));

            IBuildManager buildManager = Container.GetService<IBuildManager>();

            Container.GetServices<IModelBinder>()
                     .Select(binder => new
                                            {
                                                Binder = binder,
                                                Types = binder.GetType()
                                                              .GetCustomAttributes(KnownTypes.BindingAttributeType, true)
                                                              .OfType<BindingTypeAttribute>()
                                                              .Select(attribute => new { attribute.ModelType, attribute.Inherited })
                                                              .ToList()
                                            })
                     .Each(pair => pair.Types.Each(typeInfo =>
                                                        {
                                                            if (pair.Binder != null)
                                                            {
                                                                IList<Type> modelTypes = new List<Type>();

                                                                if (typeInfo.Inherited)
                                                                {
                                                                    modelTypes = buildManager.ConcreteTypes.Where(type => typeInfo.ModelType.IsAssignableFrom(type)).ToList();
                                                                }

                                                                if (!modelTypes.Contains(typeInfo.ModelType) && (!typeInfo.ModelType.IsInterface && !typeInfo.ModelType.IsAbstract))
                                                                {
                                                                    modelTypes.Add(typeInfo.ModelType);
                                                                }

                                                                modelTypes.Each(modelType =>
                                                                                    {
                                                                                        if (Binders.ContainsKey(modelType))
                                                                                        {
                                                                                            throw new InvalidOperationException(string.Format(Culture.CurrentUI, ExceptionMessages.CannotHaveMoreThanOneModelBinderForTheSameModelType, pair.Binder.GetType().FullName, modelType.FullName));
                                                                                        }

                                                                                        Binders.Add(modelType, pair.Binder);
                                                                                    });
                                                            }
                                                        }));

            return TaskContinuation.Continue;
        }
    }
}