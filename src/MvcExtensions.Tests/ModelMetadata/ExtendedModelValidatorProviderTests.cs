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

    public class ExtendedModelValidatorProviderTests
    {
        private readonly ExtendedModelValidatorProvider provider;

        public ExtendedModelValidatorProviderTests()
        {
            provider = new ExtendedModelValidatorProvider();
        }

        [Fact]
        public void GetValidators_should_return_empty_validators_when_metadata_is_not_extended_metadata()
        {
            Func<object> accessor = () => new DummyObject();

            var validators = provider.GetValidators(new Mock<ModelMetadata>(new Mock<ModelMetadataProvider>().Object, GetType(), accessor, typeof(DummyObject), null).Object, new ControllerContext());

            Assert.Empty(validators);
        }

        [Fact]
        public void GetValidators_should_return_validators_when_metadata_is_extended_metadata()
        {
            var builder = new StringMetadataItemBuilder(new ModelMetadataItem());

            builder.Required().AsEmail();

            var metadata = new ExtendedModelMetadata(new Mock<ModelMetadataProvider>().Object, GetType(), () => new DummyObject(), typeof(DummyObject), string.Empty, builder.Item);

            var validators = provider.GetValidators(metadata, new ControllerContext());

            Assert.NotEmpty(validators);
        }

        public class DummyObject
        {
            public string Property1 { get; set; }

            public int Property2 { get; set; }
        }
    }
}