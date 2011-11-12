namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;

    /// <summary>
    /// Allow to validate value with delegate based validation
    /// </summary>
    public class DelegateBasedValidator : ModelValidator
    {
        private readonly Func<object, object, bool> validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateBasedValidator"/> class.
        /// </summary>
        /// <param name="metadata">The model metadata</param>
        /// <param name="controllerContext">The controller context</param>
        /// <param name="validator">The validator</param>
        public DelegateBasedValidator(ModelMetadata metadata, ControllerContext controllerContext, Func<object, object, bool> validator)
            : base(metadata, controllerContext)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented in a derived class, validates the object.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>A list of validation results.</returns>
        public override sealed IEnumerable<ModelValidationResult> Validate(object container)
        {
            if (!validator(container, Metadata.Model))
            {
                yield return new ModelValidationResult
                                 {
                                     Message = string.Format(CultureInfo.CurrentCulture, ErrorMessage, Metadata.GetDisplayName())
                                 };
            }
        }
    }
}