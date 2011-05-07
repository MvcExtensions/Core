#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
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
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public Func<string> ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the error message resource.
        /// </summary>
        /// <value>The type of the error message resource.</value>
        public Type ErrorMessageResourceType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the error message resource.
        /// </summary>
        /// <value>The name of the error message resource.</value>
        public string ErrorMessageResourceName
        {
            get;
            set;
        }

        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <param name="modelMetadata">The model metadata.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public ModelValidator CreateValidator(ExtendedModelMetadata modelMetadata, ControllerContext context)
        {
            Invariant.IsNotNull(modelMetadata, "modelMetadata");
            Invariant.IsNotNull(context, "context");

            return CreateValidatorCore(modelMetadata, context);
        }

        /// <summary>
        /// Populates the error message from the given metadata.
        /// </summary>
        /// <param name="validationAttribute"></param>
        public void PopulateErrorMessage(ValidationAttribute validationAttribute)
        {
            Invariant.IsNotNull(this, "validationMetadata");

            string errorMessage = null;

            if (ErrorMessage != null)
            {
                errorMessage = ErrorMessage();
            }

            if (!String.IsNullOrEmpty(errorMessage))
            {
                validationAttribute.ErrorMessage = errorMessage;
            }
            else if ((ErrorMessageResourceType != null) &&
                     (!String.IsNullOrEmpty(ErrorMessageResourceName)))
            {
                validationAttribute.ErrorMessageResourceType = ErrorMessageResourceType;
                validationAttribute.ErrorMessageResourceName = ErrorMessageResourceName;
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