#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Moq;
    using Xunit;

    public class ModelMetadataRegistrarTests : IDisposable
    {
        private readonly Mock<IModelMetadataRegistry> registry;

        public ModelMetadataRegistrarTests()
        {
            ModelMetadataProviders.Current = new DataAnnotationsModelMetadataProvider();
            ModelValidatorProviders.Providers.Clear();

            registry = new Mock<IModelMetadataRegistry>();
            registry.Setup(r => r.RegisterModelProperties(It.IsAny<Type>(), It.IsAny<IDictionary<string, ModelMetadataItem>>()));
        }

        public void Dispose()
        {
            ModelMetadataProviders.Current = new DataAnnotationsModelMetadataProvider();
            ModelValidatorProviders.Providers.Clear();
        }

        [Fact]
        public void Should_be_able_to_register_validation_provider_and_model_metadata()
        {
            var modelMetadataRegistry = new ModelMetadataRegistry();
            var testRegistrar = new ModelMetadataRegistrar(modelMetadataRegistry);
            testRegistrar.ConstructMetadataUsing(() => new[] { new RegistrarTestDummyObjectConfiguration() });

            testRegistrar.Register();

            Assert.IsType<ExtendedModelMetadataProvider>(ModelMetadataProviders.Current);
            Assert.IsType<CompositeModelValidatorProvider>(ModelValidatorProviders.Providers[0]);
            var modelMetadataItem = modelMetadataRegistry.GetModelPropertiesMetadata(typeof(RegistrarTestDummyObject));
            Assert.NotNull(modelMetadataItem);
            Assert.NotEmpty(modelMetadataItem);
        }

        public class RegistrarTestDummyObject
        {
            public int DummyProperty { get; set; }
        }

        public class RegistrarTestDummyObjectConfiguration : ModelMetadataConfiguration<RegistrarTestDummyObject>
        {
            public RegistrarTestDummyObjectConfiguration()
            {
                Configure(x => x.DummyProperty);
            }
        }
    }
}
