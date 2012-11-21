#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
#if !MVC_3
    using System.Web.Http;
#endif
    using System.Web.Mvc;
#if !MVC_3
    using MvcExtensions.WebApi;
    using DataAnnotationsModelValidatorProvider = System.Web.Http.Validation.Providers.DataAnnotationsModelValidatorProvider;
    using ModelMetadataProvider = System.Web.Http.Metadata.ModelMetadataProvider;
#endif

    /// <summary>
    /// Provides a way to register metadata provider and model metadata configuration classes
    /// </summary>
    public sealed class ModelMetadataRegistrar : IRegistrar, IMetadataConstruction
    {
        private Func<IEnumerable<IModelMetadataConfiguration>> getConfigurations;

        /// <summary>
        /// Creates <see cref="ModelMetadataRegistrar"/> instance
        /// </summary>
        /// <param name="registry">Holds all configations data</param>
        public ModelMetadataRegistrar(IModelMetadataRegistry registry)
        {
            Registry = registry;
        }
        
        /// <summary>
        /// Holds all configations data
        /// </summary>
        public IModelMetadataRegistry Registry { get; set; }

        /// <summary>
        /// Shows whether configuration factory is defined
        /// </summary>
        internal bool ConfigurationFactoryDefined
        {
            get
            {
                return getConfigurations != null;
            }
        }

        /// <summary>
        /// Allows to define custom factory to contruct model metadata configuration classes
        /// </summary>
        /// <param name="configurationFactory">A factory to instantiate <see cref="IModelMetadataConfiguration"/> classes</param>
        /// <returns>Fluent</returns>
        public IRegistrar ConstructMetadataUsing(Func<IEnumerable<IModelMetadataConfiguration>> configurationFactory)
        {
            Invariant.IsNotNull(configurationFactory, "configurationFactory");

            getConfigurations = configurationFactory;
            return this;
        }

        /// <summary>
        /// Registers metadata provider and model metadata configuration classes
        /// </summary>
        public void Register()
        {
            RegisterMetadataConfigurations();
            RegisterMvcProviders();
#if !MVC_3
            RegisterWebApiProviders(GlobalConfiguration.Configuration);
#endif
        }

        private void RegisterMetadataConfigurations()
        {
            var configurations = getConfigurations != null ? getConfigurations().Where(t => t != null) : Enumerable.Empty<IModelMetadataConfiguration>();

            foreach (var configuration in configurations)
            {
                Registry.RegisterModelProperties(configuration.ModelType, configuration.Configurations);
            }
        }

        private void RegisterMvcProviders()
        {
            var validatorProviders = new List<ModelValidatorProvider>(ModelValidatorProviders.Providers);
            validatorProviders.Insert(0, new ExtendedModelValidatorProvider());
            var compositeModelValidatorProvider = new CompositeModelValidatorProvider(validatorProviders.ToArray());

            ModelMetadataProviders.Current = new ExtendedModelMetadataProvider(Registry);
            ModelValidatorProviders.Providers.Clear();
            ModelValidatorProviders.Providers.Add(compositeModelValidatorProvider);
        }

#if !MVC_3
        private void RegisterWebApiProviders(HttpConfiguration config)
        {
            var providers = config.Services.GetModelValidatorProviders().OfType<DataAnnotationsModelValidatorProvider>().ToArray();

            // remove dataannotation providers from list
            config.Services.RemoveAll(typeof(System.Web.Http.Validation.ModelValidatorProvider), o => o is DataAnnotationsModelValidatorProvider);

            // add WebApiModelValidationProvider and DataAnnotationsModelValidatorProviders to composite validator
            var list = new List<System.Web.Http.Validation.ModelValidatorProvider>(providers);
            list.Insert(0, new WebApiModelValidationProvider());
            config.Services.Insert(typeof(System.Web.Http.Validation.ModelValidatorProvider), 0, new WebApiCompositeModelValidatorProvider(list.ToArray()));

            // register MvcExtensions metadata provider
            var metadataProvider = config.Services.GetModelMetadataProvider();
            config.Services.Replace(typeof(ModelMetadataProvider), new WebApi.ExtendedModelMetadataProvider(Registry, metadataProvider));
        }
#endif

    }
}
