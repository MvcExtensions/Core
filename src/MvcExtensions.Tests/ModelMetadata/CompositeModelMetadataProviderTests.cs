#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;
    using System.Web.Mvc;

    using Moq;
    using Xunit;

    public class CompositeModelMetadataProviderTests
    {
        private readonly Mock<ExtendedModelMetadataProviderBase> provider1;
        private readonly Mock<ExtendedModelMetadataProviderBase> provider2;

        private readonly CompositeModelMetadataProvider compositeProvider;

        public CompositeModelMetadataProviderTests()
        {
            provider1 = new Mock<ExtendedModelMetadataProviderBase>();
            provider2 = new Mock<ExtendedModelMetadataProviderBase>();

            compositeProvider = new CompositeModelMetadataProvider(provider1.Object, provider2.Object);
        }

        [Fact]
        public void Providers_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Contains(provider1.Object, compositeProvider.Providers);
            Assert.Contains(provider2.Object, compositeProvider.Providers);
        }

        [Fact]
        public void DefaultProvider_should_be_data_annotations_provider()
        {
            Assert.IsType<DataAnnotationsModelMetadataProvider>(compositeProvider.DefaultProvider);
        }

        [Fact]
        public void GetMetadataForProperties_should_return_from_last_matching_provider()
        {
            provider1.Setup(p => p.IsRegistered(typeof(DummyObject))).Returns(true);
            provider2.Setup(p => p.IsRegistered(typeof(DummyObject))).Returns(true);

            provider2.Setup(p => p.GetMetadataForProperties(It.IsAny<object>(), It.IsAny<Type>())).Verifiable();

            compositeProvider.GetMetadataForProperties(this, typeof(DummyObject));

            provider2.Verify();
        }

        [Fact]
        public void GetMetadataForProperties_should_return_from_default_provider_when_none_of_the_providers_matches()
        {
            var defaultProvider = new Mock<ModelMetadataProvider>();
            compositeProvider.DefaultProvider = defaultProvider.Object;

            defaultProvider.Setup(p => p.GetMetadataForProperties(It.IsAny<object>(), It.IsAny<Type>())).Verifiable();

            compositeProvider.GetMetadataForProperties(this, typeof(DummyObject));

            defaultProvider.Verify();
        }

        [Fact]
        public void GetMetadataForProperty_should_return_from_last_matching_provider()
        {
            provider1.Setup(p => p.IsRegistered(typeof(CompositeModelMetadataProviderTests), It.IsAny<string>())).Returns(true);
            provider2.Setup(p => p.IsRegistered(typeof(CompositeModelMetadataProviderTests), It.IsAny<string>())).Returns(true);

            provider2.Setup(p => p.GetMetadataForProperty(It.IsAny<Func<object>>(), It.IsAny<Type>(), It.IsAny<string>())).Verifiable();

            compositeProvider.GetMetadataForProperty(() => new DummyObject(), GetType(), "Property1");

            provider2.Verify();
        }

        [Fact]
        public void GetMetadataForProperty_should_return_from_default_provider_when_none_of_the_providers_matches()
        {
            var defaultProvider = new Mock<ModelMetadataProvider>();
            compositeProvider.DefaultProvider = defaultProvider.Object;

            defaultProvider.Setup(p => p.GetMetadataForProperty(It.IsAny<Func<object>>(), It.IsAny<Type>(), It.IsAny<string>())).Verifiable();

            compositeProvider.GetMetadataForProperty(() => new DummyObject(), GetType(), "Property1");

            defaultProvider.Verify();
        }

        [Fact]
        public void GetMetadataForType_should_return_from_last_matching_provider()
        {
            provider1.Setup(p => p.IsRegistered(typeof(DummyObject))).Returns(true);
            provider2.Setup(p => p.IsRegistered(typeof(DummyObject))).Returns(true);

            provider2.Setup(p => p.GetMetadataForType(It.IsAny<Func<object>>(), It.IsAny<Type>())).Verifiable();

            compositeProvider.GetMetadataForType(() => new DummyObject(), typeof(DummyObject));

            provider2.Verify();
        }

        [Fact]
        public void GetMetadataForType_should_return_from_default_provider_when_none_of_the_providers_matches()
        {
            var defaultProvider = new Mock<ModelMetadataProvider>();
            compositeProvider.DefaultProvider = defaultProvider.Object;

            defaultProvider.Setup(p => p.GetMetadataForType(It.IsAny<Func<object>>(), It.IsAny<Type>())).Verifiable();

            compositeProvider.GetMetadataForType(() => new DummyObject(), typeof(DummyObject));

            defaultProvider.Verify();
        }

        private sealed class DummyObject
        {
            public string Property1 { get; set; }

            public int Property2 { get; set; }
        }
    }
}