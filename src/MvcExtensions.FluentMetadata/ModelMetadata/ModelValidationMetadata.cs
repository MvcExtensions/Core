#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// Represents a base class to store validation metadata.
    /// </summary>
    public abstract class ModelValidationMetadata : IModelValidationMetadata
    {
        private ModelMetadata modelMetadata;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public Func<string> ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the name of the error message resource.
        /// </summary>
        /// <value>The name of the error message resource.</value>
        public string ErrorMessageResourceName { get; set; }

        /// <summary>
        /// Gets or sets the type of the error message resource.
        /// </summary>
        /// <value>The type of the error message resource.</value>
        public Type ErrorMessageResourceType { get; set; }

        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public ModelValidator CreateValidator(ExtendedModelMetadata metadata, ControllerContext context)
        {
            Invariant.IsNotNull(metadata, "metadata");
            Invariant.IsNotNull(context, "context");

            modelMetadata = metadata;

            return CreateValidatorCore(metadata, context);
        }

        /// <summary>
        /// Populates the error message from the given metadata.
        /// </summary>
        /// <param name="validationAttribute"></param>
        public void PopulateErrorMessage(ValidationAttribute validationAttribute)
        {
            Invariant.IsNotNull(validationAttribute, "validationMetadata");

            string errorMessage = null;

            if (ErrorMessage != null)
            {
                errorMessage = ErrorMessage();
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                validationAttribute.ErrorMessage = errorMessage;
            }
            else if (ErrorMessageResourceType != null && !string.IsNullOrEmpty(ErrorMessageResourceName))
            {
                validationAttribute.ErrorMessageResourceType = ErrorMessageResourceType;
                validationAttribute.ErrorMessageResourceName = ErrorMessageResourceName;
            }
            else if (ConventionSettings.ConventionsActive)
            {
                // enables support for partial matadata
                if (ErrorMessageResourceType != null)
                {
                    validationAttribute.ErrorMessageResourceType = ErrorMessageResourceType;
                }

                if (!string.IsNullOrEmpty(ErrorMessageResourceName))
                {
                    validationAttribute.ErrorMessageResourceName = ErrorMessageResourceName;
                }

                var conventionType = modelMetadata.With(m => m.ContainerType);
                var propertyName = modelMetadata.With(m => m.PropertyName);
                var defaultResourceType = ConventionSettings.GetDefaultResourceType(conventionType);

                var transformer = ConventionalDataAnnotationsModelMetadataProvider.ValidationAttributeTransformer.Value;
                transformer.Transform(validationAttribute, conventionType, propertyName, defaultResourceType);
            }
        }

        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <param name="modelMetadata">The model metadata.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected abstract ModelValidator CreateValidatorCore(ExtendedModelMetadata modelMetadata, ControllerContext context);
    }
}
