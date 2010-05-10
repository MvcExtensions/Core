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
    /// Defines a class that is used to validate regular expression.
    /// </summary>
    public class ExtendedRegularExpressionValidator : ExtendedValidator<RegularExpressionAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedRegularExpressionValidator"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="validationMetadata">The validation metadata.</param>
        public ExtendedRegularExpressionValidator(ModelMetadata metadata, ControllerContext controllerContext, ModelValidationMetadata validationMetadata) : base(metadata, controllerContext)
        {
            RegularExpressionValidationMetadata regularExpressionValidationMetadata = validationMetadata as RegularExpressionValidationMetadata;

            if (regularExpressionValidationMetadata == null)
            {
                throw new InvalidCastException();
            }

            Attribute = new RegularExpressionAttribute(regularExpressionValidationMetadata.Pattern);
            PopulateErrorMessage(regularExpressionValidationMetadata);
        }

        /// <summary>
        /// Gets metadata for client validation.
        /// </summary>
        /// <returns>The metadata for client validation.</returns>
        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] { new ModelClientValidationRegexRule(ErrorMessage, Attribute.Pattern) };
        }
    }
}