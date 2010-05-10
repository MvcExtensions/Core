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
    /// Defines a class that is used to validate range.
    /// </summary>
    /// <typeparam name="TValueType">The type of the value type.</typeparam>
    public class ExtendedRangeValidator<TValueType> : ExtendedValidator<RangeAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedRangeValidator&lt;TValueType&gt;"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="validationMetadata">The validation metadata.</param>
        public ExtendedRangeValidator(ModelMetadata metadata, ControllerContext controllerContext, ModelValidationMetadata validationMetadata) : base(metadata, controllerContext)
        {
            RangeValidationMetadata<TValueType> rangeValidationMetadata = validationMetadata as RangeValidationMetadata<TValueType>;

            if (rangeValidationMetadata == null)
            {
                throw new InvalidCastException();
            }

            Attribute = new RangeAttribute(typeof(TValueType), rangeValidationMetadata.Minimum.ToString(), rangeValidationMetadata.Maximum.ToString());
            PopulateErrorMessage(rangeValidationMetadata);
        }

        /// <summary>
        /// Gets metadata for client validation.
        /// </summary>
        /// <returns>The metadata for client validation.</returns>
        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] { new ModelClientValidationRangeRule(ErrorMessage, Attribute.Minimum, Attribute.Maximum) };
        }
    }
}