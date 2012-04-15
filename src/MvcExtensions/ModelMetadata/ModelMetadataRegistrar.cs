#region Copyright
// Copyright (c) 2009 - 2011, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class which is used to register the default <seealso cref="ModelMetadataProvider"/>.
    /// </summary>
    public class ModelMetadataRegistrar : IModelMetadataRegistrar
    {
        private readonly IDependencyResolver dependencyResolver;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="dependencyResolver"></param>
        public ModelMetadataRegistrar(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        /// <summary>
        /// Registers metadata provider
        /// </summary>
        /// <returns></returns>
        public void RegisterMetadataProviders()
        {
            IEnumerable<IModelMetadataConfiguration> configurations = dependencyResolver.GetServices<IModelMetadataConfiguration>();

            var registry = dependencyResolver.GetService<IModelMetadataRegistry>();

            configurations.Each(configuration => registry.RegisterModelProperties(configuration.ModelType, configuration.Configurations));

            IList<ModelValidatorProvider> validatorProviders = new List<ModelValidatorProvider>(ModelValidatorProviders.Providers);
            validatorProviders.Insert(0, new ExtendedModelValidatorProvider());
            var compositeModelValidatorProvider = new CompositeModelValidatorProvider(validatorProviders.ToArray());

            ModelMetadataProviders.Current = new ExtendedModelMetadataProvider(registry);
            ModelValidatorProviders.Providers.Clear();
            ModelValidatorProviders.Providers.Add(compositeModelValidatorProvider);
        }
    }
}