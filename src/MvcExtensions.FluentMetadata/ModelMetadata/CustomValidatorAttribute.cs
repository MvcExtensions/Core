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

    /// <summary>
    /// Serves as the base class for all custom validation attributes.
    /// </summary>
    /// <exception cref="T:MvcExtensions.CustomValidatorAttribute">The <see cref="P:System.ComponentModel.DataAnnotations.ValidationAttribute.ErrorMessageResourceType"/> and <see cref="P:System.ComponentModel.DataAnnotations.ValidationAttribute.ErrorMessageResourceName"/> properties for localized error message are set at the same time that the non-localized <see cref="P:System.ComponentModel.DataAnnotations.ValidationAttribute.ErrorMessage"/> property error message is set.</exception>
    public abstract class CustomValidatorAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MvcExtensions.CustomValidatorAttribute"/> class.
        /// </summary>
        protected CustomValidatorAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MvcExtensions.CustomValidatorAttribute"/> class by using the error message to associate with a validation control.
        /// </summary>
        /// <param name="errorMessage">The error message to associate with a validation control.</param>
        protected CustomValidatorAttribute(string errorMessage) : base(errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MvcExtensions.CustomValidatorAttribute"/> class by using the function that enables access to validation resources.
        /// </summary>
        /// <param name="errorMessageAccessor">The function that enables access to validation resources.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="errorMessageAccessor"/> is null.</exception>
        protected CustomValidatorAttribute(Func<string> errorMessageAccessor) : base(errorMessageAccessor)
        {
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid. 
        /// </summary>
        /// <returns>
        /// true if the specified value is valid; otherwise, false.
        /// </returns>
        /// <param name="value">The value of the object to validate. </param>
        public override sealed bool IsValid(object value)
        {
            return IsValid(value, null);
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult"/> class. 
        /// </returns>
        /// <param name="value">Target property to validate</param>
        /// <param name="container">The Model</param>
        public abstract bool IsValid(object value, object container);

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult"/> class. 
        /// </returns>
        /// <param name="value">The value to validate.</param><param name="validationContext">The context information about the validation operation.</param>
        protected override sealed ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = ValidationResult.Success;
            if (!IsValid(value, validationContext.ObjectInstance))
            {
                var name = validationContext.MemberName == null ? null : new[] { validationContext.MemberName };
                result = new ValidationResult(FormatErrorMessage(validationContext.DisplayName), name);
            }

            return result;
        }
    }
}
