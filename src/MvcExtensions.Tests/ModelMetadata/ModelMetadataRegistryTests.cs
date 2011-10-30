#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Collections.Generic;
    using Moq;
    using Xunit;

    public class ModelMetadataRegistryTests
    {
        private readonly ModelMetadataRegistry registry;

        public ModelMetadataRegistryTests()
        {
            registry = new ModelMetadataRegistry();
        }

        [Fact]
        public void Should_be_able_to_register_model()
        {
            var modelMetadata = new Mock<ModelMetadataItem>();

            registry.RegisterModel(typeof(object), modelMetadata.Object);

            Assert.Same(modelMetadata.Object, registry.GetModelMetadata(typeof(object)));
        }

        [Fact]
        public void Should_get_model_of_derived_type()
        {
            var modelMetadata = new Mock<ModelMetadataItem>();

            registry.RegisterModel(typeof(object), modelMetadata.Object);

            Assert.Same(modelMetadata.Object, registry.GetModelMetadata(typeof(string)));
        }

        [Fact]
        public void Should_get_model_of_closest_derived_type()
        {
            var grandParentMetadata = new Mock<ModelMetadataItem>();
            var parentMetadata = new Mock<ModelMetadataItem>();

            registry.RegisterModel(typeof(DummyGrandParent), grandParentMetadata.Object);
            registry.RegisterModel(typeof(DummyParent), parentMetadata.Object);

            Assert.Same(parentMetadata.Object, registry.GetModelMetadata(typeof(Dummy)));
        }

        [Fact]
        public void Should_be_able_to_register_model_properties()
        {
            var properties = new Dictionary<string, ModelMetadataItem>
                                 {
                                     { "foo", new Mock<ModelMetadataItem>().Object },
                                     { "bar", new Mock<ModelMetadataItem>().Object }
                                 };

            registry.RegisterModelProperties(typeof(object), properties);

            IDictionary<string, ModelMetadataItem> returndedProperties = registry.GetModelPropertiesMetadata(typeof(object));

            Assert.True(returndedProperties.ContainsKey("foo"));
            Assert.True(returndedProperties.ContainsKey("bar"));
        }

        [Fact]
        public void Should_be_able_to_get_model_property()
        {
            var modelMetadata = new Mock<ModelMetadataItem>();

            var properties = new Dictionary<string, ModelMetadataItem>
                                 {
                                     { "foo", modelMetadata.Object }
                                 };

            registry.RegisterModelProperties(typeof(object), properties);

            ModelMetadataItem returnedeMetadata = registry.GetModelPropertyMetadata(typeof(object), "foo");

            Assert.Same(modelMetadata.Object, returnedeMetadata);
        }

        [Fact]
        public void GetModelPropertyMetadata_should_return_null_when_property_is_not_registered()
        {
            Assert.Null(registry.GetModelPropertyMetadata(typeof(object), "foo"));
        }

        #region Nested type: Dummy
        private class Dummy : DummyParent
        {
        }
        #endregion

        #region Nested type: DummyGrandParent
        private class DummyGrandParent
        {
        }
        #endregion

        #region Nested type: DummyParent
        private class DummyParent : DummyGrandParent
        {
        }
        #endregion
    }
}