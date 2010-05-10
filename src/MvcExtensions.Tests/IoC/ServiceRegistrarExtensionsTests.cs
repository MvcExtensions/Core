#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;

    using Moq;
    using Xunit;

    public class ServiceRegistrarExtensionsTests
    {
        private readonly Mock<IServiceRegistrar> registrar;

        public ServiceRegistrarExtensionsTests()
        {
            registrar = new Mock<IServiceRegistrar>();
        }

        [Fact]
        public void Should_be_able_to_register_instance_as_service_type()
        {
            var instance = new object();

            registrar.Setup(r => r.RegisterInstance(null, typeof(object), instance));

            registrar.Object.RegisterInstance<object>(instance);

            registrar.VerifyAll();
        }

        [Fact]
        public void Should_be_able_to_register_instance()
        {
            var instance = new object();

            registrar.Setup(r => r.RegisterInstance(null, typeof(object), instance));

            registrar.Object.RegisterInstance(instance);

            registrar.VerifyAll();
        }

        [Fact]
        public void Should_be_able_to_register_implementation_as_per_request()
        {
            registrar.Setup(r => r.RegisterType(null, typeof(object), typeof(object), LifetimeType.PerRequest));

            registrar.Object.RegisterAsPerRequest<object>();

            registrar.VerifyAll();
        }

        [Fact]
        public void Should_be_able_to_register_service_as_per_request()
        {
            registrar.Setup(r => r.RegisterType(null, typeof(IServiceRegistrar), typeof(ServiceRegistrar), LifetimeType.PerRequest));

            registrar.Object.RegisterAsPerRequest<IServiceRegistrar, ServiceRegistrar>();

            registrar.VerifyAll();
        }

        [Fact]
        public void Should_be_able_to_register_implementation_type_as_per_request()
        {
            registrar.Setup(r => r.RegisterType(null, typeof(object), typeof(object), LifetimeType.PerRequest));

            registrar.Object.RegisterAsPerRequest(typeof(object));

            registrar.VerifyAll();
        }

        [Fact]
        public void Should_be_able_to_register_service_type_as_per_request()
        {
            registrar.Setup(r => r.RegisterType(null, typeof(IServiceRegistrar), typeof(ServiceRegistrar), LifetimeType.PerRequest));

            registrar.Object.RegisterAsPerRequest(typeof(IServiceRegistrar), typeof(ServiceRegistrar));

            registrar.VerifyAll();
        }

        [Fact]
        public void Should_be_able_to_register_implementation_as_singleton()
        {
            registrar.Setup(r => r.RegisterType(null, typeof(object), typeof(object), LifetimeType.Singleton));

            registrar.Object.RegisterAsSingleton<object>();

            registrar.VerifyAll();
        }

        [Fact]
        public void Should_be_able_to_register_service_as_singleton()
        {
            registrar.Setup(r => r.RegisterType(null, typeof(IServiceRegistrar), typeof(ServiceRegistrar), LifetimeType.Singleton));

            registrar.Object.RegisterAsSingleton<IServiceRegistrar, ServiceRegistrar>();

            registrar.VerifyAll();
        }

        [Fact]
        public void Should_be_able_to_register_implementation_type_as_singleton()
        {
            registrar.Setup(r => r.RegisterType(null, typeof(object), typeof(object), LifetimeType.Singleton));

            registrar.Object.RegisterAsSingleton(typeof(object));

            registrar.VerifyAll();
        }

        [Fact]
        public void Should_be_able_to_register_service_type_as_singleton()
        {
            registrar.Setup(r => r.RegisterType(null, typeof(IServiceRegistrar), typeof(ServiceRegistrar), LifetimeType.Singleton));

            registrar.Object.RegisterAsSingleton(typeof(IServiceRegistrar), typeof(ServiceRegistrar));

            registrar.VerifyAll();
        }

        [Fact]
        public void Should_be_able_to_register_implementation_as_transient()
        {
            registrar.Setup(r => r.RegisterType(null, typeof(object), typeof(object), LifetimeType.Transient));

            registrar.Object.RegisterAsTransient<object>();

            registrar.VerifyAll();
        }

        [Fact]
        public void Should_be_able_to_register_service_as_transient()
        {
            registrar.Setup(r => r.RegisterType(null, typeof(IServiceRegistrar), typeof(ServiceRegistrar), LifetimeType.Transient));

            registrar.Object.RegisterAsTransient<IServiceRegistrar, ServiceRegistrar>();

            registrar.VerifyAll();
        }

        [Fact]
        public void Should_be_able_to_register_implementation_type_as_transient()
        {
            registrar.Setup(r => r.RegisterType(null, typeof(object), typeof(object), LifetimeType.Transient));

            registrar.Object.RegisterAsTransient(typeof(object));

            registrar.VerifyAll();
        }

        [Fact]
        public void Should_be_able_to_register_service_type_as_transient()
        {
            registrar.Setup(r => r.RegisterType(null, typeof(IServiceRegistrar), typeof(ServiceRegistrar), LifetimeType.Transient));

            registrar.Object.RegisterAsTransient(typeof(IServiceRegistrar), typeof(ServiceRegistrar));

            registrar.VerifyAll();
        }

        private sealed class ServiceRegistrar : IServiceRegistrar
        {
            public IServiceRegistrar RegisterType(string key, Type serviceType, Type implementationType, LifetimeType lifetime)
            {
                return null;
            }

            public IServiceRegistrar RegisterInstance(string key, Type serviceType, object instance)
            {
                return null;
            }
        }
    }
}