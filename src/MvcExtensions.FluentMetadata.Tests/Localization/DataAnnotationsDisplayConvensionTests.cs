#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion
namespace MvcExtensions.FluentMetadata.Tests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using MvcExtensions.FluentMetadata.Tests.Resources;
    using Xunit;

    public class DataAnnotationsDisplayConvensionTests : IDisposable
    {
        private readonly ConventionalDataAnnotationsModelMetadataProvider provider;

        public DataAnnotationsDisplayConvensionTests()
        {
            ConventionSettings.ConventionsActive = true;
            ConventionSettings.RequireConventionAttribute = false;
            provider = new ConventionalDataAnnotationsModelMetadataProvider();
        }

        public void Dispose()
        {
            ConventionSettings.DefaultResourceType = null;
            ConventionSettings.ConventionsActive = false;
            ConventionSettings.RequireConventionAttribute = false;
        }

        [Fact]
        public void Should_return_specified_name_for_property_with_display_attribute_having_name_and_not_matching_resource()
        {
            // arrange
            var model = new AttrDummyDisplayModel { PropertyWithDisplayAttributeHavingName = "HelloWorld" };

            // act
            var metadata = provider.GetMetadataForProperty(() => model, model.GetType(), "PropertyWithDisplayAttributeHavingName");

            // assert
            Assert.Equal("Property with DisplayAttribute with Name", metadata.DisplayName);
        }

        [Fact]
        public void Should_return_resource_value_for_property_with_display_attribute_matching_resource_key()
        {
            // arrange
            var model = new AttrDummyDisplayModel { PropertyWithDisplayAttributeMatchingResourceKey = "HelloWorld" };

            // act
            var metadata = provider.GetMetadataForProperty(() => model, model.GetType(), "PropertyWithDisplayAttributeMatchingResourceKey");

            // assert
            Assert.Equal(TestResource.AttrDummyDisplayModel_PropertyWithDisplayAttributeMatchingResourceKey, metadata.DisplayName);
        }

        [Fact]
        public void Should_return_resource_value_for_property_with_display_attribute_containing_name_and_resource_type()
        {
            // arrange
            var model = new AttrDummyDisplayModel { PropertyWithFullDisplayAttribute = "HelloWorld" };

            // act
            var metadata = provider.GetMetadataForProperty(() => model, model.GetType(), "PropertyWithFullDisplayAttribute");

            // assert
            Assert.Equal(TestResource2.PropertyWithFullDisplayAttribute, metadata.DisplayName);
        }

        [Fact]
        public void Should_match_resource_by_class_and_property_names_if_it_exists_in_resource()
        {
            // arrange
            var model = new AttrDummyDisplayModel { PropertyWithMatchingResourceKeyByClassAndName = "HelloWorld" };

            // act
            var metadata = provider.GetMetadataForProperty(() => model, model.GetType(), "PropertyWithMatchingResourceKeyByClassAndName");

            // assert
            Assert.Equal(TestResource.AttrDummyDisplayModel_PropertyWithMatchingResourceKeyByClassAndName, metadata.DisplayName);
        }

        [Fact]
        public void Should_match_resource_by_property_name_if_it_exists_in_resource()
        {
            // arrange
            var model = new AttrDummyDisplayModel { PropertyWithMatchingResourceKey = "HelloWorld" };

            // act
            var metadata = provider.GetMetadataForProperty(() => model, model.GetType(), "PropertyWithMatchingResourceKey");

            // assert
            Assert.Equal(TestResource.PropertyWithMatchingResourceKey, metadata.DisplayName);
        }

        [Fact]
        public void Prompt_should_match_resource_by_class_and_property_names_if_it_exists_in_resource()
        {
            // arrange
            var model = new AttrDummyDisplayModel { PropertyWithMatchingResourceKey = "HelloWorld" };

            // act
            var metadata = provider.GetMetadataForProperty(() => model, model.GetType(), "PropertyWithMatchingResourceKey");

            // assert
            Assert.Equal(TestResource.PropertyWithMatchingResourceKey_Prompt, metadata.Watermark);
        }
        
        [Fact]
        public void Prompt_should_match_resource_by_property_name_if_it_exists_in_resource()
        {
            // arrange
            var model = new AttrDummyDisplayModel { PropertyWithMatchingResourceKeyByClassAndName = "HelloWorld" };

            // act
            var metadata = provider.GetMetadataForProperty(() => model, model.GetType(), "PropertyWithMatchingResourceKeyByClassAndName");

            // assert
            Assert.Equal(TestResource.AttrDummyDisplayModel_PropertyWithMatchingResourceKeyByClassAndName_Prompt, metadata.Watermark);
        }

        /*[Fact]
        public void Should_returns_spitted_display_name()
        {
            // arrange
            var model = new { FirstName = "HelloWorld" };

            // act
            var metadata = provider.GetMetadataForProperty(() => model, model.GetType(), "FirstName");

            // assert
            Assert.Equal("First Name", metadata.DisplayName);
        }*/

        // ReSharper disable LocalizableElement
        // ReSharper disable UnusedAutoPropertyAccessor.Global

        [MetadataConventions(typeof(TestResource))]
        public class AttrDummyDisplayModel
        {
            [Display(Name = "Property with DisplayAttribute with Name")]
            public string PropertyWithDisplayAttributeHavingName { get; set; }


            [Display(Name = "AttrDummyDisplayModel_PropertyWithDisplayAttributeMatchingResourceKey")]
            public string PropertyWithDisplayAttributeMatchingResourceKey { get; set; }

            [Display(Name = "PropertyWithFullDisplayAttribute", ResourceType = typeof(TestResource2))]
            public string PropertyWithFullDisplayAttribute { get; set; }

            public string PropertyWithMatchingResourceKeyByClassAndName { get; set; }


            public string PropertyWithMatchingResourceKey { get; set; }
        }

        // ReSharper restore UnusedAutoPropertyAccessor.Global
        // ReSharper restore LocalizableElement
    }
}