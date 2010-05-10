#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Moq;
    using Xunit;

    public class RegisterModelMetadataTests
    {
        private readonly Mock<IModelMetadataRegistry> registry;
        private readonly Mock<FakeAdapter> adapter;

        private readonly RegisterModelMetadata registration;

        public RegisterModelMetadataTests()
        {
            registry = new Mock<IModelMetadataRegistry>();
            adapter = new Mock<FakeAdapter>();

            var buildManager = new Mock<IBuildManager>();

            buildManager.SetupGet(bm => bm.ConcreteTypes).Returns(new[] { new Mock<IModelMetadataConfiguration>().Object.GetType(), new Mock<IModelMetadataConfiguration>().Object.GetType(), new Mock<ExtendedModelMetadataProviderBase>().Object.GetType(), new Mock<ExtendedModelMetadataProviderBase>().Object.GetType() });

            adapter.Setup(a => a.RegisterType(It.IsAny<string>(), It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<LifetimeType>())).Returns(adapter.Object);

            adapter.Setup(a => a.GetInstance<IBuildManager>()).Returns(buildManager.Object);
            adapter.Setup(a => a.GetInstance<IModelMetadataRegistry>()).Returns(registry.Object);
            adapter.Setup(a => a.GetInstance<CompositeModelMetadataProvider>()).Returns(new CompositeModelMetadataProvider());
            adapter.Setup(a => a.GetAllInstances<ModelValidatorProvider>()).Returns(new[] { new ExtendedModelValidatorProvider() });
            registration = new RegisterModelMetadata();
        }

        [Fact]
        public void Should_be_able_to_register_model_metadata_and_validation_provider()
        {
            var configuration1 = new Mock<IModelMetadataConfiguration>();
            var configuration2 = new Mock<IModelMetadataConfiguration>();

            adapter.Setup(a => a.GetAllInstances<IModelMetadataConfiguration>()).Returns(new[] { configuration1.Object, configuration2.Object });
            registry.Setup(r => r.Register(It.IsAny<Type>(), It.IsAny<IDictionary<string, ModelMetadataItem>>()));

            registration.Execute(adapter.Object);

            registry.VerifyAll();

            Assert.IsType<CompositeModelMetadataProvider>(ModelMetadataProviders.Current);
            Assert.Contains(typeof(CompositeModelValidatorProvider), ModelValidatorProviders.Providers.Select(p => p.GetType()));
        }
    }
}