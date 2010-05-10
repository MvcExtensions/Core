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
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class that is used to validate string length.
    /// </summary>
    public class ExtendedStringLengthValidator : ExtendedValidator<StringLengthAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedStringLengthValidator"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="validationMetadata">The validation metadata.</param>
        public ExtendedStringLengthValidator(ModelMetadata metadata, ControllerContext controllerContext, ModelValidationMetadata validationMetadata) : base(metadata, controllerContext)
        {
            StringLengthValidationMetadata stringLengthValidationMetadata = validationMetadata as StringLengthValidationMetadata;

            if (stringLengthValidationMetadata == null)
            {
                throw new InvalidCastException();
            }

            Attribute = new StringLengthAttribute(stringLengthValidationMetadata.Maximum);
            PopulateErrorMessage(stringLengthValidationMetadata);
        }

        /// <summary>
        /// Gets metadata for client validation.
        /// </summary>
        /// <returns>The metadata for client validation.</returns>
        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] { new ModelClientValidationStringLengthRule(ErrorMessage, 0, Attribute.MaximumLength) };
        }
    }
}