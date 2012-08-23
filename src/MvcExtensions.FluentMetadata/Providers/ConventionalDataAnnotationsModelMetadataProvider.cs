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
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a metadata provider which supports conventional DataAnnotations model registration.
    /// </summary>
    public class ConventionalDataAnnotationsModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        internal static readonly Lazy<DisplayNameTransformer> DisplayNameTransformer = new Lazy<DisplayNameTransformer>(() => new DisplayNameTransformer());
        internal static readonly Lazy<ValidationAttributeTransformer> ValidationAttributeTransformer = new Lazy<ValidationAttributeTransformer>(() => new ValidationAttributeTransformer());
        internal static readonly Lazy<DisplayAttributeTransformer> DisplayAttributeTransformer = new Lazy<DisplayAttributeTransformer>(() => new DisplayAttributeTransformer());

        /// <summary>
        /// Gets the metadata for the specified property.
        /// </summary>
        /// <returns>
        /// The metadata for the property.
        /// </returns>
        /// <param name="attributes">The attributes.</param>
        /// <param name="containerType">The type of the container.</param>
        /// <param name="modelAccessor">The model accessor.</param>
        /// <param name="modelType">The type of the model.</param>
        /// <param name="propertyName">The name of the property.</param>
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            List<Attribute> newAttributes = null;
            if (ConventionSettings.ConventionsActive)
            {
                var defaultResourceType = ConventionSettings.GetDefaultResourceType(containerType);

                if (defaultResourceType != null)
                {
                    newAttributes = new List<Attribute>();

                    DisplayAttribute displayAttribute = null;
                    foreach (var attribute in attributes)
                    {
                        if (attribute is ValidationAttribute)
                        {
                            ValidationAttributeTransformer.Value.Transform((ValidationAttribute)attribute, containerType, propertyName, defaultResourceType);
                            newAttributes.Add(attribute);
                        }
                        else if (attribute is DisplayAttribute)
                        {
                            displayAttribute = attribute as DisplayAttribute;
                        }
                        else
                        {
                            newAttributes.Add(attribute);
                        }
                    }

                    // ensure we have DisplayAttribute
                    displayAttribute = displayAttribute ?? new DisplayAttribute();
                  
                    DisplayAttributeTransformer.Value.Transform(displayAttribute, containerType, propertyName, defaultResourceType);
                    newAttributes.Add(displayAttribute);
                }
            }

            var metadata = base.CreateMetadata(newAttributes ?? attributes, containerType, modelAccessor, modelType, propertyName);
            DisplayNameTransformer.Value.Transform(metadata);
            return metadata;
        }
    }
}
