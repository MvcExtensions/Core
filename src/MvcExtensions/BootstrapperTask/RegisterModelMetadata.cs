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

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Defines a class which is used to register the default <seealso cref="ModelMetadataProvider"/>.
    /// </summary>
    public class RegisterModelMetadata : BootstrapperTask
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RegisterModelMetadata"/> should be excluded.
        /// </summary>
        /// <value><c>true</c> if excluded; otherwise, <c>false</c>.</value>
        public static bool Excluded
        {
            get;
            set;
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
                #if (!MVC1)

                IServiceRegistrar serviceRegistrar = serviceLocator as IServiceRegistrar;

                if (serviceRegistrar != null)
                {
                    serviceRegistrar.RegisterAsSingleton<CompositeModelMetadataProvider>()
                                    .RegisterAsSingleton<IModelMetadataRegistry, ModelMetadataRegistry>();

                    IEnumerable<Type> concreteTypes = serviceLocator.GetInstance<IBuildManager>().ConcreteTypes;

                    concreteTypes.Where(type => KnownTypes.ModelMetadataConfigurationType.IsAssignableFrom(type))
                                 .Each(type => serviceRegistrar.RegisterAsTransient(KnownTypes.ModelMetadataConfigurationType, type));

                    concreteTypes.Where(type => KnownTypes.ExtendedModelMetadataProviderType.IsAssignableFrom(type))
                                 .Each(type => serviceRegistrar.RegisterAsSingleton(KnownTypes.ExtendedModelMetadataProviderType, type));

                    IEnumerable<IModelMetadataConfiguration> configurations = serviceLocator.GetAllInstances<IModelMetadataConfiguration>();

                    IModelMetadataRegistry registry = serviceLocator.GetInstance<IModelMetadataRegistry>();

                    configurations.Each(configuration => registry.Register(configuration.ModelType, configuration.Configurations));

                    ModelMetadataProviders.Current = serviceLocator.GetInstance<CompositeModelMetadataProvider>();

                    IList<ModelValidatorProvider> validatorProviders = new List<ModelValidatorProvider>(ModelValidatorProviders.Providers);
                    validatorProviders.Insert(0, new ExtendedModelValidatorProvider());
                    CompositeModelValidatorProvider compositeModelValidatorProvider = new CompositeModelValidatorProvider(validatorProviders.ToArray());

                    serviceRegistrar.RegisterInstance<ModelValidatorProvider>(compositeModelValidatorProvider);
                    ModelValidatorProviders.Providers.Clear();
                    ModelValidatorProviders.Providers.Add(compositeModelValidatorProvider);
                }

                #endif
            }

            return TaskContinuation.Continue;
        }
    }
}