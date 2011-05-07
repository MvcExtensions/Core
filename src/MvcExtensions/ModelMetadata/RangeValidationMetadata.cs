#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// Represents a class to store range validation metadata.
    /// </summary>
    /// <typeparam name="TValueType">The type of the value type.</typeparam>
    public class RangeValidationMetadata<TValueType> : ModelValidationMetadata
    {
        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        public TValueType Minimum
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public TValueType Maximum
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
        protected override ModelValidator CreateValidatorCore(ExtendedModelMetadata modelMetadata, ControllerContext context)
        {
            var attribute = new RangeAttribute(typeof(TValueType), Minimum.ToString(), Maximum.ToString());
            PopulateErrorMessage(attribute);
            return new RangeAttributeAdapter(modelMetadata, context, attribute);
        }
    }
}