#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using Xunit;

    public class ModelMetadataItemBuilderTests
    {
        private readonly ModelMetadataItemBuilder<object> builder;

        public ModelMetadataItemBuilderTests()
        {
            builder = new ModelMetadataItemBuilder<object>(new ModelMetadataItem());
        }

        [Fact]
        public void Should_be_able_to_set_display_name()
        {
            builder.DisplayName("foo");

            Assert.Equal("foo", builder.Item.DisplayName());
        }

        [Fact]
        public void Should_be_able_to_set_order()
        {
            builder.Order(123);

            Assert.Equal(123, builder.Item.Order);
        }

        [Fact]
        public void Order_is_null_by_default()
        {
            Assert.Null(builder.Item.Order);
        }

        [Fact]
        public void Should_be_able_to_set_short_display_name()
        {
            builder.ShortDisplayName("foo");

            Assert.Equal("foo", builder.Item.ShortDisplayName());
        }

        [Fact]
        public void Should_be_able_to_set_template()
        {
            builder.Template("foo");

            Assert.Equal("foo", builder.Item.TemplateName);
        }

        [Fact]
        public void Should_be_able_to_set_description()
        {
            builder.Description("foo");

            Assert.Equal("foo", builder.Item.Description());
        }

        [Fact]
        public void Should_be_able_to_set_readonly()
        {
            builder.ReadOnly();

            Assert.True(builder.Item.IsReadOnly.Value);
        }

        [Fact]
        public void Should_be_able_to_set_writable()
        {
            builder.Writable();

            Assert.False(builder.Item.IsReadOnly.Value);
        }

        [Fact]
        public void Should_be_able_to_set_other_property_to_compare()
        {
            builder.Compare("SomeProperty");

            Assert.NotEmpty(builder.Item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_other_property_to_compare_with_text_error_message()
        {
            builder.Compare("SomeProperty", "Properties must be equal");

            Assert.NotEmpty(builder.Item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_other_property_to_compare_with_type_and_resource_name()
        {
            builder.Compare("SomeProperty", typeof(object), "foo");

            Assert.NotEmpty(builder.Item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_required()
        {
            builder.Required();

            Assert.True(builder.Item.IsRequired.Value);
            Assert.NotEmpty(builder.Item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_required_with_text_error_message()
        {
            builder.Required("Value must be preset");

            Assert.True(builder.Item.IsRequired.Value);
            Assert.NotEmpty(builder.Item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_required_with_type_and_resource_name()
        {
            builder.Required(typeof(object), "foo");

            Assert.True(builder.Item.IsRequired.Value);
            Assert.NotEmpty(builder.Item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_optional()
        {
            builder.Required();
            builder.Optional();

            Assert.False(builder.Item.IsRequired.Value);
            Assert.Empty(builder.Item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_as_hidden()
        {
            builder.AsHidden();

            Assert.Equal("HiddenInput", builder.Item.TemplateName);
        }

        [Fact]
        public void Should_be_able_to_set_as_hidden_with_hide_surrounding_html()
        {
            builder.AsHidden(true);

            Assert.Equal("HiddenInput", builder.Item.TemplateName);
            Assert.True(builder.Item.HideSurroundingHtml.Value);
        }

        [Fact]
        public void Should_be_able_to_hide_surrounding_html()
        {
            builder.HideSurroundingHtml();

            Assert.True(builder.Item.HideSurroundingHtml.Value);
        }

        [Fact]
        public void Should_be_able_to_show_surrounding_html()
        {
            builder.ShowSurroundingHtml();

            Assert.False(builder.Item.HideSurroundingHtml.Value);
        }

        [Fact]
        public void Should_be_able_to_allow_html()
        {
            builder.AllowHtml();

            Assert.False(builder.Item.RequestValidationEnabled.Value);
        }

        [Fact]
        public void Should_be_able_to_disallow_html()
        {
            builder.DisallowHtml();

            Assert.True(builder.Item.RequestValidationEnabled.Value);
        }

        [Fact]
        public void Should_be_able_to_set_show_for_display()
        {
            builder.ShowForDisplay();

            Assert.True(builder.Item.ShowForDisplay);
        }

        [Fact]
        public void Should_be_able_to_set_hide_for_display()
        {
            builder.HideForDisplay();

            Assert.False(builder.Item.ShowForDisplay);
        }

        [Fact]
        public void Should_be_able_to_set_show_for_edit()
        {
            builder.ShowForEdit();

            Assert.True(builder.Item.ShowForEdit.Value);
        }

        [Fact]
        public void Should_be_able_to_set_hide_for_edit()
        {
            builder.HideForEdit();

            Assert.False(builder.Item.ShowForEdit.Value);
        }

        [Fact]
        public void Should_be_able_to_set_show()
        {
            builder.Show();

            Assert.True(builder.Item.ShowForDisplay);
            Assert.True(builder.Item.ShowForEdit.Value);
        }

        [Fact]
        public void Should_be_able_to_set_hide()
        {
            builder.Hide();

            Assert.False(builder.Item.ShowForDisplay);
            Assert.False(builder.Item.ShowForEdit.Value);
        }

        [Fact]
        public void Should_be_able_to_set_null_display_text()
        {
            builder.NullDisplayText("n/a");

            Assert.Equal("n/a", builder.Item.NullDisplayText());
        }

        [Fact]
        public void Should_be_able_to_set_watermark()
        {
            builder.Watermark("enter your value...");
            Assert.Equal("enter your value...", builder.Item.Watermark());
        }
    }
}