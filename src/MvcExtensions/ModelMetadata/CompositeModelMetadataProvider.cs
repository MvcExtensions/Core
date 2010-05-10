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

    /// <summary>
    /// Defines a class which is used to maintain multiple model metadata provider.
    /// </summary>
    public class CompositeModelMetadataProvider : ModelMetadataProvider
    {
        private ModelMetadataProvider defaultProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeModelMetadataProvider"/> class.
        /// </summary>
        /// <param name="providers">The providers.</param>
        public CompositeModelMetadataProvider(params ExtendedModelMetadataProviderBase[] providers)
        {
            Invariant.IsNotNull(providers, "providers");

            Providers = providers;
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <value>The providers.</value>
        public IEnumerable<ExtendedModelMetadataProviderBase> Providers
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the default provider.
        /// </summary>
        /// <value>The default provider.</value>
        public ModelMetadataProvider DefaultProvider
        {
            [DebuggerStepThrough]
            get
            {
                return defaultProvider ?? (defaultProvider = new DataAnnotationsModelMetadataProvider());
            }

            [DebuggerStepThrough]
            set
            {
                Invariant.IsNotNull(value, "value");
                defaultProvider = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="T:System.Web.Mvc.ModelMetadata"/> object for each property of a model.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="containerType">The type of the container.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Mvc.ModelMetadata"/> object for each property of a model.
        /// </returns>
        public override IEnumerable<ModelMetadata> GetMetadataForProperties(object container, Type containerType)
        {
            Invariant.IsNotNull(containerType, "containerType");

            ModelMetadataProvider provider = Providers.LastOrDefault(p => p.IsRegistered(containerType)) ?? DefaultProvider;

            return provider.GetMetadataForProperties(container, containerType);
        }

        /// <summary>
        /// Gets metadata for the specified property.
        /// </summary>
        /// <param name="modelAccessor">The model accessor.</param>
        /// <param name="containerType">The type of the container.</param>
        /// <param name="propertyName">The property to get the metadata model for.</param>
        /// <returns>
        /// The metadata model for the specified property.
        /// </returns>
        public override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, string propertyName)
        {
            Invariant.IsNotNull(containerType, "containerType");
            Invariant.IsNotNull(propertyName, "propertyName");

            ModelMetadataProvider provider = Providers.LastOrDefault(p => p.IsRegistered(containerType, propertyName)) ?? DefaultProvider;

            return provider.GetMetadataForProperty(modelAccessor, containerType, propertyName);
        }

        /// <summary>
        /// Gets metadata for the specified model accessor and model type.
        /// </summary>
        /// <param name="modelAccessor">The model accessor.</param>
        /// <param name="modelType">They type of the model.</param>
        /// <returns>The metadata.</returns>
        public override ModelMetadata GetMetadataForType(Func<object> modelAccessor, Type modelType)
        {
            Invariant.IsNotNull(modelType, "modelType");

            ModelMetadataProvider provider = Providers.LastOrDefault(p => p.IsRegistered(modelType)) ?? DefaultProvider;

            return provider.GetMetadataForType(modelAccessor, modelType);
        }
    }
}