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

    public class LocalizationConventionsTests : IDisposable
    {
        public LocalizationConventionsTests()
        {
            LocalizationConventions.DefaultResourceType = null;
            LocalizationConventions.Enabled = false;
            LocalizationConventions.RequireConventionAttribute = false;
        }

        [Fact]
        public void Should_activate_convenstions_when_resourceType_is_set()
        {
            // arrange
            LocalizationConventions.Enabled = false;

            // act
            LocalizationConventions.DefaultResourceType = typeof(TestResource);
            // assert
            Assert.True(LocalizationConventions.Enabled);
        }

        [Fact]
        public void Should_not_deactivate_convenstions_when_resourceType_is_set_to_null()
        {
            // arrange
            LocalizationConventions.Enabled = true;
            // act
            LocalizationConventions.DefaultResourceType = null;
            // assert
            Assert.True(LocalizationConventions.Enabled);
        }

        [Fact]
        public void Should_not_activate_convenstions_when_resourceType_is_set_to_null()
        {
            // arrange
            LocalizationConventions.Enabled = false;
            // act
            LocalizationConventions.DefaultResourceType = null;
            // assert
            Assert.False(LocalizationConventions.Enabled);
        }

        [Fact]
        public void GetDefaultResourceType_should_return_null_if_convensions_is_not_acivated()
        {
            // arrange
            LocalizationConventions.DefaultResourceType = typeof(TestResource);
            LocalizationConventions.Enabled = false;
            Type containerType = typeof(DummyContainer);

            // act
            var type = LocalizationConventions.GetDefaultResourceType(containerType);

            // assert
            Assert.Null(type);
        }

        [Fact]
        public void GetDefaultResourceType_should_return_defaultResourceType_if_convensions_is_acivated()
        {
            // arrange
            LocalizationConventions.DefaultResourceType = typeof(TestResource);
            LocalizationConventions.Enabled = true;
            Type containerType = typeof(DummyContainer);

            // act
            var type = LocalizationConventions.GetDefaultResourceType(containerType);

            // assert
            Assert.NotNull(type);
            Assert.Equal(type, typeof(TestResource));
        }

        [Fact]
        public void GetDefaultResourceType_should_return_type_from_attributeMetadataConventions()
        {
            // arrange
            LocalizationConventions.DefaultResourceType = typeof(TestResource);
            LocalizationConventions.Enabled = true;
            Type containerType = typeof(DummyContainerWithAttributeAndResourceType);

            // act
            var type = LocalizationConventions.GetDefaultResourceType(containerType);

            // assert
            Assert.NotNull(type);
            Assert.Equal(type, typeof(TestResource2));
        }

        [Fact]
        public void GetDefaultResourceType_should_return_defaultResourceType_if_metadataConventionsAttribute_has_no_type()
        {
            // arrange
            LocalizationConventions.DefaultResourceType = typeof(TestResource);
            LocalizationConventions.Enabled = true;
            Type containerType = typeof(DummyContainerWithEmptyAttribute);

            // act
            var type = LocalizationConventions.GetDefaultResourceType(containerType);

            // assert
            Assert.NotNull(type);
            Assert.Equal(type, typeof(TestResource));
        }

        [Fact]
        public void GetDefaultResourceType_should_return_null_if_defaultResourceType_and_metadataConventionsAttribute_has_no_type()
        {
            // arrange
            LocalizationConventions.DefaultResourceType = null;
            LocalizationConventions.Enabled = true;
            Type containerType = typeof(DummyContainerWithEmptyAttribute);

            // act
            var type = LocalizationConventions.GetDefaultResourceType(containerType);

            // assert
            Assert.Null(type);
        }

        [Fact]
        public void GetDefaultResourceType_should_return_null_if_metadataConventionsAttribute_has_no_type_and_requireConventionAttribute_is_true()
        {
            // arrange
            LocalizationConventions.DefaultResourceType = null;
            LocalizationConventions.Enabled = true;
            LocalizationConventions.RequireConventionAttribute = true;
            Type containerType = typeof(DummyContainerWithEmptyAttribute);

            // act
            var type = LocalizationConventions.GetDefaultResourceType(containerType);

            // assert
            Assert.Null(type);
        }


        public void Dispose()
        {
            LocalizationConventions.DefaultResourceType = null;
            LocalizationConventions.Enabled = false;
            LocalizationConventions.RequireConventionAttribute = false;
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