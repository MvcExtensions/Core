#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Xunit;

    public class CustomValidationMetadataTests : ValidationMetadataTestsBase
    {
        private readonly ModelMetadataItemBuilder<string> builder;
        private readonly ModelMetadataItem item;

        public CustomValidationMetadataTests()
        {
            item = new ModelMetadataItem();
            builder = new ModelMetadataItemBuilder<string>(item);
        }

        [Fact]
        public void Should_be_able_to_create_two_custom_validators()
        {
            // act
            builder.ValidateBy<DummyModelValidatorAttribute>().ValidateBy<DummyModelValidator2Attribute>();


            var validator = item.Validations.First().CreateValidator(CreateMetadata(), new ControllerContext());
            var validator2 = item.Validations.Skip(1).First().CreateValidator(CreateMetadata(), new ControllerContext());

            // assert
            Assert.NotNull(validator);
            Assert.IsType<DataAnnotationsModelValidator<DummyModelValidatorAttribute>>(validator);

            Assert.NotNull(validator2);
            Assert.IsType<DataAnnotationsModelValidator<DummyModelValidator2Attribute>>(validator2);
        }

        [Fact]
        public void Should_be_able_to_create_validator()
        {
            // act
            var validator = builder.ValidateBy<DummyModelValidatorAttribute>().Item.Validations.First().CreateValidator(CreateMetadata(), new ControllerContext());

            // assert
            Assert.NotNull(validator);
        }

        [Fact]
        public void Should_be_able_to_register_validator()
        {
            // act
            var validations = builder.ValidateBy<DummyModelValidatorAttribute>().Item.Validations;

            // assert
            Assert.NotEmpty(validations);
            Assert.IsType<CustomValidationMetadata<DummyModelValidatorAttribute>>(validations.First());
        }

        [Fact]
        public void Should_be_able_to_create_validator_manully()
        {
            var metadata = CreateMetadata();
            var context = new ControllerContext();

            // act
            var validator = builder.ValidateBy(new DummyModelValidatorAttribute()).Item.Validations.First().CreateValidator(metadata, context);

            // assert
            Assert.NotNull(validator);
        }

        [Fact]
        public void Should_be_able_to_create_validator_via_user_factory()
        {
            var metadata = CreateMetadata();
            var context = new ControllerContext();

            // act
            var validator = builder.ValidateBy(() => new DummyModelValidatorAttribute()).Item.Validations.First().CreateValidator(metadata, context);

            // assert
            Assert.NotNull(validator);
        }

        [Fact]
        public void Should_be_able_to_create_and_configure_validator()
        {
            // arrange
            var metadata = CreateMetadata("DummyProp");
            var context = new ControllerContext();
            const int DummyPropValue = 10;

            // act
            var itemBuilder = builder.ValidateBy<DummyConfigureModelValidatorAttribute>(v =>v.DummyProp = DummyPropValue);

            var modelValidationMetadata = itemBuilder.Item.Validations.First();
            var validator = (DataAnnotationsModelValidator)modelValidationMetadata.CreateValidator(metadata, context);

            // assert
            Assert.NotNull(validator);

            var propertyInfo = validator.GetType().GetProperty("Attribute", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            var attr = (DummyConfigureModelValidatorAttribute)propertyInfo.GetValue(validator, null);
            Assert.NotNull(attr);
            Assert.Equal(attr.DummyProp, DummyPropValue);
        }

        [Fact]
        public void Should_get_a_valid_error_message()
        {
            // arrange
            const string PropertyName = "SomePropertyName";
            const string Message = "The {0} with value {1} is invalid.";
            const int PropertyValue = 10;

            var metadata = CreateMetadata(PropertyName);
            var context = new ControllerContext();

            var itemBuilder = builder.ValidateBy<DummyConfigureModelValidatorAttribute>(
                v =>
                {
                    v.DummyProp = PropertyValue;
                    v.ErrorMessage = Message;
                });

            // act
            var modelValidationMetadata = itemBuilder.Item.Validations.First();
            var validator = modelValidationMetadata.CreateValidator(metadata, context);
            var result = validator.Validate(new object()).First();

            // assert
            Assert.Equal(result.Message, string.Format(Message, PropertyName, PropertyValue));
        }

        #region Tests data

        public class DummyModelValidatorAttribute : CustomValidatorAttribute
        {
            public override bool IsValid(object value, object container)
            {
                return true;
            }
        }

        public class DummyModelValidator2Attribute : CustomValidatorAttribute
        {
            public override bool IsValid(object value, object container)
            {
                return true;
            }
        }

        public class DummyConfigureModelValidatorAttribute : CustomValidatorAttribute
        {
            public int DummyProp { get; set; }

            public override bool IsValid(object value, object container)
            {
                return false;
            }

            public override string FormatErrorMessage(string name)
            {
                return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, DummyProp);
            }
        }
        #endregion
    }
}