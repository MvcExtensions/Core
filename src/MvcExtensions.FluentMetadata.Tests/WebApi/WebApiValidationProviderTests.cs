#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests.WebApi
{
    using System;
    using System.Linq;
    using System.Web.Http.Metadata;
    using System.Web.Http.Validation;
    using Moq;
    using MvcExtensions.WebApi;
    using Xunit;

    public class WebApiValidationProviderProviderTests
    {
        private readonly WebApiValidationProvider provider;

        public WebApiValidationProviderProviderTests()
        {
            provider = new WebApiValidationProvider();
        }

        [Fact]
        public void GetValidators_should_return_empty_validators_when_metadata_is_not_extended_metadata()
        {
            Func<object> accessor = () => new DummyObject();

            var metadata = new Mock<ModelMetadata>(new Mock<ModelMetadataProvider>().Object, GetType(), accessor, typeof(DummyObject), null).Object;
            var validators = provider.GetValidators(metadata, Enumerable.Empty<ModelValidatorProvider>());

            Assert.Empty(validators);
        }

        [Fact]
        public void GetValidators_should_return_validators_when_metadata_is_extended_metadata()
        {
            var builder = new ModelMetadataItemBuilder<string>(new ModelMetadataItem());

            builder.Required().AsEmail();

            var metadata = new ExtendedModelMetadata(new Mock<ModelMetadataProvider>().Object, GetType(), () => new DummyObject(), typeof(DummyObject), string.Empty, builder.Item);

            var validators = provider.GetValidators(metadata, Enumerable.Empty<ModelValidatorProvider>());

            Assert.NotEmpty(validators);
        }

        public class DummyObject
        {
            public string Property1 { get; set; }

            public int Property2 { get; set; }
        }
    }
}