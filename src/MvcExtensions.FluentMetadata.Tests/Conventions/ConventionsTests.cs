#region Copyright
// Copyright (c) 2009 - 2011, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public class ConventionsTests
    {
        private const string PropertyName = "Name";
        private readonly Type modelType;
        private readonly ModelMetadataRegistry registry;

        public ConventionsTests()
        {
            registry = new ModelMetadataRegistry();
            modelType = typeof(DummyMetaconvTestModel);
        }

        [Fact]
        public void Should_add_conventions_when_condition_is_matched()
        {
            // arrange
            registry.Conventions.Add(new TestPropertyMetadataConvention());
            registry.RegisterModelProperties(modelType, new Dictionary<string, ModelMetadataItem>());

            // act
            var result = registry.GetModelPropertyMetadata(modelType, PropertyName);

            // assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Validations);
            Assert.NotNull(result.IsRequired);
            Assert.True(result.IsRequired.Value);
        }

        [Fact]
        public void Should_not_overwrite_existing_metadata_validation()
        {
            // arrange
            registry.Conventions.Add(new TestPropertyMetadataConvention());

            var items = new Dictionary<string, ModelMetadataItem>();
            var metadataItem = new ModelMetadataItem();
            const int expected = 200;
            new ModelMetadataItemBuilder<string>(metadataItem).MaximumLength(expected);
            items.Add(PropertyName, metadataItem);

            registry.RegisterModelProperties(modelType, items);

            // act
            var result = registry.GetModelPropertyMetadata(modelType, PropertyName);

            // assert
            var val = result.Validations.OfType<StringLengthValidationMetadata>().FirstOrDefault();
            Assert.NotNull(val);
            Assert.Equal(expected, val.Maximum);
        }

        public class DummyMetaconvTestModel
        {
            public string Name { get; set; }
        }

        public class TestPropertyMetadataConvention : DefaultPropertyMetadataConvention<string>
        {
            protected override bool CanBeAcceptedCore(PropertyInfo propertyInfo)
            {
                return propertyInfo.Name == "Name";
            }

            protected override void CreateMetadataRulesCore(ModelMetadataItemBuilder<string> builder)
            {
                builder
                    .Required()
                    .MaximumLength(50);
            }
        }
    }
}
