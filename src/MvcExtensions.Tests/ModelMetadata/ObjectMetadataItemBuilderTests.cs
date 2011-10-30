#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using Xunit;

    public class ObjectMetadataItemBuilderTests
    {
        private readonly ModelMetadataItem item;
        private readonly ObjectMetadataItemBuilder<object> builder;

        public ObjectMetadataItemBuilderTests()
        {
            item = new ModelMetadataItem();
            builder = new ObjectMetadataItemBuilder<object>(item);
        }

        [Fact]
        public void Should_be_able_to_set_as_drop_down_list()
        {
            builder.AsDropDownList("dummyKey");

            Assert.Equal("DropDownList", item.TemplateName);
            Assert.NotEmpty(item.AdditionalSettings);
        }

        [Fact]
        public void Should_be_able_to_set_as_drop_down_list_with_option_label()
        {
            builder.AsDropDownList("dummyKey", "[Select an option]");

            Assert.Equal("DropDownList", item.TemplateName);
            Assert.NotEmpty(item.AdditionalSettings);
        }

        [Fact]
        public void Should_be_able_to_set_as_drop_down_list_with_option_label_and_template()
        {
            builder.AsDropDownList("dummyKey", "[Select an option]", "fooTemplate");

            Assert.Equal("fooTemplate", item.TemplateName);
            Assert.NotEmpty(item.AdditionalSettings);
        }

        [Fact]
        public void Should_be_able_to_set_as_list_box()
        {
            builder.AsListBox("dummyKey");

            Assert.Equal("ListBox", item.TemplateName);
            Assert.NotEmpty(item.AdditionalSettings);
        }

        [Fact]
        public void Should_be_able_to_set_as_list_box_with_template()
        {
            builder.AsListBox("dummyKey", "fooTemplate");

            Assert.Equal("fooTemplate", item.TemplateName);
            Assert.NotEmpty(item.AdditionalSettings);
        }
    }
}