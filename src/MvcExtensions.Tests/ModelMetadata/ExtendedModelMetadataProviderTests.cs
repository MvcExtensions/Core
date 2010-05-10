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
    using System.Linq;

    using Moq;
    using Xunit;
    using Xunit.Extensions;

    public class ExtendedModelMetadataProviderTests
    {
        private readonly Mock<IModelMetadataRegistry> registry;
        private readonly ExtendedModelMetadataProvider provider;

        public ExtendedModelMetadataProviderTests()
        {
            registry = new Mock<IModelMetadataRegistry>();
            provider = new ExtendedModelMetadataProvider(registry.Object);
        }

        [Fact]
        public void GetMetadataForProperties_should_return_matching_metadata()
        {
            registry.Setup(r => r.Matching(It.IsAny<Type>())).Returns(new Dictionary<string, ModelMetadataItem>());

            var metadata = provider.GetMetadataForProperties(new DummyObject(), typeof(DummyObject));

            Assert.Equal(2, metadata.Count());
        }

        [Fact]
        public void GetMetadataForProperty_should_return_mathing_metadata()
        {
            registry.Setup(r => r.Matching(It.IsAny<Type>(), It.IsAny<string>())).Returns(new Mock<ModelMetadataItem>().Object);

            var metadata = provider.GetMetadataForProperty(() => new DummyObject(), typeof(DummyObject), "Property1");

            Assert.NotNull(metadata);
        }

        [Fact]
        public void GetMetadataForProperty_should_throw_exception_for_invalid_property()
        {
            Assert.Throws<ArgumentException>(() => provider.GetMetadataForProperty(() => new DummyObject(), typeof(DummyObject), "Property3"));
        }

        [Fact]
        public void GetMetadataForType_should_always_return_metadata()
        {
            Assert.NotNull(provider.GetMetadataForType(() => new DummyObject(), typeof(DummyObject)));
        }

        [Theory]
        [InlineData(false, false, false, false, false)]
        [InlineData(true, true, true, true, true)]
        public void Should_return_metadata_with_same_value_as_model_meta_data_item(bool hideSurroundingHtml, bool isReadOnly, bool isRequired, bool showForEdit, bool applyFormatInEditMode)
        {
            var metadataItem = new StringMetadataItem { HideSurroundingHtml = hideSurroundingHtml, IsReadOnly = isReadOnly, IsRequired = isRequired, ShowForEdit = showForEdit, ApplyFormatInEditMode = applyFormatInEditMode };

            registry.Setup(r => r.Matching(It.IsAny<Type>(), It.IsAny<string>())).Returns(metadataItem);

            var metadata = provider.GetMetadataForProperty(() => new DummyObject(), typeof(DummyObject), "Property1");

            Assert.Equal(metadataItem.HideSurroundingHtml, metadata.HideSurroundingHtml);
            Assert.Equal(metadataItem.IsReadOnly, metadata.IsReadOnly);
            Assert.Equal(metadataItem.IsRequired, metadata.IsRequired);
            Assert.Equal(metadataItem.ShowForEdit, metadata.ShowForEdit);
            Assert.Equal(metadataItem.EditFormat, metadata.EditFormatString);
        }

        [Fact]
        public void IsRegister_with_only_type_should_be_same_as_registry()
        {
            registry.Setup(r => r.IsRegistered(typeof(DummyObject))).Returns(true);

            Assert.True(provider.IsRegistered(typeof(DummyObject)));
        }

        [Fact]
        public void IsRegister_with_type_and_property_name_should_be_same_as_registry()
        {
            registry.Setup(r => r.IsRegistered(typeof(DummyObject), "Property1")).Returns(true);

            Assert.True(provider.IsRegistered(typeof(DummyObject), "Property1"));
        }

        public class DummyObject
        {
            public string Property1 { get; set; }

            public int Property2 { get; set; }
        }
    }
}