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
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Web.Mvc;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines a metadata provider which supports fluent registration.
    /// </summary>
    [TypeForwardedFrom(KnownAssembly.MvcExtensions)]
    public class ExtendedModelMetadataProvider : ConventionalDataAnnotationsModelMetadataProvider
    {
        private readonly IModelMetadataRegistry registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedModelMetadataProvider"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        public ExtendedModelMetadataProvider([NotNull] IModelMetadataRegistry registry)
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
        public override IEnumerable<ModelMetadata> GetMetadataForProperties([NotNull] object container, [NotNull] Type containerType)
        {
            Invariant.IsNotNull(containerType, "containerType");

            var metadataItems = registry.GetModelPropertiesMetadata(containerType);

            if (metadataItems == null || metadataItems.Count == 0)
            {
                return base.GetMetadataForProperties(container, containerType);
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
        public override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, [NotNull] Type containerType, [NotNull] string propertyName)
        {
            Invariant.IsNotNull(containerType, "containerType");
            Invariant.IsNotNull(propertyName, "propertyName");

            var metadataItem = registry.GetModelPropertyMetadata(containerType, propertyName);

            if (metadataItem == null)
            {
                return base.GetMetadataForProperty(modelAccessor, containerType, propertyName);
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

        private static void Copy(ModelMetadataItem metadataItem, ModelMetadata metadata)
        {
            metadata.ShowForDisplay = metadataItem.ShowForDisplay;

            if (metadataItem.DisplayName != null)
            {
                metadata.DisplayName = metadataItem.DisplayName();
            }

            if (metadataItem.ShortDisplayName != null)
            {
                metadata.ShortDisplayName = metadataItem.ShortDisplayName();
            }

            if (!string.IsNullOrEmpty(metadataItem.TemplateName))
            {
                metadata.TemplateHint = metadataItem.TemplateName;
            }

            if (metadataItem.Description != null)
            {
                metadata.Description = metadataItem.Description();
            }

            if (metadataItem.NullDisplayText != null)
            {
                metadata.NullDisplayText = metadataItem.NullDisplayText();
            }

            if (metadataItem.Watermark != null)
            {
                metadata.Watermark = metadataItem.Watermark();
            }

            if (metadataItem.HideSurroundingHtml.HasValue)
            {
                metadata.HideSurroundingHtml = metadataItem.HideSurroundingHtml.Value;
            }

            if (metadataItem.RequestValidationEnabled.HasValue)
            {
                metadata.RequestValidationEnabled = metadataItem.RequestValidationEnabled.Value;
            }

            if (metadataItem.IsReadOnly.HasValue)
            {
                metadata.IsReadOnly = metadataItem.IsReadOnly.Value;
            }

            if (metadataItem.IsRequired.HasValue)
            {
                metadata.IsRequired = metadataItem.IsRequired.Value;
            }

            if (metadataItem.ShowForEdit.HasValue)
            {
                metadata.ShowForEdit = metadataItem.ShowForEdit.Value;
            }
            else
            {
                metadata.ShowForEdit = !metadata.IsReadOnly;
            }

            if (metadataItem.Order.HasValue)
            {
                metadata.Order = metadataItem.Order.Value;
            }

            if (metadataItem.DisplayFormat != null)
            {
                metadata.DisplayFormatString = metadataItem.DisplayFormat();
            }

            if (metadataItem.ApplyFormatInEditMode && metadata.ShowForEdit && metadataItem.EditFormat != null)
            {
                metadata.EditFormatString = metadataItem.EditFormat();
            }

            if (metadataItem.ConvertEmptyStringToNull.HasValue)
            {
                metadata.ConvertEmptyStringToNull = metadataItem.ConvertEmptyStringToNull.Value;
            }

            foreach (var item in metadataItem.MetadataAwares)
            {
                item.OnMetadataCreated(metadata);
            }

            FluentModelMetadataTransformer.Transform(metadata);
            MvcExtensions.DisplayNameTransformer.Transform(metadata);
        }

        [NotNull]
        private ModelMetadata CreateModelMetadata(Type modelType, Func<object> modelAccessor, ModelMetadataItem metadataItem)
        {
            ModelMetadata modelMetadata = new ExtendedModelMetadata(this, null, modelAccessor, modelType, null, metadataItem);

            if (metadataItem != null)
            {
                Copy(metadataItem, modelMetadata);
                
            }

            return modelMetadata;
        }

        [NotNull]
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