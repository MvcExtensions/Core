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

            Assert.Equal("DropDownList", builder.Item.TemplateName);
            Assert.NotEmpty(builder.Item.AdditionalSettings);
        }

        [Fact]
        public void Should_be_able_to_set_as_drop_down_list_with_option_label()
        {
            builder.AsDropDownList("dummyKey", "[Select an option]");

            Assert.Equal("DropDownList", builder.Item.TemplateName);
            Assert.NotEmpty(builder.Item.AdditionalSettings);
        }

        [Fact]
        public void Should_be_able_to_set_as_drop_down_list_with_option_label_and_template()
        {
            builder.AsDropDownList("dummyKey", "[Select an option]", "fooTemplate");

            Assert.Equal("fooTemplate", builder.Item.TemplateName);
            Assert.NotEmpty(builder.Item.AdditionalSettings);
        }

        [Fact]
        public void Should_be_able_to_set_as_list_box()
        {
            builder.AsListBox("dummyKey");

            Assert.Equal("ListBox", builder.Item.TemplateName);
            Assert.NotEmpty(builder.Item.AdditionalSettings);
        }

        [Fact]
        public void Should_be_able_to_set_as_list_box_with_template()
        {
            builder.AsListBox("dummyKey", "fooTemplate");

            Assert.Equal("fooTemplate", builder.Item.TemplateName);
            Assert.NotEmpty(builder.Item.AdditionalSettings);
        }
    }
}