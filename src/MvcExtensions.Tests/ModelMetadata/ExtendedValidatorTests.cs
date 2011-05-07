#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Xunit;
    using Xunit.Extensions;

    public class ExtendedValidatorTests : ValidatorTestsBase
    {
        [Fact]
        public void Attribute_should_not_be_null()
        {
            var validator = new ExtendedValidatorTestDouble(CreateModelMetadataWithModel(), new ControllerContext(), new RequiredValidationMetadata());

            Assert.NotNull(validator.ValidationAttribute);
        }

        [Theory]
        [InlineData("Value must be present", null, null)]
        [InlineData(null, typeof(DummyObjectResource), "ErrorMessage")]
        [InlineData(null, null, null)]
        public void ErrorMessage_should_not_be_blank_validation_metadata_has_error_message(string errorMessage, Type errorMessageResourceType, string errorMessageResourceName)
        {
            var validator = new ExtendedValidatorTestDouble(CreateModelMetadataWithModel(), new ControllerContext(), new RequiredValidationMetadata { ErrorMessage = () => errorMessage, ErrorMessageResourceType = errorMessageResourceType, ErrorMessageResourceName = errorMessageResourceName });

            Assert.NotNull(validator.ValidationErrorMessage);
            Assert.NotEqual(string.Empty, validator.ValidationErrorMessage);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_be_able_to_validate(bool hasValue)
        {
            var modelMetadata = hasValue ? CreateModelMetadataWithModel() : CreateModelMetadataWithoutModel();

            var validator = new ExtendedValidatorTestDouble(modelMetadata, new ControllerContext(), new RequiredValidationMetadata());

            var result = validator.Validate(this);

            if (hasValue)
            {
                Assert.Empty(result);
            }
            else
            {
                Assert.NotEmpty(result);
            }
        }

        public class DummyObjectResource
        {
            public static string ErrorMessage
            {
                get { return "Value must be present"; }
            }
        }

        private sealed class ExtendedValidatorTestDouble : ExtendedValidator<RequiredAttribute>
        {
            public ExtendedValidatorTestDouble(ModelMetadata metadata, ControllerContext controllerContext, ModelValidationMetadata validationMetadata) : base(metadata, controllerContext)
            {
                var attribute = new RequiredAttribute();
                validationMetadata.PopulateErrorMessage(attribute);
                Attribute = attribute;
            }

            public RequiredAttribute ValidationAttribute
            {
                get { return Attribute; }
            }

            public string ValidationErrorMessage
            {
                get { return ErrorMessage; }
            }
        }
    }
}