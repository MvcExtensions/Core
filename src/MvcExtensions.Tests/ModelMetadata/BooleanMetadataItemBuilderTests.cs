#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using Xunit;

    public class BooleanMetadataItemBuilderTests
    {
        [Fact]
        public void Should_be_able_to_create()
        {
            var builder = new ModelMetadataItemBuilder<bool>(new ModelMetadataItem());

            Assert.NotNull(builder);
        }
    }
}