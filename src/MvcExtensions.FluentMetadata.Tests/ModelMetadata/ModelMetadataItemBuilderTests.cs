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

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("foo", item.DisplayName());
        }

        [Fact]
        public void Should_be_able_to_set_order()
        {
            builder.Order(123);

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal(123, item.Order);
        }

        [Fact]
        public void Order_is_null_by_default()
        {
            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Null(item.Order);
        }

        [Fact]
        public void Should_be_able_to_set_short_display_name()
        {
            builder.ShortDisplayName("foo");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("foo", item.ShortDisplayName());
        }

        [Fact]
        public void Should_be_able_to_set_template()
        {
            builder.Template("foo");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("foo", item.TemplateName);
        }

        [Fact]
        public void Should_be_able_to_set_description()
        {
            builder.Description("foo");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("foo", item.Description());
        }

        [Fact]
        public void Should_be_able_to_set_readonly()
        {
            builder.ReadOnly();

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.True(item.IsReadOnly.Value);
        }

        [Fact]
        public void Should_be_able_to_set_writable()
        {
            builder.Writable();

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.False(item.IsReadOnly.Value);
        }

        [Fact]
        public void Should_be_able_to_set_other_property_to_compare()
        {
            builder.Compare("SomeProperty");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.NotEmpty(item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_other_property_to_compare_with_text_error_message()
        {
            builder.Compare("SomeProperty", "Properties must be equal");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.NotEmpty(item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_other_property_to_compare_with_type_and_resource_name()
        {
            builder.Compare("SomeProperty", typeof(object), "foo");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.NotEmpty(item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_required()
        {
            builder.Required();

            var configurator1 = (IModelMetadataItemConfigurator)builder;
            var item1 = new ModelMetadataItem();
            configurator1.Configure(item1);
            Assert.True(item1.IsRequired.Value);
            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.NotEmpty(item.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_required_with_text_error_message()
        {
            builder.Required("Value must be preset");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.True(item.IsRequired.Value);
            var configurator1 = (IModelMetadataItemConfigurator)builder;
            var item1 = new ModelMetadataItem();
            configurator1.Configure(item1);
            Assert.NotEmpty(item1.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_required_with_type_and_resource_name()
        {
            builder.Required(typeof(object), "foo");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.True(item.IsRequired.Value);
            var configurator1 = (IModelMetadataItemConfigurator)builder;
            var item1 = new ModelMetadataItem();
            configurator1.Configure(item1);
            Assert.NotEmpty(item1.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_optional()
        {
            builder.Required();
            builder.Optional();

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.False(item.IsRequired.Value);
            var configurator1 = (IModelMetadataItemConfigurator)builder;
            var item1 = new ModelMetadataItem();
            configurator1.Configure(item1);
            Assert.Empty(item1.Validations);
        }

        [Fact]
        public void Should_be_able_to_set_as_hidden()
        {
            builder.AsHidden();

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("HiddenInput", item.TemplateName);
        }

        [Fact]
        public void Should_be_able_to_set_as_hidden_with_hide_surrounding_html()
        {
            builder.AsHidden(true);

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("HiddenInput", item.TemplateName);
            var configurator1 = (IModelMetadataItemConfigurator)builder;
            var item1 = new ModelMetadataItem();
            configurator1.Configure(item1);
            Assert.True(item1.HideSurroundingHtml.Value);
        }

        [Fact]
        public void Should_be_able_to_hide_surrounding_html()
        {
            builder.HideSurroundingHtml();

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.True(item.HideSurroundingHtml.Value);
        }

        [Fact]
        public void Should_be_able_to_show_surrounding_html()
        {
            builder.ShowSurroundingHtml();

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.False(item.HideSurroundingHtml.Value);
        }

        [Fact]
        public void Should_be_able_to_allow_html()
        {
            builder.AllowHtml();

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.False(item.RequestValidationEnabled.Value);
        }

        [Fact]
        public void Should_be_able_to_disallow_html()
        {
            builder.DisallowHtml();

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.True(item.RequestValidationEnabled.Value);
        }

        [Fact]
        public void Should_be_able_to_set_show_for_display()
        {
            builder.ShowForDisplay();

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.True(item.ShowForDisplay);
        }

        [Fact]
        public void Should_be_able_to_set_hide_for_display()
        {
            builder.HideForDisplay();

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.False(item.ShowForDisplay);
        }

        [Fact]
        public void Should_be_able_to_set_show_for_edit()
        {
            builder.ShowForEdit();

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.True(item.ShowForEdit.Value);
        }

        [Fact]
        public void Should_be_able_to_set_hide_for_edit()
        {
            builder.HideForEdit();

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.False(item.ShowForEdit.Value);
        }

        [Fact]
        public void Should_be_able_to_set_show()
        {
            builder.Show();

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.True(item.ShowForDisplay);
            var configurator1 = (IModelMetadataItemConfigurator)builder;
            var item1 = new ModelMetadataItem();
            configurator1.Configure(item1);
            Assert.True(item1.ShowForEdit.Value);
        }

        [Fact]
        public void Should_be_able_to_set_hide()
        {
            builder.Hide();

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.False(item.ShowForDisplay);
            var configurator1 = (IModelMetadataItemConfigurator)builder;
            var item1 = new ModelMetadataItem();
            configurator1.Configure(item1);
            Assert.False(item1.ShowForEdit.Value);
        }

        [Fact]
        public void Should_be_able_to_set_null_display_text()
        {
            builder.NullDisplayText("n/a");

            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("n/a", item.NullDisplayText());
        }

        [Fact]
        public void Should_be_able_to_set_watermark()
        {
            builder.Watermark("enter your value...");
            var configurator = (IModelMetadataItemConfigurator)builder;
            var item = new ModelMetadataItem();
            configurator.Configure(item);
            Assert.Equal("enter your value...", item.Watermark());
        }
    }
}