#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using MvcExtensions.FluentMetadata.Tests.Resources;
    using Xunit;

    public class FluentConfigurationLocalizationTests : LocalizationTestsBase
    {
        private readonly LocalizationModelConfiguration metadataConfiguration;
        private readonly LocalizationModel model;

        public FluentConfigurationLocalizationTests()
        {
            model = new LocalizationModel();
            metadataConfiguration = new LocalizationModelConfiguration();
        }

        [Fact]
        public void RequiredValidation_should_use_common_convention_if_it_is_found_but_convension_for_type_does_not_exist()
        {
            const string PropertyName = "LocalizedByCommonPattern";
            ModelMetadataItem item = metadataConfiguration.Configurations[PropertyName];

            var errorMessage = GetErrorMessageForFluentlyConfigiguredItem(model, PropertyName, () => item, item.GetValidation<ModelValidationMetadata>());

            Assert.Equal(TestResource.Validation_Required, errorMessage);
        }

        [Fact]
        public void RequiredValidation_should_use_modeltype_propertyname_attributename_convention_if_it_exists()
        {
            const string PropertyName = "LocalizedByKey";
            ModelMetadataItem item = metadataConfiguration.Configurations[PropertyName];

            var errorMessage = GetErrorMessageForFluentlyConfigiguredItem(model, PropertyName, () => item, item.GetValidation<ModelValidationMetadata>());

            Assert.Equal(TestResource.LocalizationModel_LocalizedByKey_Required, errorMessage);
        }

        [Fact]
        public void RequiredValidation_should_always_use_user_defined_message()
        {
            const string PropertyName = "LocalizedByKey";
            const string UserMessage = "user message";

            var builder = new ModelMetadataItemBuilder<string>(new ModelMetadataItem());
            builder.Required(() => UserMessage);
            var metadata = builder.Item.GetValidation<RequiredValidationMetadata>();

            var errorMessage = GetErrorMessageForFluentlyConfigiguredItem(model, PropertyName, () => builder.Item, metadata);

            Assert.Equal(UserMessage, errorMessage);
        }

        [Fact]
        public void RequiredValidation_should_always_use_user_defined_message_from_resource()
        {
            const string PropertyName = "LocalizedByKeyWithCustomMessageForPropertyFromRes";
            ModelMetadataItem item = metadataConfiguration.Configurations[PropertyName];

            var errorMessage = GetErrorMessageForFluentlyConfigiguredItem(model, PropertyName, () => item, item.GetValidation<RequiredValidationMetadata>());

            Assert.Equal(TestResource.CustomMessageForProperty, errorMessage);
        }

        [Fact]
        public void RequiredValidation_should_use_specified_resource_and_resourcename_should_be_set_by_convensions()
        {
            const string PropertyName = "PropertyToTestResTypePartialMetadata";
            ModelMetadataItem item = metadataConfiguration.Configurations[PropertyName];

            var errorMessage = GetErrorMessageForFluentlyConfigiguredItem(model, PropertyName, () => item, item.GetValidation<RequiredValidationMetadata>());

            Assert.Equal(TestResource2.AttrLocalizationModel_PropertyToTestResTypePartialMetadata_Required, errorMessage);
        }

        [Fact]
        public void RequiredValidation_should_use_specified_resourcename_and_resourcetype_should_be_set_from_global_resource()
        {
            const string PropertyName = "PropertyToTestResNamePartialMetadata";
            ModelMetadataItem item = metadataConfiguration.Configurations[PropertyName];

            var errorMessage = GetErrorMessageForFluentlyConfigiguredItem(model, PropertyName, () => item, item.GetValidation<RequiredValidationMetadata>());

            Assert.Equal(TestResource.PropertyToTestResNamePartialMetadata, errorMessage);
        }

        public class LocalizationModel
        {
            public string LocalizedByCommonPattern { get; set; }
            public string LocalizedByKey { get; set; }
            public string LocalizedByKeyWithCustomMessageForPropertyFromRes { get; set; }
            public string PropertyToTestResTypePartialMetadata { get; set; }
            public string PropertyToTestResNamePartialMetadata { get; set; }
        }

        public class LocalizationModelConfiguration : ModelMetadataConfiguration<LocalizationModel>
        {
            public LocalizationModelConfiguration()
            {
                Configure(x => x.LocalizedByKey).Required();
                Configure(x => x.LocalizedByCommonPattern).Required();
                Configure(x => x.LocalizedByKeyWithCustomMessageForPropertyFromRes).Required(typeof(TestResource), "CustomMessageForProperty");
                Configure(x => x.PropertyToTestResTypePartialMetadata).Required(typeof(TestResource2), null);
                Configure(x => x.PropertyToTestResNamePartialMetadata).Required(null, "PropertyToTestResNamePartialMetadata");
            }
        }
    }
}
