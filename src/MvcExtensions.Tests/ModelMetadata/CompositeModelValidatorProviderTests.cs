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

    public class CompositeModelValidatorProviderTests
    {
        private readonly Mock<ModelValidatorProvider> provider1;
        private readonly Mock<ModelValidatorProvider> provider2;

        private readonly CompositeModelValidatorProvider compositeProvider;

        public CompositeModelValidatorProviderTests()
        {
            provider1 = new Mock<ModelValidatorProvider>();
            provider2 = new Mock<ModelValidatorProvider>();

            compositeProvider = new CompositeModelValidatorProvider(provider1.Object, provider2.Object);
        }

        [Fact]
        public void Providers_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Contains(provider1.Object, compositeProvider.Providers);
            Assert.Contains(provider2.Object, compositeProvider.Providers);
        }

        [Fact]
        public void GetValidators_should_return_validators_from_first_provider()
        {
            var validator1 = new Mock<ModelValidator>(CreateModelMetadata(), new Mock<ControllerContext>().Object);
            var validator2 = new Mock<ModelValidator>(CreateModelMetadata(), new Mock<ControllerContext>().Object);

            provider1.Setup(p => p.GetValidators(It.IsAny<ModelMetadata>(), It.IsAny<ControllerContext>())).Returns(new[] { validator1.Object });
            provider2.Setup(p => p.GetValidators(It.IsAny<ModelMetadata>(), It.IsAny<ControllerContext>())).Returns(new[] { validator2.Object });

            var validators = compositeProvider.GetValidators(CreateModelMetadata(), new Mock<ControllerContext>().Object);

            Assert.Contains(validator1.Object, validators);
            Assert.DoesNotContain(validator2.Object, validators);
        }

        [Fact]
        public void GetValidators_should_return_empty_validators_when_none_of_the_provider_contains_validators()
        {
            var validators = compositeProvider.GetValidators(CreateModelMetadata(), new Mock<ControllerContext>().Object);

            Assert.Empty(validators);
        }

        private static ModelMetadata CreateModelMetadata()
        {
            Func<object> accessor = () => new object();

            return new Mock<ModelMetadata>(new Mock<ModelMetadataProvider>().Object, typeof(object), accessor, typeof(object), "foo").Object;
        }
    }
}