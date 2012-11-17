namespace MvcExtensions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public abstract class CustomValidatorAttribute : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        protected CustomValidatorAttribute()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        protected CustomValidatorAttribute(string errorMessage) : base(errorMessage)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessageAccessor"></param>
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
        /// <param name="value">The value to validate.</param><param name="validationContext">The context information about the validation operation.</param>
        protected override sealed ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult result = ValidationResult.Success;
            if (!IsValid(value, validationContext.ObjectInstance))
            {
                var name = validationContext.MemberName == null ? null : new[]{validationContext.MemberName};
                result = new ValidationResult(FormatErrorMessage(validationContext.DisplayName), name);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public abstract bool IsValid(object value, object container);
    }
}