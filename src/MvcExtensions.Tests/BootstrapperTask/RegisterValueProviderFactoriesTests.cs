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

    public class RegisterValueProviderFactoriesTests : IDisposable
    {
        public RegisterValueProviderFactoriesTests()
        {
            RegisterValueProviderFactories.Excluded = false;
            RegisterValueProviderFactories.IgnoredTypes.Clear();
        }

        public void Dispose()
        {
            RegisterValueProviderFactories.Excluded = false;
            RegisterValueProviderFactories.IgnoredTypes.Clear();
        }

        [Fact]
        public void Should_register_available_value_provider_factories()
        {
            var valueProviderFactories = new ValueProviderFactoryCollection();

            new RegisterValueProviderFactories(valueProviderFactories).Execute(SetupAdapter().Object);

            Assert.Equal(1, valueProviderFactories.Count);
        }

        [Fact]
        public void Should_not_register_value_provider_factory_when_excluded()
        {
            var valueProviderFactories = new ValueProviderFactoryCollection();

            RegisterValueProviderFactories.Excluded = true;

            new RegisterValueProviderFactories(valueProviderFactories).Execute(SetupAdapter().Object);

            Assert.Empty(valueProviderFactories);
        }

        [Fact]
        public void Should_not_register_value_provider_factory_when_factory_exists_in_ignored_list()
        {
            var adapter = SetupAdapter();

            RegisterValueProviderFactories.IgnoredTypes.Add(typeof(DummyValueProviderFactory));

            new RegisterValueProviderFactories(new ValueProviderFactoryCollection()).Execute(adapter.Object);

            adapter.Verify(a => a.RegisterType(null, typeof(ValueProviderFactory), typeof(DummyValueProviderFactory), LifetimeType.Singleton), Times.Never());
        }

        [Fact]
        public void Should_not_register_already_registered_factory()
        {
            var factories = new ValueProviderFactoryCollection { new DummyValueProviderFactory() };

            new RegisterValueProviderFactories(factories).Execute(SetupAdapter().Object);

            Assert.Equal(1, factories.Count(f => f.GetType() == typeof(DummyValueProviderFactory)));
        }

        private static Mock<FakeAdapter> SetupAdapter()
        {
            var buildManager = new Mock<IBuildManager>();
            buildManager.Setup(bm => bm.ConcreteTypes).Returns(new[] { typeof(DummyValueProviderFactory) });

            var adapter = new Mock<FakeAdapter>();

            adapter.Setup(a => a.GetInstance<IBuildManager>()).Returns(buildManager.Object);

            var factories = new List<ValueProviderFactory>();

            adapter.Setup(a => a.RegisterType(null, It.IsAny<Type>(), It.IsAny<Type>(), LifetimeType.Singleton)).Callback((string k, Type t1, Type t2, LifetimeType lt) => factories.Add((ValueProviderFactory)Activator.CreateInstance(t2)));
            adapter.Setup(a => a.GetAllInstances<ValueProviderFactory>()).Returns(() => factories);

            return adapter;
        }

        private sealed class DummyValueProviderFactory : ValueProviderFactory
        {
            public override IValueProvider GetValueProvider(ControllerContext controllerContext)
            {
                return null;
            }
        }
    }
}