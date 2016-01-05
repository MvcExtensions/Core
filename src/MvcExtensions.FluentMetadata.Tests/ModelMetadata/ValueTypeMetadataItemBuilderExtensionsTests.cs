#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using System;
    using Xunit;

    public class ValueTypeMetadataItemBuilderExtensionsTests
    {
        [Fact]
        public void Should_be_able_to_format_as_currency_with_decimal()
        {
            var builder = new ModelMetadataItemBuilder<decimal>(new ModelMetadataItem());

            builder.FormatAsCurrency();

            var item = builder.Item;
            Assert.Equal(item.DisplayFormat(), "{0:c}");
            Assert.Equal(item.EditFormat(), "{0:c}");
        }

        [Fact]
        public void Should_be_able_to_format_as_currency_with_nullable_decimal()
        {
            var builder = new ModelMetadataItemBuilder<decimal?>(new ModelMetadataItem());

            builder.FormatAsCurrency();

            var item = builder.Item;
            Assert.Equal(item.DisplayFormat(), "{0:c}");
            Assert.Equal(item.EditFormat(), "{0:c}");
        }

        [Fact]
        public void Should_be_able_to_format_as_date_with_date_time()
        {
            var builder = new ModelMetadataItemBuilder<DateTime>(new ModelMetadataItem());

            builder.FormatAsDateOnly();

            var item = builder.Item;
            Assert.Equal(item.DisplayFormat(), "{0:d}");
            Assert.Equal(item.EditFormat(), "{0:d}");
        }

        [Fact]
        public void Should_be_able_to_format_as_date_with_nullable_date_time()
        {
            var builder = new ModelMetadataItemBuilder<DateTime?>(new ModelMetadataItem());

            builder.FormatAsDateOnly();

            var item = builder.Item;
            Assert.Equal(item.DisplayFormat(), "{0:d}");
            Assert.Equal(item.EditFormat(), "{0:d}");
        }

        [Fact]
        public void Should_be_able_to_format_as_time_with_date_time()
        {
            var builder = new ModelMetadataItemBuilder<DateTime>(new ModelMetadataItem());

            builder.FormatAsTimeOnly();

            var item = builder.Item;
            Assert.Equal(item.DisplayFormat(), "{0:t}");
            Assert.Equal(item.EditFormat(), "{0:t}");
        }

        [Fact]
        public void Should_be_able_to_format_as_time_with_nullable_date_time()
        {
            var builder = new ModelMetadataItemBuilder<DateTime?>(new ModelMetadataItem());

            builder.FormatAsTimeOnly();

            var item = builder.Item;
            Assert.Equal(item.DisplayFormat(), "{0:t}");
            Assert.Equal(item.EditFormat(), "{0:t}");
        }
    }
}