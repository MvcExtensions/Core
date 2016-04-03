#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using Xunit;

    public class ValueTypeItemBuilderTests
    {
        private readonly ModelMetadataItemBuilder<int> builder;

        public ValueTypeItemBuilderTests()
        {
            builder = new ModelMetadataItemBuilder<int>(new ModelMetadataItem());
        }

        [Fact]
        public void Should_be_able_to_set_display_format()
        {
            builder.DisplayFormat("{0:d}");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("{0:d}", item.DisplayFormat());
        }

        [Fact]
        public void Should_be_able_to_set_edit_format()
        {
            builder.EditFormat("{0:d}");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("{0:d}", item.EditFormat());
        }

        [Fact]
        public void Should_be_able_to_set_format()
        {
            builder.Format("{0:d}");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("{0:d}", item.DisplayFormat());
            var configurator1 = (IModelMetadataItemConfigurator)builder;
            var item1 = new ModelMetadataItem();
            configurator1.Configure(item1);
            Assert.Equal("{0:d}", item1.EditFormat());
        }

        [Fact]
        public void Should_be_able_to_set_apply_format_in_editmode()
        {
            builder.ApplyFormatInEditMode();

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.True(item.ApplyFormatInEditMode);
        }

        [Fact]
        public void Should_be_able_to_set_range()
        {
            builder.Range(1, 100);

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.NotEmpty(item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_expression_with_text_message()
        {
            builder.Range(1, 100, "Value must be between 1 -100");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.NotEmpty(item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_expression_with_type_and_resource_name()
        {
            builder.Range(1, 100, typeof(object), "foo");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.NotEmpty(item.Validations);
        }
    }
}