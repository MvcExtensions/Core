#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class that is used to validate whether a value is specified.
    /// </summary>
    public class ExtendedRequiredValidator : ExtendedValidator<RequiredAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedRequiredValidator"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="validationMetadata">The validation metadata.</param>
        public ExtendedRequiredValidator(ModelMetadata metadata, ControllerContext controllerContext, IModelValidationMetadata validationMetadata) : base(metadata, controllerContext)
        {
            Invariant.IsNotNull(validationMetadata, "validationMetadata");

            Attribute = new RequiredAttribute();
            PopulateErrorMessage(validationMetadata);
        }

        /// <summary>
        /// Gets metadata for client validation.
        /// </summary>
        /// <returns>The metadata for client validation.</returns>
        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] { new ModelClientValidationRequiredRule(ErrorMessage) };
        }
    }
}