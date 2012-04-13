#region Copyright
// Copyright (c) 2009 - 2011, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Moq;

    using Xunit;

    public class ModelMetadataRegistratorTests
    {
        private readonly Mock<IModelMetadataRegistry> registry;
        private readonly Mock<IDependencyResolver> resolver;
        private readonly Mock<IBuildManager> buildManager;
        private readonly ModelMetadataRegistrator registrator;

        public ModelMetadataRegistratorTests()
        {
            ModelMetadataProviders.Current = new DataAnnotationsModelMetadataProvider();
            ModelValidatorProviders.Providers.Clear();

            buildManager = new Mock<IBuildManager>();
            registry = new Mock<IModelMetadataRegistry>();
            resolver = new Mock<IDependencyResolver>();

            registrator = new ModelMetadataRegistrator(buildManager.Object);

            var configuration1 = new Mock<IModelMetadataConfiguration>();
            var configuration2 = new Mock<IModelMetadataConfiguration>();

            resolver.Setup(a => a.GetService(typeof(IBuildManager))).Returns(buildManager.Object);
            resolver.Setup(a => a.GetService(typeof(IModelMetadataRegistry))).Returns(registry.Object);
            resolver.Setup(a => a.GetServices(typeof(IModelMetadataConfiguration))).Returns(new[] { configuration1.Object, configuration2.Object });

            registry.Setup(r => r.RegisterModelProperties(It.IsAny<Type>(), It.IsAny<IDictionary<string, ModelMetadataItem>>()));

            buildManager.SetupGet(bm => bm.ConcreteTypes).Returns(new[] { new Mock<IModelMetadataConfiguration>().Object.GetType(), new Mock<IModelMetadataConfiguration>().Object.GetType() });
        }

        public void Dispose()
        {
            ModelMetadataProviders.Current = new DataAnnotationsModelMetadataProvider();
            ModelValidatorProviders.Providers.Clear();
        }

        [Fact]
        public void Should_be_able_to_register_model_metadata_and_validation_provider()
        {
            int registeredTransients = 0;
            int registeredSingletons = 0;

            registrator.RegisterMetadataTypes((s, i) => { registeredTransients++; }, (s, i) => { registeredSingletons++; });
            registrator.RegisterMetadataProviders(resolver.Object);

            Assert.Equal(2, registeredTransients);
            Assert.Equal(1, registeredSingletons);
            Assert.IsType<ExtendedModelMetadataProvider>(ModelMetadataProviders.Current);
            Assert.IsType<CompositeModelValidatorProvider>(ModelValidatorProviders.Providers[0]);
        }

        [Fact]
        public void Should_be_able_to_register_model_metadata_and_validation_provider_via_singleton_instance()
        {
            Assert.NotNull(ModelMetadataRegistrator.Current);
        }

        [Fact]
        public void Should_be_able_to_register_model_metadata_and_validation_provider_via_singleton_instance_only_once()
        {
            registrator.RegisterMetadataTypes((s, i) => {  }, (s, i) => {  });
            Assert.ThrowsDelegate action = () => registrator.RegisterMetadataTypes((s, i) => {  }, (s, i) => {  });

            Assert.Throws<InvalidOperationException>(action);
        }
    }
}