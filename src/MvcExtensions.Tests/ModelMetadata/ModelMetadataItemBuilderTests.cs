#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using Xunit;

    public class ModelMetadataItemBuilderTests
    {
        private readonly ModelMetadataItemTestDouble item;
        private readonly ModelMetadataItemBuilderTestDouble builder;

        public ModelMetadataItemBuilderTests()
        {
            item = new ModelMetadataItemTestDouble();
            builder = new ModelMetadataItemBuilderTestDouble(item);
        }

        [Fact]
        public void Should_be_able_to_set_display_name()
        {
            builder.DisplayName("foo");

            Assert.Equal("foo", item.DisplayName());
        }

        [Fact]
        public void Should_be_able_to_set_order()
        {
            builder.Order(123);

            Assert.Equal(123, item.Order);
        }

        [Fact]
        public void Order_is_null_by_default()
        {
            Assert.Null(item.Order);
        }

        [Fact]
        public void Should_be_able_to_set_short_display_name()
        {
            builder.ShortDisplayName("foo");

            Assert.Equal("foo", item.ShortDisplayName());
        }

        [Fact]
        public void Should_be_able_to_set_template()
        {
            builder.Template("foo");

            Assert.Equal("foo", item.TemplateName);
        }

        [Fact]
        public void Should_be_able_to_set_description()
        {
            builder.Description("foo");

            Assert.Equal("foo", item.Description());
        }

        [Fact]
        public void Should_be_able_to_set_readonly()
        {
            builder.ReadOnly();

            Assert.True(item.IsReadOnly.Value);
        }

        [Fact]
        public void Should_be_able_to_set_writable()
        {
            builder.Writable();

            Assert.False(item.IsReadOnly.Value);
        }

        [Fact]
        public void Should_be_able_to_set_other_property_to_compare()
        {
            builder.Compare("SomeProperty");

            Assert.NotEmpty(item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_other_property_to_compare_with_text_error_message()
        {
            builder.Compare("SomeProperty", "Properties must be equal");

            Assert.NotEmpty(item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_other_property_to_compare_with_type_and_resource_name()
        {
            builder.Compare("SomeProperty", typeof(object), "foo");

            Assert.NotEmpty(item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_required()
        {
            builder.Required();

            Assert.True(item.IsRequired.Value);
            Assert.NotEmpty(item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_required_with_text_error_message()
        {
            builder.Required("Value must be preset");

            Assert.True(item.IsRequired.Value);
            Assert.NotEmpty(item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_required_with_type_and_resource_name()
        {
            builder.Required(typeof(object), "foo");

            Assert.True(item.IsRequired.Value);
            Assert.NotEmpty(item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_optional()
        {
            builder.Required();
            builder.Optional();

            Assert.False(item.IsRequired.Value);
            Assert.Empty(item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_as_hidden()
        {
            builder.AsHidden();

            Assert.Equal("HiddenInput", item.TemplateName);
        }

        [Fact]
        public void Should_be_able_to_set_as_hidden_with_hide_surrounding_html()
        {
            builder.AsHidden(true);

            Assert.Equal("HiddenInput", item.TemplateName);
            Assert.True(item.HideSurroundingHtml.Value);
        }

        [Fact]
        public void Should_be_able_to_hide_surrounding_html()
        {
            builder.HideSurroundingHtml();

            Assert.True(item.HideSurroundingHtml.Value);
        }

        [Fact]
        public void Should_be_able_to_show_surrounding_html()
        {
            builder.ShowSurroundingHtml();

            Assert.False(item.HideSurroundingHtml.Value);
        }

        [Fact]
        public void Should_be_able_to_allow_html()
        {
            builder.AllowHtml();

            Assert.False(item.RequestValidationEnabled.Value);
        }

        [Fact]
        public void Should_be_able_to_disallow_html()
        {
            builder.DisallowHtml();

            Assert.True(item.RequestValidationEnabled.Value);
        }

        [Fact]
        public void Should_be_able_to_set_show_for_display()
        {
            builder.ShowForDisplay();

            Assert.True(item.ShowForDisplay);
        }

        [Fact]
        public void Should_be_able_to_set_hide_for_display()
        {
            builder.HideForDisplay();

            Assert.False(item.ShowForDisplay);
        }

        [Fact]
        public void Should_be_able_to_set_show_for_edit()
        {
            builder.ShowForEdit();

            Assert.True(item.ShowForEdit.Value);
        }

        [Fact]
        public void Should_be_able_to_set_hide_for_edit()
        {
            builder.HideForEdit();

            Assert.False(item.ShowForEdit.Value);
        }

        [Fact]
        public void Should_be_able_to_set_show()
        {
            builder.Show();

            Assert.True(item.ShowForDisplay);
            Assert.True(item.ShowForEdit.Value);
        }

        [Fact]
        public void Should_be_able_to_set_hide()
        {
            builder.Hide();

            Assert.False(item.ShowForDisplay);
            Assert.False(item.ShowForEdit.Value);
        }

        [Fact]
        public void Should_be_able_to_set_null_display_text()
        {
            builder.NullDisplayText("n/a");

            Assert.Equal("n/a", item.NullDisplayText());
        }

        [Fact]
        public void Should_be_able_to_set_watermark()
        {
            builder.Watermark("enter your value...");

            Assert.Equal("enter your value...", item.Watermark());
        }

        private sealed class ModelMetadataItemTestDouble : ModelMetadataItem
        {
        }

        private sealed class ModelMetadataItemBuilderTestDouble : ModelMetadataItemBuilder<ModelMetadataItemBuilderTestDouble>
        {
            public ModelMetadataItemBuilderTestDouble(ModelMetadataItemTestDouble item) : base(item)
            {
            }
        }
    }
}