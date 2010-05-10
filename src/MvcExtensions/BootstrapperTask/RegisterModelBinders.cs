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
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Defines a class which is used to register available <seealso cref="IModelBinder"/> that is decorated with <seealso cref="BindingTypeAttribute"/>.
    /// </summary>
    public class RegisterModelBinders : BootstrapperTask
    {
        private static readonly IList<Type> ignoredTypes = new List<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterModelBinders"/> class.
        /// </summary>
        /// <param name="binders">The binders.</param>
        public RegisterModelBinders(ModelBinderDictionary binders)
        {
            Invariant.IsNotNull(binders, "binders");

            Binders = binders;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RegisterModelBinders"/> should be excluded.
        /// </summary>
        /// <value><c>true</c> if excluded; otherwise, <c>false</c>.</value>
        public static bool Excluded
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the ignored model binder types.
        /// </summary>
        /// <value>The ignored types.</value>
        public static ICollection<Type> IgnoredTypes
        {
            [DebuggerStepThrough]
            get
            {
                return ignoredTypes;
            }
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
        /// <param name="serviceLocator">The service locator.</param>
        /// <returns></returns>
        protected override TaskContinuation ExecuteCore(IServiceLocator serviceLocator)
        {
            if (!Excluded)
            {
                IServiceRegistrar serviceRegistrar = serviceLocator as IServiceRegistrar;

                if (serviceRegistrar != null)
                {
                    Func<Type, bool> filter = type => KnownTypes.ModelBinderType.IsAssignableFrom(type) &&
                                                      type.IsDefined(KnownTypes.BindingAttributeType, true) &&
                                                      !IgnoredTypes.Any(ignoredType => ignoredType == type);

                    serviceLocator.GetInstance<IBuildManager>().ConcreteTypes
                                  .Where(filter)
                                  .Each(type => serviceRegistrar.RegisterAsSingleton(KnownTypes.ModelBinderType, type));

                    IBuildManager buildManager = serviceLocator.GetInstance<IBuildManager>();

                    serviceLocator.GetAllInstances<IModelBinder>()
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

                                          if (!modelTypes.Contains(typeInfo.ModelType) && !typeInfo.ModelType.IsAbstract && typeInfo.ModelType.IsClass)
                                          {
                                              modelTypes.Add(typeInfo.ModelType);
                                          }

                                          modelTypes.Each(modelType =>
                                          {
                                              if (Binders.ContainsKey(modelType))
                                              {
                                                  throw new InvalidOperationException(string.Format(Culture.Current, ExceptionMessages.CannotHaveMoreThanOneModelBinderForTheSameModelType, pair.Binder.GetType().FullName, modelType.FullName));
                                              }

                                              Binders.Add(modelType, pair.Binder);
                                          });
                                      }
                                  }));
                }
            }

            return TaskContinuation.Continue;
        }
    }
}