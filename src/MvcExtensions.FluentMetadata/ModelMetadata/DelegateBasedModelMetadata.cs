#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>, 2012 AlexBar <abarbashin@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
#if !MVC_3
    using ModelValidator = System.Web.Http.Validation.ModelValidator;
    using ModelValidatorProvider = System.Web.Http.Validation.ModelValidatorProvider;
#endif

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

#if !MVC_3
        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <returns></returns>
        public override ModelValidator CreateWebApiValidator(IEnumerable<ModelValidatorProvider> validatorProviders)
        {
            return new WebApiDelegateBasedModelValidator(validatorProviders, validators);
        }
#endif

        /// <summary>
        /// Returns current count of validators 
        /// </summary>
        /// <returns></returns>
        internal int GetValidatorsCount()
        {
            return validators.Count;
        }

        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override System.Web.Mvc.ModelValidator CreateValidatorCore(ExtendedModelMetadata metadata, ControllerContext context)
        {
            return new DelegateBasedModelValidator(metadata, context, validators);
        }

        internal sealed class DelegateBasedModelValidator : System.Web.Mvc.ModelValidator
        {
            private readonly IEnumerable<DelegateBasedValidatorAttribute> attributes;

            public DelegateBasedModelValidator(
                ModelMetadata metadata, ControllerContext controllerContext, IEnumerable<DelegateBasedValidatorAttribute> attributes)
                : base(metadata, controllerContext)
            {
                this.attributes = attributes;
            }

            public override IEnumerable<ModelValidationResult> Validate(object container)
            {
                foreach (var attribute in attributes)
                {
                    var context = new ValidationContext(container ?? Metadata.Model, null, null) { DisplayName = Metadata.GetDisplayName() };

                    var result = attribute.GetValidationResult(Metadata.Model, context);

                    if (result != null && result != ValidationResult.Success)
                    {
                        yield return new ModelValidationResult { Message = result.ErrorMessage };
                    }
                }
            }
        }

#if !MVC_3
        internal sealed class WebApiDelegateBasedModelValidator : ModelValidator
        {
            private readonly IList<DelegateBasedValidatorAttribute> attributes;

            public WebApiDelegateBasedModelValidator(IEnumerable<ModelValidatorProvider> validatorProviders, IList<DelegateBasedValidatorAttribute> attributes)
                : base(validatorProviders)
            {
                this.attributes = attributes;
            }

            public override IEnumerable<System.Web.Http.Validation.ModelValidationResult> Validate(System.Web.Http.Metadata.ModelMetadata metadata, object container)
            {
                foreach (var attribute in attributes)
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
#endif

    }

    /// <summary>
    /// Allow to validate value with delegate based validation
    /// </summary>
    internal sealed class DelegateBasedValidatorAttribute : CustomValidatorAttribute
    {
        private readonly Func<object, object, bool> validator;

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
