namespace MvcExtensions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// AbstractCustomValidator
    /// </summary>
    public class CustomValidationMetadata<TValidator> : ModelValidationMetadata where TValidator : CustomValidatorAttribute
    {
        /// <summary>
        /// The configuration for targer configurator
        /// </summary>
        public Action<TValidator> ConfigureValidator { get; set; }

        /// <summary>
        /// The factory
        /// </summary>
        public Func<TValidator> Factory { get; set; }

        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <param name="modelMetadata">The model metadata.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override ModelValidator CreateValidatorCore(ExtendedModelMetadata modelMetadata, ControllerContext context)
        {
            return new DataAnnotationsModelValidator<TValidator>(modelMetadata, context, (TValidator)CreateValidationAttribute());
        }

        /// <summary>
        /// Creates validation attribute
        /// </summary>
        /// <returns>Instance of ValidationAttribute type</returns>
        protected override ValidationAttribute CreateValidationAttribute()
        {
            var validator = Factory != null ? Factory() : Activator.CreateInstance<TValidator>();
            PopulateErrorMessage(validator);
            if (ConfigureValidator != null)
            {
                ConfigureValidator(validator);
            }

            return validator;
        }
    }
}