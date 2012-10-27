#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using System.ComponentModel.DataAnnotations;
    using MvcExtensions.FluentMetadata.Tests.Resources;
    using Xunit;

    public class DataAnnotationLocalizationTests : LocalizationTestsBase
    {
        [Fact]
        public void RequiredValidation_should_return_resource_value_if_resource_is_set_and_custom_error_message_is_not_defined()
        {
            // arrange
            const string PropertyName = "AttrLocalizedByKey";
            var model = new AttrLocalizationModel();

            var errorMessage = GetErrorMessageForAttributeBasedConfigiguredItem(model, PropertyName);

            // assert
            Assert.Equal(TestResource.AttrLocalizationModel_AttrLocalizedByKey_Required, errorMessage);
        }

        [Fact]
        public void RequiredValidation_should_use_common_convention_if_it_is_found_but_convension_for_type_does_not_exist()
        {
            // arrange
            const string PropertyName = "AttrLocalizedByCommonPattern";
            var model = new AttrLocalizationModel();

            var errorMessage = GetErrorMessageForAttributeBasedConfigiguredItem(model, PropertyName);

            // assert
            Assert.Equal(TestResource.Validation_Required, errorMessage);
        }

        [Fact]
        public void RequiredValidation_should_always_use_user_defined_message()
        {
            const string PropertyName = "AttrLocalizedByKeyWithUserMessage";
            const string UserMessage = "user message";

            var model = new AttrLocalizationModel();

            var errorMessage = GetErrorMessageForAttributeBasedConfigiguredItem(model, PropertyName);

            Assert.Equal(UserMessage, errorMessage);
        }
        
        [Fact]
        public void RequiredValidation_should_always_use_user_defined_message_from_resource()
        {
            const string PropertyName = "AttrLocalizedByKeyWithCustomMessageForPropertyFromRes";

            var model = new AttrLocalizationModel();

            var errorMessage = GetErrorMessageForAttributeBasedConfigiguredItem(model, PropertyName);

            Assert.Equal(TestResource.CustomMessageForProperty, errorMessage);
        } 
        
        [Fact]
        public void RequiredValidation_should_use_specified_resource_and_resourcename_should_be_set_by_convensions()
        {
            const string PropertyName = "PropertyToTestResTypePartialMetadata";
            var model = new AttrLocalizationModel();

            var errorMessage = GetErrorMessageForAttributeBasedConfigiguredItem(model, PropertyName);

            Assert.Equal(TestResource2.AttrLocalizationModel_PropertyToTestResTypePartialMetadata_Required, errorMessage);
        }

        [Fact]
        public void RequiredValidation_should_use_specified_resourcename_and_resourcetype_should_be_set_from_global_resource()
        {
            const string PropertyName = "PropertyToTestResNamePartialMetadata";
            var model = new AttrLocalizationModel();

            var errorMessage = GetErrorMessageForAttributeBasedConfigiguredItem(model, PropertyName);

            Assert.Equal(TestResource.PropertyToTestResNamePartialMetadata, errorMessage);
        }

        [Fact]
        public void Should_split_camel_case_property_name()
        {
            var model = new { PropertyName = "Hello World!" };
            var provider = new ConventionalDataAnnotationsModelMetadataProvider();

            var metadata = provider.GetMetadataForProperty(() => model, model.GetType(), "PropertyName");

            Assert.Equal("Property Name", metadata.DisplayName);
        }

        [Fact]
        public void Should_not_split_single_word_property_name()
        {
            var model = new { Name = "Hello World!" };
            var provider = new ConventionalDataAnnotationsModelMetadataProvider();

            var metadata = provider.GetMetadataForProperty(() => model, model.GetType(), "Name");

            Assert.Equal("Name", metadata.DisplayName);
        }

        public class AttrLocalizationModel
        {
            [Required]
            public string AttrLocalizedByKey { get; set; }
            [Required]
            public string AttrLocalizedByCommonPattern { get; set; }
            [Required(ErrorMessage = "user message")]
            public string AttrLocalizedByKeyWithUserMessage { get; set; }
            [Required(ErrorMessageResourceName = "CustomMessageForProperty", ErrorMessageResourceType = typeof(TestResource))]
            public string AttrLocalizedByKeyWithCustomMessageForPropertyFromRes { get; set; }

            [Required(ErrorMessageResourceType = typeof(TestResource2))]
            public string PropertyToTestResTypePartialMetadata { get; set; }

            [Required(ErrorMessageResourceName = "PropertyToTestResNamePartialMetadata")]
            public string PropertyToTestResNamePartialMetadata { get; set; }
        }

    }
}
