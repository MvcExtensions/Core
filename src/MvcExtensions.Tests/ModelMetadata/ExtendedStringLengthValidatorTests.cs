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
    using System.Linq;
    using System.Web.Mvc;

    using Moq;
    using Xunit;

    public class ExtendedStringLengthValidatorTests : ValidatorTestsBase
    {
        [Fact]
        public void Should_throw_exception_when_validation_metadata_is_not_string_length_validation_meta_data()
        {
            Assert.Throws<InvalidCastException>(() => new ExtendedStringLengthValidator(CreateModelMetadataWithModel(), new ControllerContext(), new Mock<ModelValidationMetadata>().Object));
        }

        [Fact]
        public void Maximum_length_should_be_same_as_metadata()
        {
            var validator = new ExtendedStringLengthValidatorTestDouble(CreateModelMetadataWithModel(), new ControllerContext(), new StringLengthValidationMetadata { Maximum = 256 });

            Assert.Equal(256, validator.PublicAttribute.MaximumLength);
        }

        [Fact]
        public void Should_be_able_to_get_client_rules()
        {
            var validator = new ExtendedStringLengthValidatorTestDouble(CreateModelMetadataWithModel(), new ControllerContext(), new StringLengthValidationMetadata { Maximum = 256, ErrorMessage = "Value length must be less than or equal to 256 character." });

            var rules = validator.GetClientValidationRules();

            Assert.NotEmpty(rules);
            Assert.IsType<ModelClientValidationStringLengthRule>(rules.First());
        }

        private sealed class ExtendedStringLengthValidatorTestDouble : ExtendedStringLengthValidator
        {
            public ExtendedStringLengthValidatorTestDouble(ModelMetadata metadata, ControllerContext controllerContext, ModelValidationMetadata validationMetadata) : base(metadata, controllerContext, validationMetadata)
            {
            }

            public StringLengthAttribute PublicAttribute
            {
                get
                {
                    return Attribute;
                }
            }
        }
    }
}