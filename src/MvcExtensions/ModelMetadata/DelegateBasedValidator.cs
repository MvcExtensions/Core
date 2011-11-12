namespace MvcExtensions
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Allow to validate value with delegate based validation
    /// </summary>
    public class DelegateBasedValidator : ExtendedValidator
    {
        private readonly Func<ControllerContext, object, bool> validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateBasedValidator"/> class.
        /// </summary>
        /// <param name="metadata">The model metadata</param>
        /// <param name="controllerContext">The controller context</param>
        /// <param name="validator">The validator</param>
        public DelegateBasedValidator(ModelMetadata metadata, ControllerContext controllerContext, Func<ControllerContext, object, bool> validator)
            : base(metadata, controllerContext)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Validates the object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c></returns>
        protected override bool ValidateCore(object value)
        {
            return validator(ControllerContext, value);
        }
    }
}