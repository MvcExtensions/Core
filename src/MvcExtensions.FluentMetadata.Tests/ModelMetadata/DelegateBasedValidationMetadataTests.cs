#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using System.Linq;
    using System.Web.Mvc;
    using Xunit;

    public class DelegateBasedValidationMetadataTests : ValidationMetadataTestsBase
    {
        private readonly ModelMetadataItemBuilder<string> builder;
        private readonly ModelMetadataItem item;

        public DelegateBasedValidationMetadataTests()
        {
            item = new ModelMetadataItem();
            builder = new ModelMetadataItemBuilder<string>(item);
        }

        [Fact]
        public void Should_be_able_to_create_delegate_based_validator()
        {
            // act
            var validations = builder.Validate<DelegateTestModel>(model => model.Property1 != "1").Item.Validations;
            var validator = validations.First().CreateValidator(CreateMetadata(), new ControllerContext());

            // assert
            Assert.NotNull(validator);
        }

        [Fact]
        public void Should_be_able_to_set_error_message_for_delegate_based_validator()
        {
            // arrange
            const string PropertyValue = "1";
            const string ErrorMsg = "error";
            var testModel = new DelegateTestModel { Property1 = PropertyValue };
            var metadata = CreateMetadata("Property1");
            metadata.Model = PropertyValue;

            // act
            var validations = builder.Validate<DelegateTestModel>(model => model.Property1 != PropertyValue, () => ErrorMsg).Item.Validations;
            var validator = validations.First().CreateValidator(metadata, new ControllerContext());

            // assert
            Assert.NotEmpty(validations);
            Assert.IsType<DelegateBasedModelMetadata.DelegateBasedModelValidator>(validator);

            var modelValidationResult = validator.Validate(testModel).First();
            Assert.Equal(ErrorMsg, modelValidationResult.Message);
        }

        [Fact]
        public void Should_be_able_to_create_two_delegate_based_validators()
        {
            // act
            builder.Validate<DelegateTestModel>(model => model.Property1 != "1").Validate<DelegateTestModel>(model => model.Property1 != "2");

            var metadata = (DelegateBasedModelMetadata)item.Validations.First();
            var validator = (DelegateBasedModelMetadata.DelegateBasedModelValidator)metadata.CreateValidator(CreateMetadata(), new ControllerContext());

            // assert
            Assert.NotNull(validator);
            Assert.IsType<DelegateBasedModelMetadata.DelegateBasedModelValidator>(validator);
            Assert.Equal(2, metadata.GetValidatorsCount());
        }

        [Fact]
        public void Should_be_able_to_validate_with_two_delegate_based_validators()
        {
            // arrange
            var extendedModelMetadata = CreateMetadata("Property1");
            builder.Validate<DelegateTestModel>(model => model.Property1 != "1", "1 error").Validate<DelegateTestModel>(model => model.Property1 != "2", "2 error");

            var metadata = (DelegateBasedModelMetadata)item.Validations.First();
            var validator = (DelegateBasedModelMetadata.DelegateBasedModelValidator)metadata.CreateValidator(extendedModelMetadata, new ControllerContext());

            Assert.NotNull(validator);
            Assert.IsType<DelegateBasedModelMetadata.DelegateBasedModelValidator>(validator);
            Assert.Equal(2, metadata.GetValidatorsCount());

            foreach (string valueToValidate in new[] { "1", "2", "3" })
            {
                var delegateTestModel = new DelegateTestModel { Property1 = valueToValidate };
                extendedModelMetadata.Model = valueToValidate;

                // act
                var result = validator.Validate(delegateTestModel).FirstOrDefault();

                // assert
                if (valueToValidate == "3")
                {
                    Assert.Null(result);
                }
                else
                {
                    Assert.NotNull(result);
                    Assert.Equal(valueToValidate + " error", result.Message);
                }
            }
        }

        #region Tests data

        public class DelegateTestModel
        {
            public string Property1 { get; set; }
        }
        #endregion
    }
}