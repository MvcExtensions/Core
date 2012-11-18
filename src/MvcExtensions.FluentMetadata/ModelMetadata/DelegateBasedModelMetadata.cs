namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using ModelValidatorProvider = System.Web.Http.Validation.ModelValidatorProvider;

    /// <summary>
    /// 
    /// </summary>
    public class DelegateBasedModelMetadata : ModelValidationMetadata
    {
        private readonly IList<DelegateBasedValidatorAttribute> validators;

        /// <summary>
        /// 
        /// </summary>
        public DelegateBasedModelMetadata()
        {
            validators = new List<DelegateBasedValidatorAttribute>();
        }

        /// <summary>
        /// Returns current count of validators 
        /// </summary>
        /// <returns></returns>
        internal int GetValidatorsCount()
        {
            return validators.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validate"></param>
        /// <param name="errorMessage"></param>
        public void AddValidator(Func<object, object, bool> validate, string errorMessage)
        {
            var attribute = new DelegateBasedValidatorAttribute(validate, errorMessage);
           // base.PopulateErrorMessage(attribute);
            validators.Add(attribute);
        }

        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override ModelValidator CreateValidatorCore(ExtendedModelMetadata metadata, ControllerContext context)
        {
            return new DelegateBasedModelValidator(metadata, context, validators);
        }

        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <returns></returns>
        public override System.Web.Http.Validation.ModelValidator CreateWebApiValidator(IEnumerable<ModelValidatorProvider> validatorProviders)
        {
            return new WebApiDelegateBasedModelValidator(validatorProviders, validators);
        }


        internal sealed class WebApiDelegateBasedModelValidator : System.Web.Http.Validation.ModelValidator
        {
            private readonly IList<DelegateBasedValidatorAttribute> attributes;

            public WebApiDelegateBasedModelValidator(IEnumerable<ModelValidatorProvider> validatorProviders, IList<DelegateBasedValidatorAttribute> attributes)
                : base(validatorProviders)
            {
                this.attributes = attributes;
            }

            public override IEnumerable<System.Web.Http.Validation.ModelValidationResult> Validate(System.Web.Http.Metadata.ModelMetadata metadata, object container)
            {
                foreach (DelegateBasedValidatorAttribute attribute in attributes)
                {
                    var context = new ValidationContext(container ?? metadata.Model, null, null) { DisplayName = metadata.GetDisplayName() };

                    var result = attribute.GetValidationResult(metadata.Model, context);

                    if (result != null && result != ValidationResult.Success)
                    {
                        yield return new System.Web.Http.Validation.ModelValidationResult { Message = result.ErrorMessage };
                    }
                }
            }
        }


        internal sealed class DelegateBasedModelValidator : ModelValidator
        {
            private readonly IEnumerable<DelegateBasedValidatorAttribute> attributes;

            public DelegateBasedModelValidator(ModelMetadata metadata, ControllerContext controllerContext, IEnumerable<DelegateBasedValidatorAttribute> attributes)
                : base(metadata, controllerContext)
            {
                this.attributes = attributes;
            }

            public override IEnumerable<ModelValidationResult> Validate(object container)
            {
                return attributes
                    .Where(validator => !validator.IsValid(Metadata.Model, container))
                    .Select(
                        v => new ModelValidationResult
                        {
                            Message = v.FormatErrorMessage(Metadata.GetDisplayName())
                        });
            }
        }

        /// <summary>
        /// Allow to validate value with delegate based validation
        /// </summary>
        internal sealed class DelegateBasedValidatorAttribute : CustomValidatorAttribute
        {
            private readonly Func<object, object, bool> validator;

            /*/// <summary>
            /// Initializes a new instance of the <see cref="DelegateBasedValidatorAttribute"/> class.
            /// </summary>
            /// <param name="validator">The validator of {Model, Value, Result}</param>
            internal DelegateBasedValidatorAttribute(Func<object, object, bool> validator)
            {
                this.validator = validator;
            }*/

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
   
}