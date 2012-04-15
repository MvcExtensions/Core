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

    public class ModelMetadataRegistrarTests : IDisposable
    {
        private readonly Mock<IModelMetadataRegistry> registry;
        private readonly Mock<IDependencyResolver> resolver;
        private readonly Mock<IBuildManager> buildManager;
        private readonly ModelMetadataRegistrar registrar;

        public ModelMetadataRegistrarTests()
        {
            ModelMetadataProviders.Current = new DataAnnotationsModelMetadataProvider();
            ModelValidatorProviders.Providers.Clear();

            buildManager = new Mock<IBuildManager>();
            registry = new Mock<IModelMetadataRegistry>();
            resolver = new Mock<IDependencyResolver>();

            registrar = new ModelMetadataRegistrar(resolver.Object);

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
            registrar.RegisterMetadataProviders();

            Assert.IsType<ExtendedModelMetadataProvider>(ModelMetadataProviders.Current);
            Assert.IsType<CompositeModelValidatorProvider>(ModelValidatorProviders.Providers[0]);
        }
    }
}