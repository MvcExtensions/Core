#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion
#if !MVC_3
namespace MvcExtensions.FluentMetadata.Tests.WebApi
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http.Validation;
    using System.Web.Http.Validation.Validators;
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

            var validator = item.Validations.First().CreateWebApiValidator(GetProviders());
            var validator2 = item.Validations.Skip(1).First().CreateWebApiValidator(GetProviders());

            // assert
            Assert.NotNull(validator);
            Assert.IsType<DataAnnotationsModelValidator>(validator);
            Assert.NotNull(GetAttributeFromValidator<DummyModelValidatorAttribute>(validator));

            Assert.NotNull(validator2);
            Assert.IsType<DataAnnotationsModelValidator>(validator2);
            Assert.NotNull(GetAttributeFromValidator<DummyModelValidator2Attribute>(validator2));
        }

        [Fact]
        public void Should_be_able_to_create_validator()
        {
            // act
            var metadata = builder.ValidateBy<DummyModelValidatorAttribute>().Item.Validations.First();
            var validator = metadata.CreateWebApiValidator(GetProviders());

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
        public void Should_be_able_to_create_validator_via_user_factory()
        {
            // act
            var metadata = builder.ValidateBy(() => new DummyModelValidatorAttribute()).Item.Validations.First();
            var validator = metadata.CreateWebApiValidator(GetProviders());

            // assert
            Assert.NotNull(validator);
        }

        [Fact]
        public void Should_be_able_to_create_and_configure_validator()
        {
            // arrange
            const int DummyPropValue = 10;

            // act
            var itemBuilder = builder.ValidateBy<DummyConfigureModelValidatorAttribute>(v =>v.DummyProp = DummyPropValue);

            var modelValidationMetadata = itemBuilder.Item.Validations.First();
            var validator = modelValidationMetadata.CreateWebApiValidator(GetProviders());

            // assert
            Assert.NotNull(validator);

            var attr = GetAttributeFromValidator<DummyConfigureModelValidatorAttribute>(validator);
            Assert.NotNull(attr);
            Assert.Equal(attr.DummyProp, DummyPropValue);
        }

        [Fact]
        public void Should_get_a_valid_error_message()
        {
            // arrange
            const string PropertyName = "SomePropertyName";
            const string Message = "The {0} is invalid.";
            var metadata = CreateMetadata(PropertyName);

            var itemBuilder = builder.ValidateBy(() => new DummyConfigureModelValidatorAttribute(), () => Message);

            // act
            var modelValidationMetadata = itemBuilder.Item.Validations.First();
            var validator = modelValidationMetadata.CreateWebApiValidator(GetProviders());
            var result = validator.Validate(metadata, new object()).First();

            // assert
            Assert.Equal(result.Message, string.Format(Message, PropertyName));
        }

        [Fact]
        public void Should_get_a_valid_error_message_with_custom_configuration()
        {
            // arrange
            const string PropertyName = "SomePropertyName";
            const string Message = "The {0} with value {1} is invalid.";
            const int PropertyValue = 10;

            var metadata = CreateMetadata(PropertyName);

            var itemBuilder = builder.ValidateBy<DummyConfigureModelValidatorAttribute>(
                v =>
                {
                    v.DummyProp = PropertyValue;
                    v.ErrorMessage = Message;
                });

            // act
            var modelValidationMetadata = itemBuilder.Item.Validations.First();
            var validator = modelValidationMetadata.CreateWebApiValidator(GetProviders());
            var result = validator.Validate(metadata, new object()).First();

            // assert
            Assert.Equal(result.Message, string.Format(Message, PropertyName, PropertyValue));
        }

        private static T GetAttributeFromValidator<T>(ModelValidator validator) where  T : ValidationAttribute
        {
            var propertyInfo = validator.GetType().GetProperty("Attribute", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            if (propertyInfo == null)
            {
                return null;
            }

            return (T)propertyInfo.GetValue(validator, null);
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

            /// <summary>
            /// Custom error formating - adds attribute's property value (of DummyProp)
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public override string FormatErrorMessage(string name)
            {
                return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, DummyProp);
            }
        }
        #endregion
    }
}
#endif