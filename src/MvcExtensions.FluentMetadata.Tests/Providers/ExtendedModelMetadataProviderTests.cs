#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
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
        public void GetMetadataForProperties_should_return_properties_metadata_when_model_type_is_registered()
        {
            registry.Setup(r => r.GetModelPropertiesMetadata(It.IsAny<Type>())).Returns(new Dictionary<string, ModelMetadataItem> { { "Property1", new Mock<ModelMetadataItem>().Object } });

            var metadata = provider.GetMetadataForProperties(new DummyObject(), typeof(DummyObject));

            Assert.Equal(2, metadata.Count());
        }

        [Fact]
        public void GetMetadataForProperties_should_return_properties_metadata_when_model_type_is_not_registered()
        {
            registry.Setup(r => r.GetModelPropertiesMetadata(It.IsAny<Type>())).Returns((IDictionary<string, ModelMetadataItem>)null);

            var metadata = provider.GetMetadataForProperties(new DummyObject(), typeof(DummyObject));

            Assert.Equal(2, metadata.Count());
        }

        [Fact]
        public void GetMetadataForProperty_should_return_property_metadata_when_model_type_and_property_is_registered()
        {
            registry.Setup(r => r.GetModelPropertyMetadata(It.IsAny<Type>(), It.IsAny<string>())).Returns(new Mock<ModelMetadataItem>().Object);

            var metadata = provider.GetMetadataForProperty(() => new DummyObject(), typeof(DummyObject), "Property1");

            Assert.NotNull(metadata);
        }

        [Fact]
        public void GetMetadataForProperty_should_return_property_metadata_when_model_type_and_property_is_not_registered()
        {
            registry.Setup(r => r.GetModelPropertyMetadata(It.IsAny<Type>(), It.IsAny<string>())).Returns((ModelMetadataItem)null);

            var metadata = provider.GetMetadataForProperty(() => new DummyObject(), typeof(DummyObject), "Property1");

            Assert.NotNull(metadata);
        }

        [Fact]
        public void GetMetadataForProperty_should_throw_exception_for_invalid_property()
        {
            registry.Setup(r => r.GetModelPropertyMetadata(It.IsAny<Type>(), It.IsAny<string>())).Returns(new Mock<ModelMetadataItem>().Object);

            Assert.Throws<ArgumentException>(() => provider.GetMetadataForProperty(() => new DummyObject(), typeof(DummyObject), "Property3"));
        }

        [Fact]
        public void GetMetadataForType_should_return_metadata_model_type_is_registered()
        {
            registry.Setup(r => r.GetModelMetadata(It.IsAny<Type>())).Returns(new Mock<ModelMetadataItem>().Object);

            Assert.NotNull(provider.GetMetadataForType(() => new DummyObject(), typeof(DummyObject)));
        }

        [Fact]
        public void GetMetadataForType_should_return_metadata_model_type_is_not_registered()
        {
            Assert.NotNull(provider.GetMetadataForType(() => new DummyObject(), typeof(DummyObject)));
        }

        [Theory]
        [InlineData(false, false, false, true, true, "A property", "a property", "foo", "a description", "n/a", "Please enter...", "{0:c}", "{0:c}")]
        [InlineData(true, true, true, false, false, null, null, null, null, null, null, null, null)]
        public void Should_return_metadata_with_same_value_as_model_meta_data_item(bool hideSurroundingHtml, bool isReadOnly, bool isRequired, bool showForEdit, bool applyFormatInEditMode, string displayName, string shortDisplayName, string template, string description, string nullDisplayText, string watermark, string displayFormat, string editFormat)
        {
            var metadataItem = new ModelMetadataItem { HideSurroundingHtml = hideSurroundingHtml, IsReadOnly = isReadOnly, IsRequired = isRequired, ShowForEdit = showForEdit, ApplyFormatInEditMode = applyFormatInEditMode, TemplateName = template };

            if (displayName != null)
            {
                metadataItem.DisplayName = () => displayName;
            }

            if (shortDisplayName != null)
            {
                metadataItem.ShortDisplayName = () => shortDisplayName;
            }

            if (description != null)
            {
                metadataItem.Description = () => description;
            }

            if (nullDisplayText != null)
            {
                metadataItem.NullDisplayText = () => nullDisplayText;
            }

            if (watermark != null)
            {
                metadataItem.Watermark = () => watermark;
            }

            if (displayFormat != null)
            {
                metadataItem.DisplayFormat = () => displayFormat;
            }

            if (editFormat != null)
            {
                metadataItem.EditFormat = () => editFormat;
            }

            registry.Setup(r => r.GetModelPropertyMetadata(It.IsAny<Type>(), It.IsAny<string>())).Returns(metadataItem);

            var metadata = provider.GetMetadataForProperty(() => new DummyObject(), typeof(DummyObject), "Property1");

            Assert.Equal(metadataItem.HideSurroundingHtml, metadata.HideSurroundingHtml);
            Assert.Equal(metadataItem.IsReadOnly, metadata.IsReadOnly);
            Assert.Equal(metadataItem.IsRequired, metadata.IsRequired);
            Assert.Equal(metadataItem.ShowForEdit, metadata.ShowForEdit);

            if (displayName != null)
            {
                Assert.Equal(metadataItem.DisplayName(), metadata.DisplayName);
            }

            if (shortDisplayName != null)
            {
                Assert.Equal(metadataItem.ShortDisplayName(), metadata.ShortDisplayName);
            }

            Assert.Equal(metadataItem.TemplateName, metadata.TemplateHint);

            if (description != null)
            {
                Assert.Equal(metadataItem.Description(), metadata.Description);
            }

            if (nullDisplayText != null)
            {
                Assert.Equal(metadataItem.NullDisplayText(), metadata.NullDisplayText);
            }

            if (watermark != null)
            {
                Assert.Equal(metadataItem.Watermark(), metadata.Watermark);
            }

            if (displayFormat != null)
            {
                Assert.Equal(metadataItem.DisplayFormat(), metadata.DisplayFormatString);
            }

            if (editFormat != null)
            {
                Assert.Equal(metadataItem.EditFormat(), metadata.EditFormatString);
            }
        }

        public class DummyObject
        {
            public string Property1 { get; set; }

            public int Property2 { get; set; }
        }
    }
}