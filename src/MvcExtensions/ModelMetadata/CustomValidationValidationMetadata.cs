namespace MvcExtensions
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Represents a class to store custom validation metadata.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomValidationValidationMetadata<T> : IModelValidationMetadata
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
        /// <param name="modelMetadata">The model metadata.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public ModelValidator CreateValidator(ExtendedModelMetadata modelMetadata, ControllerContext context)
        {
            var validator = Factory(modelMetadata, context);
            Configure(validator);
            return validator;
        }
    }
}