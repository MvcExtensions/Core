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
    using System.Reflection;
    using System.Web.Mvc;
    using Moq;
    using Xunit;

    public class RegisterModelMetadataTests : IDisposable
    {
        private readonly Mock<IModelMetadataRegistry> registry;
        private readonly Mock<ContainerAdapter> adapter;

        private readonly RegisterModelMetadata registration;

        public RegisterModelMetadataTests()
        {
            ModelMetadataProviders.Current = new DataAnnotationsModelMetadataProvider();
            ModelValidatorProviders.Providers.Clear();

            registry = new Mock<IModelMetadataRegistry>();
            adapter = new Mock<ContainerAdapter>();

            registration = new RegisterModelMetadata(adapter.Object);
        }

        public void Dispose()
        {
            ModelMetadataProviders.Current = new DataAnnotationsModelMetadataProvider();
            ModelValidatorProviders.Providers.Clear();
        }

        [Fact]
        public void Should_be_able_to_register_model_metadata_and_validation_provider()
        {
            var configuration = new DummyObjectConfiguration();

            var buildManager = new Mock<IBuildManager>();
            buildManager.SetupGet(bm => bm.Assemblies).Returns(new[] { Assembly.GetExecutingAssembly() });
            
            adapter.Setup(a => a.RegisterType(It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<LifetimeType>())).Returns(adapter.Object);
            adapter.Setup(a => a.GetService(typeof(IBuildManager))).Returns(buildManager.Object);
            adapter.Setup(a => a.GetService(typeof(IModelMetadataRegistry))).Returns(registry.Object);
            adapter.Setup(a => a.GetServices(typeof(IModelMetadataConfiguration))).Returns(new[] { configuration });
            
            registry.Setup(r => r.RegisterModelProperties(It.IsAny<Type>(), It.IsAny<IDictionary<string, Func<ModelMetadataItem>>>()));

            registration.Execute();

            Assert.IsType<ExtendedModelMetadataProvider>(ModelMetadataProviders.Current);
            Assert.IsType<CompositeModelValidatorProvider>(ModelValidatorProviders.Providers[0]);
        }

        public class DummyObjectConfiguration : ModelMetadataConfiguration<DummyObject>
        {
            public DummyObjectConfiguration()
            {
                Configure(x => x.DummyProperty);
            }
        }

        public class DummyObject
        {
            public int DummyProperty { get; set; }
        }
    }
}