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
    using System.Reflection;
    using Xunit;
    using System.Linq;

    public class MetadataMergeExtensionsTests
    {
        [Fact]
        public void ShowForDisplay_should_be_true_for_new_metadata_item()
        {
            var metadataItem = new ModelMetadataItem();
            Assert.True(metadataItem.ShowForDisplay);
        }

        [Fact]
        public void ShowForDisplay_should_be_overwritten_if_it_was_set_to_true()
        {
            // arrange
            var to = new ModelMetadataItem { ShowForDisplay = true };
            var from = new ModelMetadataItem { ShowForDisplay = false };

            // act
            from.MergeTo(to);

            // assert
            Assert.False(to.ShowForDisplay);
        }
        
        [Fact]
        public void ShowForDisplay_should_not_be_overwritten_if_it_was_set_to_false()
        {
            // arrange
            var to = new ModelMetadataItem { ShowForDisplay = false };
            var from = new ModelMetadataItem { ShowForDisplay = true };

            // act
            from.MergeTo(to);

            // assert
            Assert.False(to.ShowForDisplay);
        }

        [Fact]
        public void ApplyFormatInEditMode_should_be_false_for_new_metadata_item()
        {
            var metadataItem = new ModelMetadataItem();
            Assert.False(metadataItem.ApplyFormatInEditMode);
        }

        [Fact]
        public void ApplyFormatInEditMode_should_be_overwritten_if_it_was_set_to_false()
        {
            // arrange
            var to = new ModelMetadataItem { ApplyFormatInEditMode = false };
            var from = new ModelMetadataItem { ApplyFormatInEditMode = true };

            // act
            from.MergeTo(to);

            // assert
            Assert.True(to.ApplyFormatInEditMode);
        }
        
        [Fact]
        public void ApplyFormatInEditMode_should_not_be_overwritten_if_it_was_set_to_true()
        {
            // arrange
            var to = new ModelMetadataItem { ApplyFormatInEditMode = true };
            var from = new ModelMetadataItem { ApplyFormatInEditMode = false };

            // act
            from.MergeTo(to);

            // assert
            Assert.True(to.ApplyFormatInEditMode);
        }

        [Fact]
        public void RequiredMetadata_should_not_removed()
        {
            // arrange
            var builder = new ModelMetadataItemBuilder<string>(new ModelMetadataItem());
            builder.Required();
            var to = builder.Item;

            var from = new ModelMetadataItem();

            // act
            from.MergeTo(to);

            // assert
            Assert.NotNull(to.IsRequired);
            Assert.True(to.IsRequired.Value);
            Assert.NotNull(to.GetValidation<RequiredValidationMetadata>());
        }

        [Fact]
        public void RequiredMetadata_should_set_if_it_was_added_for_from_metadata()
        {
            // arrange
            var to = new ModelMetadataItem();

            var builder = new ModelMetadataItemBuilder<string>(new ModelMetadataItem());
            builder.Required();
            var from = builder.Item;

            // act
            from.MergeTo(to);

            // assert
            Assert.NotNull(to.IsRequired);
            Assert.True(to.IsRequired.Value);
            Assert.NotNull(to.GetValidation<RequiredValidationMetadata>());
        }

        [Fact]
        public void RequiredMetadata_should_be_removed_if_it_was_set_as_optional_for_from_metadata()
        {
            // arrange
            var toBuilder = new ModelMetadataItemBuilder<string>(new ModelMetadataItem());
            toBuilder.Required();
            var to = toBuilder.Item;

            var fromBuilder = new ModelMetadataItemBuilder<string>(new ModelMetadataItem());
            fromBuilder.Optional();
            var @from = fromBuilder.Item;

            // act
            from.MergeTo(to);

            // assert
            Assert.NotNull(to.IsRequired);
            Assert.False(to.IsRequired.Value);
            Assert.Null(to.GetValidation<RequiredValidationMetadata>());
        }

        [Fact]
        public void Validation_should_not_removed()
        {
            // arrange
            var to = new ModelMetadataItem();
            new ModelMetadataItemBuilder<string>(to).MaximumLength(50);

            var from = new ModelMetadataItem();

            // act
            from.MergeTo(to);

            // assert
            var item = to.GetValidation<StringLengthValidationMetadata>();
            Assert.NotNull(item);
            Assert.Equal(50, item.Maximum);
        }

        [Fact]
        public void Validation_should_set_if_it_was_added_for_from_metadata()
        {
            // arrange
            var to = new ModelMetadataItem();

            var from = new ModelMetadataItem();
            new ModelMetadataItemBuilder<string>(from).MaximumLength(50);

            // act
            from.MergeTo(to);

            // assert
            var item = to.GetValidation<StringLengthValidationMetadata>();
            Assert.NotNull(item);
            Assert.Equal(50, item.Maximum);
        }

        [Fact]
        public void Validation_should_be_overwritten()
        {
            // arrange
            var to = new ModelMetadataItem();
            new ModelMetadataItemBuilder<string>(to).MaximumLength(100);

            var from = new ModelMetadataItem();
            new ModelMetadataItemBuilder<string>(from).MaximumLength(50);

            // act
            from.MergeTo(to);

            // assert
            var item = to.GetValidation<StringLengthValidationMetadata>();
            Assert.NotNull(item);
            Assert.Equal(100, item.Maximum);
        }


        [Fact]
        public void Should_not_overwrite_values_that_were_set()
        {
            // assert
            var properties = GetMetadataProperties();

            var to = new ModelMetadataItem();
            var from = new ModelMetadataItem();

            Func<string> func = () => "value";
            Func<string> funcFrom = () => "valueFrom";
            
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(bool?))
                {
                    propertyInfo.SetValue(to, true, null);
                    propertyInfo.SetValue(from, false, null);
                }
                else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
                {
                    propertyInfo.SetValue(to, 5, null);
                    propertyInfo.SetValue(from, 10, null);
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(to, func(), null);
                    propertyInfo.SetValue(from, funcFrom(), null);
                }
                else if (propertyInfo.PropertyType == typeof(Func<string>))
                {
                    propertyInfo.SetValue(to, func, null);
                    propertyInfo.SetValue(from, funcFrom, null);
                }
            }

            // act
            from.MergeTo(to);

            // assert
            foreach (var propertyInfo in properties)
            {
                Console.WriteLine(propertyInfo.Name);
                if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(bool?))
                {
                    Assert.True((bool)propertyInfo.GetValue(to, null));
                }
                else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
                {
                    Assert.Equal(5, (int)propertyInfo.GetValue(to, null));
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    var value = (string)propertyInfo.GetValue(to, null);
                    Assert.True(value == "value");
                }
                else if (propertyInfo.PropertyType == typeof(Func<string>))
                {
                    var value = (Func<string>)propertyInfo.GetValue(to, null);
                    Assert.True(value() == "value");
                }
            }
        }
        
        [Fact]
        public void Should_overwrite_values_that_were_not_set()
        {
            // assert
            var properties = GetMetadataProperties();

            var to = new ModelMetadataItem();
            var from = new ModelMetadataItem();

            Func<string> funcFrom = () => "valueFrom";

            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(bool?))
                {
                    propertyInfo.SetValue(from, true, null);
                }
                else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
                {
                    propertyInfo.SetValue(from, 10, null);
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(from, funcFrom(), null);
                }
                else if (propertyInfo.PropertyType == typeof(Func<string>))
                {
                    propertyInfo.SetValue(from, funcFrom, null);
                }
            }

            // act
            from.MergeTo(to);

            // assert
            foreach (var propertyInfo in properties)
            {
                Console.WriteLine(propertyInfo.Name);
                if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(bool?))
                {
                    Assert.True((bool)propertyInfo.GetValue(to, null));
                }
                else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
                {
                    Assert.Equal(10, (int)propertyInfo.GetValue(to, null));
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    var value = (string)propertyInfo.GetValue(to, null);
                    Assert.True(value == "valueFrom");
                }
                else if (propertyInfo.PropertyType == typeof(Func<string>))
                {
                    var value = (Func<string>)propertyInfo.GetValue(to, null);
                    Assert.True(value() == "valueFrom");
                }
            }
        }

        private static List<PropertyInfo> GetMetadataProperties()
        {
            Type type = typeof(ModelMetadataItem);
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(
                    p =>
                    (!p.PropertyType.IsValueType || p.PropertyType == typeof(bool?) ||
                     !p.PropertyType.IsValueType || p.PropertyType == typeof(int?)) &&
                    p.Name != "IsRequired")
                .ToList();

            Assert.Equal(16, properties.Count);
            return properties;
        }


        [Fact]
        public void AdditinalSettings_should_not_removed()
        {
            // arrange
            var to = new ModelMetadataItem();
            var settings = new DummyAdditinalSettings { Name = "val" };
            to.AdditionalSettings.Add(settings);

            var from = new ModelMetadataItem();

            // act
            from.MergeTo(to);

            // assert
            var item = to.GetAdditionalSetting<DummyAdditinalSettings>();
            Assert.NotNull(item);
            Assert.Equal("val", item.Name);
        }

        [Fact]
        public void AdditinalSettings_should_set_if_it_was_added_for_from_metadata()
        {
            // arrange
            var to = new ModelMetadataItem();
           
            var from = new ModelMetadataItem();
            var settings = new DummyAdditinalSettings { Name = "val2" };
            from.AdditionalSettings.Add(settings);

            // act
            from.MergeTo(to);

            // assert
            var item = to.GetAdditionalSetting<DummyAdditinalSettings>();
            Assert.NotNull(item);
            Assert.Equal("val2", item.Name);
        }

        [Fact]
        public void AdditinalSettings_should_be_overwritten()
        {
            // arrange
            var to = new ModelMetadataItem();
            var settings = new DummyAdditinalSettings { Name = "val" };
            to.AdditionalSettings.Add(settings);

            var from = new ModelMetadataItem();
            var settings2 = new DummyAdditinalSettings { Name = "val2" };
            from.AdditionalSettings.Add(settings2);

            // act
            from.MergeTo(to);

            // assert
            var item = to.GetAdditionalSetting<DummyAdditinalSettings>();
            Assert.NotNull(item);
            Assert.Equal("val", item.Name);
        }

        class DummyAdditinalSettings : IModelMetadataAdditionalSetting
        {
            public string Name { get; set; }
        }
    }
}
