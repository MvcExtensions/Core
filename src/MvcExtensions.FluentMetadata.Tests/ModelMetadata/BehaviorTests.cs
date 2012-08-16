#region Copyright
// Copyright (c) 2009 - 2011, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using System;
    using System.Linq;
    using Moq;
    using Xunit;

    public class BehaviorTests
    {
        [Fact]
        public void ShouldHaveSameConvertEmptyStringToNull()
        {
            var registryMock = new Mock<IModelMetadataRegistry>();
            registryMock.Setup(x => x.GetModelPropertiesMetadata(It.IsAny<Type>())).Returns(new DummyObjectConfiguration().Configurations);

            var provider = new ExtendedModelMetadataProvider(registryMock.Object);
            var properties = provider.GetMetadataForProperties(null, typeof(DummyObject)).ToArray();
            Assert.Equal(2, properties.Count());

            var first = properties[0];
            var second = properties[1];

            Assert.Equal(second.ConvertEmptyStringToNull, first.ConvertEmptyStringToNull);
        }

        private class DummyObject
        {
            public string Item1
            {
                get;
                set;
            }

            public string Item2
            {
                get;
                set;
            }
        }

        private class DummyObjectConfiguration : ModelMetadataConfiguration<DummyObject>
        {
            public DummyObjectConfiguration()
            {
                Configure(x => x.Item1)
                    .DisplayName("SomeName");
            }
        }
    }
}