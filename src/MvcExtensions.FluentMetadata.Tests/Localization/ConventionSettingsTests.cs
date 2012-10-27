#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion
namespace MvcExtensions.FluentMetadata.Tests
{
    using System;
    using MvcExtensions.FluentMetadata.Tests.Resources;
    using Xunit;

    public class ConventionSettingsTests : IDisposable
    {
        public ConventionSettingsTests()
        {
            ConventionSettings.DefaultResourceType = null;
            ConventionSettings.ConventionsActive = false;
            ConventionSettings.RequireConventionAttribute = false;
        }

        [Fact]
        public void Should_activate_convenstions_when_resourceType_is_set()
        {
            // arrange
            ConventionSettings.ConventionsActive = false;

            // act
            ConventionSettings.DefaultResourceType = typeof(TestResource);
            // assert
            Assert.True(ConventionSettings.ConventionsActive);
        }

        [Fact]
        public void Should_not_deactivate_convenstions_when_resourceType_is_set_to_null()
        {
            // arrange
            ConventionSettings.ConventionsActive = true;
            // act
            ConventionSettings.DefaultResourceType = null;
            // assert
            Assert.True(ConventionSettings.ConventionsActive);
        }

        [Fact]
        public void Should_not_activate_convenstions_when_resourceType_is_set_to_null()
        {
            // arrange
            ConventionSettings.ConventionsActive = false;
            // act
            ConventionSettings.DefaultResourceType = null;
            // assert
            Assert.False(ConventionSettings.ConventionsActive);
        }

        [Fact]
        public void GetDefaultResourceType_should_return_null_if_convensions_is_not_acivated()
        {
            // arrange
            ConventionSettings.DefaultResourceType = typeof(TestResource);
            ConventionSettings.ConventionsActive = false;
            Type containerType = typeof(DummyContainer);

            // act
            var type = ConventionSettings.GetDefaultResourceType(containerType);

            // assert
            Assert.Null(type);
        }

        [Fact]
        public void GetDefaultResourceType_should_return_defaultResourceType_if_convensions_is_acivated()
        {
            // arrange
            ConventionSettings.DefaultResourceType = typeof(TestResource);
            ConventionSettings.ConventionsActive = true;
            Type containerType = typeof(DummyContainer);

            // act
            var type = ConventionSettings.GetDefaultResourceType(containerType);

            // assert
            Assert.NotNull(type);
            Assert.Equal(type, typeof(TestResource));
        }

        [Fact]
        public void GetDefaultResourceType_should_return_type_from_attributeMetadataConventions()
        {
            // arrange
            ConventionSettings.DefaultResourceType = typeof(TestResource);
            ConventionSettings.ConventionsActive = true;
            Type containerType = typeof(DummyContainerWithAttributeAndResourceType);

            // act
            var type = ConventionSettings.GetDefaultResourceType(containerType);

            // assert
            Assert.NotNull(type);
            Assert.Equal(type, typeof(TestResource2));
        }

        [Fact]
        public void GetDefaultResourceType_should_return_defaultResourceType_if_metadataConventionsAttribute_has_no_type()
        {
            // arrange
            ConventionSettings.DefaultResourceType = typeof(TestResource);
            ConventionSettings.ConventionsActive = true;
            Type containerType = typeof(DummyContainerWithEmptyAttribute);

            // act
            var type = ConventionSettings.GetDefaultResourceType(containerType);

            // assert
            Assert.NotNull(type);
            Assert.Equal(type, typeof(TestResource));
        }

        [Fact]
        public void GetDefaultResourceType_should_return_null_if_defaultResourceType_and_metadataConventionsAttribute_has_no_type()
        {
            // arrange
            ConventionSettings.DefaultResourceType = null;
            ConventionSettings.ConventionsActive = true;
            Type containerType = typeof(DummyContainerWithEmptyAttribute);

            // act
            var type = ConventionSettings.GetDefaultResourceType(containerType);

            // assert
            Assert.Null(type);
        }

        [Fact]
        public void GetDefaultResourceType_should_return_null_if_metadataConventionsAttribute_has_no_type_and_requireConventionAttribute_is_true()
        {
            // arrange
            ConventionSettings.DefaultResourceType = null;
            ConventionSettings.ConventionsActive = true;
            ConventionSettings.RequireConventionAttribute = true;
            Type containerType = typeof(DummyContainerWithEmptyAttribute);

            // act
            var type = ConventionSettings.GetDefaultResourceType(containerType);

            // assert
            Assert.Null(type);
        }


        public void Dispose()
        {
            ConventionSettings.DefaultResourceType = null;
            ConventionSettings.ConventionsActive = false;
            ConventionSettings.RequireConventionAttribute = false;
        }

        public class DummyContainer
        {
             
        }

        [MetadataConventions]
        public class DummyContainerWithEmptyAttribute
        {

        }

        [MetadataConventions(typeof(TestResource2))]
        public class DummyContainerWithAttributeAndResourceType
        {

        }
        
    }
}