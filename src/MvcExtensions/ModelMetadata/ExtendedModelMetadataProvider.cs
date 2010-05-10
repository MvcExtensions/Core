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
    using System.ComponentModel;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a metadata provider which supports fluent registration.
    /// </summary>
    public class ExtendedModelMetadataProvider : ExtendedModelMetadataProviderBase
    {
        private readonly IModelMetadataRegistry registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedModelMetadataProvider"/> class.
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

            IDictionary<string, ModelMetadataItem> metadataDictionary = registry.Matching(containerType);

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(containerType))
            {
                ModelMetadataItem metadata = null;

                if (metadataDictionary != null)
                {
                    metadataDictionary.TryGetValue(descriptor.Name, out metadata);
                }

                PropertyDescriptor tempDescriptor = descriptor;

                yield return CreateModelMetadata(containerType, tempDescriptor.Name, tempDescriptor.PropertyType, metadata, () => tempDescriptor.GetValue(container));
            }
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

            PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(containerType)
                                                                  .Cast<PropertyDescriptor>()
                                                                  .FirstOrDefault(property => property.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

            if (propertyDescriptor == null)
            {
                throw new ArgumentException(string.Format(Culture.Current, ExceptionMessages.ThePropertyNameOfTypeCouldNotBeFound, containerType.FullName, propertyName));
            }

            return CreateModelMetadata(containerType, propertyName, propertyDescriptor.PropertyType, registry.Matching(containerType, propertyName), modelAccessor);
        }

        /// <summary>
        /// Gets metadata for the specified model accessor and model type.
        /// </summary>
        /// <param name="modelAccessor">The model accessor.</param>
        /// <param name="modelType">They type of the model.</param>
        /// <returns>The metadata.</returns>
        public override ModelMetadata GetMetadataForType(Func<object> modelAccessor, Type modelType)
        {
            return new ExtendedModelMetadata(this, null, modelAccessor, modelType, null, null);
        }

        /// <summary>
        /// Determines whether the specified model type is registered.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>
        /// <c>true</c> if the specified model type is registered; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsRegistered(Type modelType)
        {
            return registry.IsRegistered(modelType);
        }

        /// <summary>
        /// Determines whether the specified model type with the property name is registered.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// <c>true</c> if the specified model type with property name is registered; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsRegistered(Type modelType, string propertyName)
        {
            return registry.IsRegistered(modelType, propertyName);
        }

        /// <summary>
        /// Creates the model metadata.
        /// </summary>
        /// <param name="containerType">Type of the container.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="propertyMetadata">The property meta data.</param>
        /// <param name="modelAccessor">The model accessor.</param>
        /// <returns></returns>
        protected virtual ModelMetadata CreateModelMetadata(Type containerType, string propertyName, Type propertyType, ModelMetadataItem propertyMetadata, Func<object> modelAccessor)
        {
            if (propertyMetadata == null)
            {
                return new ExtendedModelMetadata(this, containerType, modelAccessor, propertyType, propertyName, propertyMetadata);
            }

            ModelMetadata modelMetadata = new ExtendedModelMetadata(this, containerType, modelAccessor, propertyType, propertyName, propertyMetadata)
                                              {
                                                  ShowForDisplay = propertyMetadata.ShowForDisplay,
                                              };

            modelMetadata.DisplayName = !string.IsNullOrEmpty(propertyMetadata.DisplayName) ? propertyMetadata.DisplayName : modelMetadata.DisplayName;
            modelMetadata.ShortDisplayName = !string.IsNullOrEmpty(propertyMetadata.ShortDisplayName) ? propertyMetadata.ShortDisplayName : modelMetadata.ShortDisplayName;
            modelMetadata.TemplateHint = !string.IsNullOrEmpty(propertyMetadata.TemplateName) ? propertyMetadata.TemplateName : modelMetadata.TemplateHint;
            modelMetadata.Description = !string.IsNullOrEmpty(propertyMetadata.Description) ? propertyMetadata.Description : modelMetadata.Description;
            modelMetadata.NullDisplayText = !string.IsNullOrEmpty(propertyMetadata.NullDisplayText) ? propertyMetadata.NullDisplayText : modelMetadata.NullDisplayText;
            modelMetadata.Watermark = !string.IsNullOrEmpty(propertyMetadata.Watermark) ? propertyMetadata.Watermark : modelMetadata.Watermark;

            if (propertyMetadata.HideSurroundingHtml.HasValue)
            {
                modelMetadata.HideSurroundingHtml = propertyMetadata.HideSurroundingHtml.Value;
            }

            if (propertyMetadata.IsReadOnly.HasValue)
            {
                modelMetadata.IsReadOnly = propertyMetadata.IsReadOnly.Value;
            }

            if (propertyMetadata.IsRequired.HasValue)
            {
                modelMetadata.IsRequired = propertyMetadata.IsRequired.Value;
            }

            if (propertyMetadata.ShowForEdit.HasValue)
            {
                modelMetadata.ShowForEdit = propertyMetadata.ShowForEdit.Value;
            }
            else
            {
                modelMetadata.ShowForEdit = !modelMetadata.IsReadOnly;
            }

            IModelMetadataFormattableItem formattableItem = propertyMetadata as IModelMetadataFormattableItem;

            if (formattableItem != null)
            {
                modelMetadata.DisplayFormatString = formattableItem.DisplayFormat;

                if (formattableItem.ApplyFormatInEditMode && modelMetadata.ShowForEdit)
                {
                    modelMetadata.EditFormatString = formattableItem.EditFormat;
                }
            }

            StringMetadataItem stringMetadataItem = propertyMetadata as StringMetadataItem;

            if (stringMetadataItem != null)
            {
                modelMetadata.ConvertEmptyStringToNull = stringMetadataItem.ConvertEmptyStringToNull;
            }

            return modelMetadata;
        }
    }
}