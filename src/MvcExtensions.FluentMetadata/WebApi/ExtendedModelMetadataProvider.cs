namespace MvcExtensions.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Web.Http.Metadata;

    /// <summary>
    /// Custom ModelMetadataProvider for WebApi
    /// </summary>
    public class ExtendedModelMetadataProvider : ModelMetadataProvider
    {
        private readonly IModelMetadataRegistry registry;
        private readonly ModelMetadataProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MvcExtensions.WebApi.ExtendedModelMetadataProvider"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <param name="provider">Fallback ModelValidatorProvider</param>
        public ExtendedModelMetadataProvider(IModelMetadataRegistry registry, ModelMetadataProvider provider)
        {
            Invariant.IsNotNull(registry, "registry");

            this.registry = registry;
            this.provider = provider;
        }


        /// <summary>
        /// Gets a <see cref="T:System.Web.Http.Metadata.ModelMetadata"/> object for each property of a model.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="containerType">The type of the container.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Http.Metadata.ModelMetadata"/> object for each property of a model.
        /// </returns>
        public override IEnumerable<ModelMetadata> GetMetadataForProperties(object container, Type containerType)
        {
            Invariant.IsNotNull(containerType, "containerType");

            var metadataItems = registry.GetModelPropertiesMetadata(containerType);

            if (metadataItems == null || metadataItems.Count == 0)
            {
                return null; //provider.GetMetadataForProperties(container, containerType);
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
                return provider.GetMetadataForProperty(modelAccessor, containerType, propertyName);
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
                       ? provider.GetMetadataForType(modelAccessor, modelType)
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