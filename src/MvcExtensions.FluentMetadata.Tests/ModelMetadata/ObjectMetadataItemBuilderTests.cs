#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using Xunit;

    public class ObjectMetadataItemBuilderTests
    {
        private readonly ModelMetadataItemBuilder<object> builder;

        public ObjectMetadataItemBuilderTests()
        {
            builder = new ModelMetadataItemBuilder<object>(new ModelMetadataItem());
        }

        [Fact]
        public void Should_be_able_to_set_as_drop_down_list()
        {
            builder.AsDropDownList("dummyKey");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("DropDownList", item.TemplateName);
            var configurator1 = (IModelMetadataItemConfigurator)builder;
            var item1 = new ModelMetadataItem();
            configurator1.Configure(item1);
            Assert.NotEmpty(item1.AdditionalSettings);
        }

        [Fact]
        public void Should_be_able_to_set_as_drop_down_list_with_option_label()
        {
            builder.AsDropDownList("dummyKey", "[Select an option]");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("DropDownList", item.TemplateName);
            var configurator1 = (IModelMetadataItemConfigurator)builder;
            var item1 = new ModelMetadataItem();
            configurator1.Configure(item1);
            Assert.NotEmpty(item1.AdditionalSettings);
        }

        [Fact]
        public void Should_be_able_to_set_as_drop_down_list_with_option_label_and_template()
        {
            builder.AsDropDownList("dummyKey", "[Select an option]", "fooTemplate");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("fooTemplate", item.TemplateName);
            var configurator1 = (IModelMetadataItemConfigurator)builder;
            var item1 = new ModelMetadataItem();
            configurator1.Configure(item1);
            Assert.NotEmpty(item1.AdditionalSettings);
        }

        [Fact]
        public void Should_be_able_to_set_as_list_box()
        {
            builder.AsListBox("dummyKey");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("ListBox", item.TemplateName);
            var configurator1 = (IModelMetadataItemConfigurator)builder;
            var item1 = new ModelMetadataItem();
            configurator1.Configure(item1);
            Assert.NotEmpty(item1.AdditionalSettings);
        }

        [Fact]
        public void Should_be_able_to_set_as_list_box_with_template()
        {
            builder.AsListBox("dummyKey", "fooTemplate");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("fooTemplate", item.TemplateName);
            var configurator1 = (IModelMetadataItemConfigurator)builder;
            var item1 = new ModelMetadataItem();
            configurator1.Configure(item1);
            Assert.NotEmpty(item1.AdditionalSettings);
        }
    }
}