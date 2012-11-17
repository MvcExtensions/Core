namespace MvcExtensions
{
    using System;

    /// <summary>
    /// Allow to validate value with delegate based validation
    /// </summary>
    internal sealed class DelegateBasedValidatorAttribute : CustomValidatorAttribute
    {
        private readonly Func<object, object, bool> validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateBasedValidatorAttribute"/> class.
        /// </summary>
        /// <param name="validator">The validator of {Model, Value, Result}</param>
        internal DelegateBasedValidatorAttribute(Func<object, object, bool> validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateBasedValidatorAttribute"/> class.
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <param name="validator">The validator</param>
        internal DelegateBasedValidatorAttribute(Func<object, object, bool> validator, string errorMessage)
            : base(errorMessage)
        {
            this.validator = validator;
        }

        /// <summary>
        /// When implemented in a derived class, validates the object.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="container">The container.</param>
        /// <returns>A list of validation results.</returns>
        public override bool IsValid(object value, object container)
        {
            return validator(container, value);
        }
    }
}