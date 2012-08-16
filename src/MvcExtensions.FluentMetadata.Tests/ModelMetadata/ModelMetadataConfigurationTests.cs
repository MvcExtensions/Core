#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using System;
    using System.Linq.Expressions;
    using Xunit;

    public class ModelMetadataConfigurationTests
    {
        private readonly DummyObjectConfiguration configuration;

        public ModelMetadataConfigurationTests()
        {
            configuration = new DummyObjectConfiguration();
        }

        [Fact]
        public void ModelType_should_be_same_as_generic_argument()
        {
            Assert.Same(typeof(DummyObject), configuration.ModelType);
        }

        [Fact]
        public void Configurations_should_be_empty_when_new_instance_is_created()
        {
            Assert.Empty(configuration.Configurations);
        }

        [Fact]
        public void Should_be_able_to_configure_string_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.StringProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_boolean_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.BooleanProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_nullable_boolean_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.NullableBooleanProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_datetime_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.DateTimeProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_nullable_datetime_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.NullableDateTimeProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_byte_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.ByteProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_nullable_byte_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.NullableByteProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_short_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.ShortProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_nullable_short_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.NullableShortProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_integer_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.IntegerProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_nullable_integer_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.NullableIntegerProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_long_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.LongProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_nullable_long_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.NullableLongProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_float_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.FloatProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_nullable_float_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.NullableFloatProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_double_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.DoubleProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_nullable_double_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.NullableDoubleProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_decimal_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.DecimalProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_nullable_decimal_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.NullableDecimalProperty));
        }

        [Fact]
        public void Should_be_able_to_configure_object_property()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.ObjectProperty));
        }

        [Fact]
        public void Should_be_able_to_override_configuration()
        {
            Assert.NotNull(configuration.PublicConfigure(x => x.ObjectProperty));
            Assert.NotNull(configuration.PublicConfigure(x => x.ObjectProperty));
        }

        private sealed class DummyObject
        {
            public string StringProperty { get; set; }

            public bool BooleanProperty { get; set; }

            public bool? NullableBooleanProperty { get; set; }

            public DateTime DateTimeProperty { get; set; }

            public DateTime? NullableDateTimeProperty { get; set; }

            public byte ByteProperty { get; set; }

            public byte? NullableByteProperty { get; set; }

            public short ShortProperty { get; set; }

            public short? NullableShortProperty { get; set; }

            public int IntegerProperty { get; set; }

            public int? NullableIntegerProperty { get; set; }

            public long LongProperty { get; set; }

            public long? NullableLongProperty { get; set; }

            public float FloatProperty { get; set; }

            public float? NullableFloatProperty { get; set; }

            public double DoubleProperty { get; set; }

            public double? NullableDoubleProperty { get; set; }

            public decimal DecimalProperty { get; set; }

            public decimal? NullableDecimalProperty { get; set; }

            public object ObjectProperty { get; set; }
        }

        private sealed class DummyObjectConfiguration : ModelMetadataConfiguration<DummyObject>
        {
            public ModelMetadataItemBuilder<T> PublicConfigure<T>(Expression<Func<DummyObject, T>> expression)
            {
                return Configure(expression);
            }
        }
    }
}