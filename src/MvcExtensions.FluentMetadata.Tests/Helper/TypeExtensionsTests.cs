#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests.Helper
{
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using Moq;
    using MvcExtensions.FluentMetadata.Tests.Resources;
    using Xunit;

    public class TypeExtensionsTests
    {
        [Fact]
        public void Should_return_matched_attribute()
        {
            // arrange
            var attributeProvider = new Mock<ICustomAttributeProvider>();
            var attribute = new MetadataConventionsAttribute { ResourceType = typeof(TestResource) };
            attributeProvider.Setup(a => a.GetCustomAttributes(typeof(MetadataConventionsAttribute), true)).Returns(new object[] { attribute });

            // act
            var result = attributeProvider.Object.FirstOrDefault<MetadataConventionsAttribute>();

            // assert
            Assert.Equal(typeof(TestResource), result.ResourceType);
        }

        [Fact]
        public void Should_return_null_if_no_matching_attributes()
        {
            // arrange
            var attributeProvider = new Mock<ICustomAttributeProvider>();
            var attribute = new MetadataConventionsAttribute { ResourceType = typeof(TestResource) };
            attributeProvider.Setup(a => a.GetCustomAttributes(typeof(MetadataConventionsAttribute), true)).Returns(new object[] { attribute });

            // act
            var result = attributeProvider.Object.FirstOrDefault<DisplayAttribute>();

            // assert
            Assert.Null(result);
        }
    }
}
