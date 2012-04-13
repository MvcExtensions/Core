#region Copyright
// Copyright (c) 2009 - 2011, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
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
    /// Defines a class which is used to register the default <seealso cref="ModelMetadataProvider"/>.
    /// </summary>
    public class ModelMetadataRegistrator
    {
        private static readonly object locker = new object();
        private static volatile ModelMetadataRegistrator current;
        private readonly IBuildManager buildManager;

        /// <summary>
        /// Ctor
        /// </summary>
        public ModelMetadataRegistrator(IBuildManager buildManager)
        {
            this.buildManager = buildManager;
        }

        /// <summary>
        /// Singleton instance to simplify provider registration 
        /// without using bootstraping features of MvcExtensions
        /// </summary>
        public static ModelMetadataRegistrator Current
        {
            get
            {
                if (current == null)
                {
                    lock (locker)
                    {
                        if (current == null)
                        {
                            current = new ModelMetadataRegistrator(new BuildManagerWrapper());
                        }
                    }
                }
                return current;
            }
        }

        private bool MetadataRegistered
        {
            get;
            set;
        }

        /// <summary>
        /// Register all ModelMetadata types with IoC container
        /// </summary>
        /// <param name="registerTransient">Needs to register component as transient via IoC: first type is service type, second one is implementation</param>
        /// <param name="registerAsSingleton">Needs to register component as singleton via IoC: first type is service type, second one is implementation</param>
        public void RegisterMetadataTypes(Action<Type, Type> registerTransient, Action<Type, Type> registerAsSingleton)
        {
            if (MetadataRegistered)
            {
                throw new InvalidOperationException("Metadata types have been already registered.");
            }
            IEnumerable<Type> concreteTypes = buildManager.ConcreteTypes;
            concreteTypes
                .Where(type => KnownTypes.ModelMetadataConfigurationType.IsAssignableFrom(type))
                .Each(type => registerTransient(KnownTypes.ModelMetadataConfigurationType, type));

            registerAsSingleton(typeof(IModelMetadataRegistry), typeof(ModelMetadataRegistry));
            MetadataRegistered = true;
        }

        /// <summary>
        /// Registers metadata provider
        /// </summary>
        /// <returns></returns>
        public void RegisterMetadataProviders(IDependencyResolver dependencyResolver)
        {
            if (!MetadataRegistered)
                throw new ApplicationException("Register metadata types before register providers.");

            IEnumerable<IModelMetadataConfiguration> configurations = dependencyResolver.GetServices<IModelMetadataConfiguration>();

            var registry = dependencyResolver.GetService<IModelMetadataRegistry>();

            configurations.Each(configuration => registry.RegisterModelProperties(configuration.ModelType, configuration.Configurations));

            IList<ModelValidatorProvider> validatorProviders = new List<ModelValidatorProvider>(ModelValidatorProviders.Providers);
            validatorProviders.Insert(0, new ExtendedModelValidatorProvider());
            var compositeModelValidatorProvider = new CompositeModelValidatorProvider(validatorProviders.ToArray());

            ModelMetadataProviders.Current = new ExtendedModelMetadataProvider(registry);
            ModelValidatorProviders.Providers.Clear();
            ModelValidatorProviders.Providers.Add(compositeModelValidatorProvider);

            // clean up current instance to release build manager (we need it only for app starting stage)
            current = null;
        }
    }
}