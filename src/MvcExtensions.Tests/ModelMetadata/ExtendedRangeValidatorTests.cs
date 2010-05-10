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

    public class ExtendedRangeValidatorTests : ValidatorTestsBase
    {
        [Fact]
        public void Should_throw_exception_when_validation_metadata_is_not_range_validation_meta_data()
        {
            Assert.Throws<InvalidCastException>(() => new ExtendedRangeValidator<int>(CreateModelMetadataWithModel(), new ControllerContext(), new Mock<ModelValidationMetadata>().Object));
        }

        [Fact]
        public void Minimum_and_maximum_should_be_same_as_metadata()
        {
            var validator = new ExtendedRangeValidatorTestDouble<int>(CreateModelMetadataWithModel(), new ControllerContext(), new RangeValidationMetadata<int> { Minimum = 1, Maximum = 7 });

            Assert.Equal(1, validator.PublicAttribute.Minimum);
            Assert.Equal(7, validator.PublicAttribute.Maximum);
        }

        [Fact]
        public void Should_be_able_to_get_client_rules()
        {
            var validator = new ExtendedRangeValidatorTestDouble<int>(CreateModelMetadataWithModel(), new ControllerContext(), new RangeValidationMetadata<int> { Minimum = 1, Maximum = 7, ErrorMessage = "Value must be between 1 t0 7." });

            var rules = validator.GetClientValidationRules();

            Assert.NotEmpty(rules);
            Assert.IsType<ModelClientValidationRangeRule>(rules.First());
        }

        private sealed class ExtendedRangeValidatorTestDouble<TValueType> : ExtendedRangeValidator<TValueType>
        {
            public ExtendedRangeValidatorTestDouble(ModelMetadata metadata, ControllerContext controllerContext, ModelValidationMetadata validationMetadata) : base(metadata, controllerContext, validationMetadata)
            {
            }

            public RangeAttribute PublicAttribute
            {
                get
                {
                    return Attribute;
                }
            }
        }
    }
}