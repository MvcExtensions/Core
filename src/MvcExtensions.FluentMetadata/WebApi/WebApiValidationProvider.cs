#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Web.Http.Metadata;
    using System.Web.Http.Validation;
    using System.Web.Http.Validation.Validators;

    /*
      Using 
     * 
      FluentMetadataConfiguration.Register();
      config.Services.Insert(typeof(ModelValidatorProvider), 0, new WebApiValidationProvider());
      var provider = config.Services.GetModelMetadataProvider();
      config.Services.Replace(typeof(ModelMetadataProvider), 
            new CompositeModelMetadataProvider(new ExtendedModelMetadataProvider(FluentMetadataConfiguration.Registry), provider));
  
     */

    /// <summary>
    /// 
    /// </summary>
    public class WebApiValidationProvider : ModelValidatorProvider
    {
        /// <summary>
        /// Gets a list of validators associated with this <see cref="T:System.Web.Http.Validation.ModelValidatorProvider"/>.
        /// </summary>
        /// <returns>
        /// The list of validators.
        /// </returns>
        /// <param name="metadata">The metadata.</param>
        /// <param name="validatorProviders">The validator providers.</param>
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders)
        {
            Invariant.IsNotNull(metadata, "metadata");
            Invariant.IsNotNull(validatorProviders, "validatorProviders");

            var extendedModelMetadata = metadata as ExtendedModelMetadata;

            if (extendedModelMetadata == null || extendedModelMetadata.Metadata == null)
            {
                yield break;
            }

            var attributes = extendedModelMetadata.Metadata.Validations.Select(validationMeta => validationMeta.CreateValidationAttribute());
            foreach (var attribute in attributes)
            {
                yield return new DataAnnotationsModelValidator(validatorProviders, attribute);
            }
        }
    }

    /// <summary>
    /// Defines a metadata class which supports fluent metadata registration.
    /// </summary>
    public class ExtendedModelMetadata : ModelMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedModelMetadata"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="containerType">Type of the container.</param>
        /// <param name="modelAccessor">The model accessor.</param>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="metadata">Metadata</param>
        public ExtendedModelMetadata(
            ModelMetadataProvider provider, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName, ModelMetadataItem metadata)
            : base(provider, containerType, modelAccessor, modelType, propertyName)
        {
            Metadata = metadata;
        }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        public ModelMetadataItem Metadata { get; private set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CompositeModelMetadataProvider : ModelMetadataProvider
    {
        private readonly IEnumerable<ModelMetadataProvider> metadataProviders;

        /// <summary>
        /// 
        /// </summary>
        public CompositeModelMetadataProvider(IEnumerable<ModelMetadataProvider> metadataProviders)
        {
            this.metadataProviders = metadataProviders;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadataProviders"></param>
        public CompositeModelMetadataProvider(params ModelMetadataProvider[] metadataProviders)
        {
            this.metadataProviders = metadataProviders;
        }

        /// <summary>
        /// Gets a ModelMetadata object for each property of a model.
        /// </summary>
        /// <returns>
        /// A ModelMetadata object for each property of a model.
        /// </returns>
        /// <param name="container">The container.</param><param name="containerType">The type of the container.</param>
        public override IEnumerable<ModelMetadata> GetMetadataForProperties(object container, Type containerType)
        {
            foreach (var provider in metadataProviders)
            {
                var result = provider.GetMetadataForProperties(container, containerType);
                if (result != null)
                    return result;
            }

            return Enumerable.Empty<ModelMetadata>();
        }

        /// <summary>
        /// Get metadata for the specified property.
        /// </summary>
        /// <returns>
        /// The metadata model for the specified property.
        /// </returns>
        /// <param name="modelAccessor">The model accessor.</param><param name="containerType">The type of the container.</param><param name="propertyName">The property to get the metadata model for.</param>
        public override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, string propertyName)
        {
            foreach (var provider in metadataProviders)
            {
                var result = provider.GetMetadataForProperty(modelAccessor, containerType, propertyName);
                if (result != null)
                    return result;
            }

            return new ModelMetadata(this, containerType, modelAccessor, containerType, propertyName);
        }

        /// <summary>
        /// Gets the metadata for the specified model accessor and model type.
        /// </summary>
        /// <returns>
        /// The metadata.
        /// </returns>
        /// <param name="modelAccessor">The model accessor.</param><param name="modelType">The type of the mode.</param>
        public override ModelMetadata GetMetadataForType(Func<object> modelAccessor, Type modelType)
        {
            foreach (var provider in metadataProviders)
            {
                var result = provider.GetMetadataForType(modelAccessor, modelType);
                if (result != null)
                    return result;
            }

            return new ModelMetadata(this, null, modelAccessor, modelType, null);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExtendedModelMetadataProvider : ModelMetadataProvider
    {
        private readonly IModelMetadataRegistry registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcExtensions.WebApi.ExtendedModelMetadataProvider"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        public ExtendedModelMetadataProvider(IModelMetadataRegistry registry)
        {
            Invariant.IsNotNull(registry, "registry");

            this.registry = registry;
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

            var metadataItems = registry.GetModelPropertiesMetadata(containerType);

            if (metadataItems == null || metadataItems.Count == 0)
            {
                return null; //base.GetMetadataForProperties(container, containerType);
            }

            IList<ModelMetadata> list = new List<ModelMetadata>();

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(containerType))
            {
                ModelMetadataItem metadata;
                metadataItems.TryGetValue(descriptor.Name, out metadata);

                var tempDescriptor = descriptor;

                var modelMetadata = CreatePropertyMetadata(
                    containerType,
                    tempDescriptor.Name,
                    tempDescriptor.PropertyType,
                    metadata,
                    () => tempDescriptor.GetValue(container));

                list.Add(modelMetadata);
            }

            return list;
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

            var metadataItem = registry.GetModelPropertyMetadata(containerType, propertyName);

            if (metadataItem == null)
            {
                return null; //base.GetMetadataForProperty(modelAccessor, containerType, propertyName);
            }

            var propertyDescriptor = TypeDescriptor.GetProperties(containerType)
                                                   .Cast<PropertyDescriptor>()
                                                   .FirstOrDefault(property => property.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

            if (propertyDescriptor == null)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentUICulture,
                        ExceptionMessages.ThePropertyNameOfTypeCouldNotBeFound,
                        containerType.FullName,
                        propertyName));
            }

            return CreatePropertyMetadata(containerType, propertyName, propertyDescriptor.PropertyType, metadataItem, modelAccessor);
        }

        /// <summary>
        /// Gets metadata for the specified model accessor and model type.
        /// </summary>
        /// <param name="modelAccessor">The model accessor.</param>
        /// <param name="modelType">They type of the model.</param>
        /// <returns>The metadata.</returns>
        public override ModelMetadata GetMetadataForType(Func<object> modelAccessor, Type modelType)
        {
            var metadataItem = registry.GetModelMetadata(modelType);

            return metadataItem == null
                       ? null //base.GetMetadataForType(modelAccessor, modelType)
                       : CreateModelMetadata(modelType, modelAccessor, metadataItem);
        }

        private static void Copy(ModelMetadataItem metadataItem, ModelMetadata metadata)
        {
            if (metadataItem.Description != null)
            {
                metadata.Description = metadataItem.Description();
            }

            if (metadataItem.IsReadOnly.HasValue)
            {
                metadata.IsReadOnly = metadataItem.IsReadOnly.Value;
            }


            if (metadataItem.ConvertEmptyStringToNull.HasValue)
            {
                metadata.ConvertEmptyStringToNull = metadataItem.ConvertEmptyStringToNull.Value;
            }

            /*FluentModelMetadataTransformer.Value.Transform(metadata);
            DisplayNameTransformer.Value.Transform(metadata);*/
        }

        private ModelMetadata CreateModelMetadata(Type modelType, Func<object> modelAccessor, ModelMetadataItem metadataItem)
        {
            ModelMetadata modelMetadata = new ExtendedModelMetadata(this, null, modelAccessor, modelType, null, metadataItem);

            if (metadataItem != null)
            {
                Copy(metadataItem, modelMetadata);
            }

            return modelMetadata;
        }

        private ModelMetadata CreatePropertyMetadata(
            Type containerType, string propertyName, Type propertyType, ModelMetadataItem propertyMetadata, Func<object> modelAccessor)
        {
            ModelMetadata modelMetadata = new ExtendedModelMetadata(this, containerType, modelAccessor, propertyType, propertyName, propertyMetadata);

            if (propertyMetadata != null)
            {
                Copy(propertyMetadata, modelMetadata);
            }

            return modelMetadata;
        }
    }
}
