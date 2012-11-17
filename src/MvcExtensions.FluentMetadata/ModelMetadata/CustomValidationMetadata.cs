namespace MvcExtensions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.CompilerServices;
    using System.Web.Mvc;

    /// <summary>
    /// Represents a class to store custom validation metadata.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [TypeForwardedFrom(KnownAssembly.MvcExtensions)]
    public class CustomValidationMetadata<T> : IModelValidationMetadata
        where T : ModelValidator
    {
        /// <summary>
        /// The configuration
        /// </summary>
        public Action<T> Configure
        {
            get;
            set;
        }

        /// <summary>
        /// The factory
        /// </summary>
        public Func<ModelMetadata, ControllerContext, T> Factory
        {
            get;
            set;
        }

        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public ModelValidator CreateValidator(ExtendedModelMetadata metadata, ControllerContext context)
        {
            var validator = Factory(metadata, context);
            Configure(validator);
            return validator;
        }

        /// <summary>
        /// Creates validation attribute
        /// </summary>
        /// <returns>Instance of ValidationAttribute type</returns>
        public ValidationAttribute CreateValidationAttribute()
        {
            return null; //new CustomValidationAttribute();
        }

        
    }
}