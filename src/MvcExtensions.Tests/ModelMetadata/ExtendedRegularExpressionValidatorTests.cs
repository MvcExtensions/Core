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

    public class ExtendedRegularExpressionValidatorTests : ValidatorTestsBase
    {
        [Fact]
        public void Should_throw_exception_when_validation_metadata_is_not_regular_expression_validation_meta_data()
        {
            Assert.Throws<InvalidCastException>(() => new ExtendedRegularExpressionValidator(CreateModelMetadataWithModel(), new ControllerContext(), new Mock<ModelValidationMetadata>().Object));
        }

        [Fact]
        public void Pattern_should_be_same_as_metadata()
        {
            var validator = new ExtendedRegularExpressionValidatorTestDouble(CreateModelMetadataWithModel(), new ControllerContext(), new RegularExpressionValidationMetadata { Pattern = @"^[a-zA-Z0-9]+$" });

            Assert.Equal(@"^[a-zA-Z0-9]+$", validator.PublicAttribute.Pattern);
        }

        [Fact]
        public void Should_be_able_to_get_client_rules()
        {
            var validator = new ExtendedRegularExpressionValidatorTestDouble(CreateModelMetadataWithModel(), new ControllerContext(), new RegularExpressionValidationMetadata { Pattern = @"^[a-zA-Z0-9]+$", ErrorMessage = () => "Value must be alphanumeric." });

            var rules = validator.GetClientValidationRules();

            Assert.NotEmpty(rules);
            Assert.IsType<ModelClientValidationRegexRule>(rules.First());
        }

        private sealed class ExtendedRegularExpressionValidatorTestDouble : ExtendedRegularExpressionValidator
        {
            public ExtendedRegularExpressionValidatorTestDouble(ModelMetadata metadata, ControllerContext controllerContext, ModelValidationMetadata validationMetadata) : base(metadata, controllerContext, validationMetadata)
            {
            }

            public RegularExpressionAttribute PublicAttribute
            {
                get
                {
                    return Attribute;
                }
            }
        }
    }
}