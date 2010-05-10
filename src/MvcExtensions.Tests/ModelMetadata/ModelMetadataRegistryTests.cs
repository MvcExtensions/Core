#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;
    using System.Collections.Generic;

    using Moq;
    using Xunit;

    public class ModelMetadataRegistryTests
    {
        private readonly ModelMetadataRegistryTestDouble registry;

        public ModelMetadataRegistryTests()
        {
            registry = new ModelMetadataRegistryTestDouble();
        }

        [Fact]
        public void Configurations_should_be_empty_when_new_instance_is_created()
        {
            Assert.Empty(registry.PublicConfigurations);
        }

        [Fact]
        public void Should_be_able_to_register()
        {
            registry.Register(typeof(object), new Dictionary<string, ModelMetadataItem>());

            Assert.NotEmpty(registry.PublicConfigurations);
        }

        [Fact]
        public void Should_be_able_to_check_whether_model_type_is_registered()
        {
            registry.Register(typeof(object), new Dictionary<string, ModelMetadataItem>());

            Assert.True(registry.IsRegistered(typeof(object)));
        }

        [Fact]
        public void Should_be_able_to_check_whether_property_of_model_type_is_registered()
        {
            registry.Register(typeof(object), new Dictionary<string, ModelMetadataItem> { { "foo", new Mock<ModelMetadataItem>().Object } });

            Assert.True(registry.IsRegistered(typeof(object), "foo"));
        }

        [Fact]
        public void Should_be_able_to_get_metadata_of_model_type()
        {
            registry.Register(typeof(object), new Dictionary<string, ModelMetadataItem>());

            Assert.NotNull(registry.Matching(typeof(object)));
        }

        [Fact]
        public void Should_be_able_to_get_metadata_of_model_type_property()
        {
            registry.Register(typeof(object), new Dictionary<string, ModelMetadataItem> { { "foo", new Mock<ModelMetadataItem>().Object } });

            Assert.NotNull(registry.Matching(typeof(object), "foo"));
        }

        [Fact]
        public void Should_return_null_when_property_of_model_type_does_not_exists()
        {
            Assert.Null(registry.Matching(typeof(object), "foo"));
        }

        private sealed class ModelMetadataRegistryTestDouble : ModelMetadataRegistry
        {
            public IDictionary<Type, IDictionary<string, ModelMetadataItem>> PublicConfigurations
            {
                get
                {
                    return Configurations;
                }
            }
        }
    }
}