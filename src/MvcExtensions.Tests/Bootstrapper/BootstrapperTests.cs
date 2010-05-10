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
    using System.Web.Routing;

    using Microsoft.Practices.ServiceLocation;

    using Moq;
    using Xunit;

    public class BootstrapperTests
    {
        private readonly Mock<FakeAdapter> adapter;
        private readonly Mock<IBuildManager> buildManager;
        private readonly Bootstrapper bootstrapper;

        public BootstrapperTests()
        {
            buildManager = new Mock<IBuildManager>();
            adapter = new Mock<FakeAdapter>();

            adapter.Setup(a => a.RegisterType(It.IsAny<string>(), It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<LifetimeType>())).Returns(adapter.Object);
            adapter.Setup(a => a.RegisterInstance(It.IsAny<string>(), It.IsAny<Type>(), It.IsAny<object>())).Returns(adapter.Object);

            bootstrapper = new BootstrapperTestDouble(adapter, buildManager.Object);
        }

        [Fact]
        public void Execute_should_execute_bootstrapper_tasks()
        {
            var task = new Mock<IBootstrapperTask>();
            task.Setup(t => t.Execute(adapter.Object)).Verifiable();

            adapter.Setup(sl => sl.GetAllInstances<IBootstrapperTask>()).Returns(new[] { task.Object }).Verifiable();

            bootstrapper.Execute();

            task.VerifyAll();
            adapter.VerifyAll();
        }

        [Fact]
        public void Dispose_should_dispose_bootstrapper_tasks()
        {
            var task = new Mock<IBootstrapperTask>();
            task.Setup(t => t.Dispose()).Verifiable();

            adapter.Setup(sl => sl.GetAllInstances<IBootstrapperTask>()).Returns(new[] { task.Object }).Verifiable();

            Assert.NotNull(bootstrapper.ServiceLocator);

            bootstrapper.Dispose();

            task.VerifyAll();
            adapter.VerifyAll();
        }

        [Fact]
        public void ServiceLocator_should_be_set()
        {
            Assert.Same(adapter.Object, bootstrapper.ServiceLocator);
            Assert.Same(adapter.Object, ServiceLocator.Current);
        }

        [Fact]
        public void Should_register_route_collection()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(RouteCollection), RouteTable.Routes)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.ServiceLocator);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_controller_builder()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(ControllerBuilder), ControllerBuilder.Current)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.ServiceLocator);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_model_binder_dictionary()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(ModelBinderDictionary), ModelBinders.Binders)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.ServiceLocator);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_view_engine_collection()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(ViewEngineCollection), ViewEngines.Engines)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.ServiceLocator);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_build_manager()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(IBuildManager), buildManager.Object)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.ServiceLocator);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_registrar()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(IServiceRegistrar), adapter.Object)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.ServiceLocator);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_service_locator()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(IServiceLocator), adapter.Object)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.ServiceLocator);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_injector()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(IServiceInjector), adapter.Object)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.ServiceLocator);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_filter_registry_as_singleton()
        {
            adapter.Setup(a => a.RegisterType(null, typeof(IFilterRegistry), typeof(FilterRegistry), LifetimeType.Singleton)).Returns(adapter.Object);

            Assert.NotNull(bootstrapper.ServiceLocator);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_value_provider_factories_as_singleton()
        {
            adapter.Setup(a => a.RegisterType(null, typeof(ValueProviderFactoryCollection), typeof(ValueProviderFactoryCollection), LifetimeType.Singleton)).Returns(adapter.Object);

            Assert.NotNull(bootstrapper.ServiceLocator);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_bootstrapper_tasks_as_singleton()
        {
            buildManager.Setup(bm => bm.ConcreteTypes).Returns(new[] { typeof(TestBootstrapperTask) });

            adapter.Setup(a => a.RegisterType(null, typeof(IBootstrapperTask), typeof(TestBootstrapperTask), LifetimeType.Singleton)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.ServiceLocator);

            adapter.Verify();
        }

        [Fact]
        public void Should_be_able_to_skip_tasks()
        {
            var task1 = new Mock<IBootstrapperTask>();
            var task2 = new Mock<IBootstrapperTask>();

            task1.Setup(t => t.Execute(It.IsAny<IServiceLocator>())).Returns(TaskContinuation.Skip);

            adapter.Setup(sl => sl.GetAllInstances<IBootstrapperTask>()).Returns(new[] { task1.Object, task2.Object }).Verifiable();

            bootstrapper.Execute();

            task2.Verify(t => t.Execute(It.IsAny<IServiceLocator>()), Times.Never());
        }

        [Fact]
        public void Should_be_able_to_break_tasks()
        {
            var task1 = new Mock<IBootstrapperTask>();
            var task2 = new Mock<IBootstrapperTask>();

            task1.Setup(t => t.Execute(It.IsAny<IServiceLocator>())).Returns(TaskContinuation.Break);

            adapter.Setup(sl => sl.GetAllInstances<IBootstrapperTask>()).Returns(new[] { task1.Object, task2.Object }).Verifiable();

            bootstrapper.Execute();

            task2.Verify(t => t.Execute(It.IsAny<IServiceLocator>()), Times.Never());
        }

        private sealed class BootstrapperTestDouble : Bootstrapper
        {
            private readonly Mock<FakeAdapter> adapter;

            public BootstrapperTestDouble(Mock<FakeAdapter> adapter, IBuildManager buildManager) : base(buildManager)
            {
                this.adapter = adapter;
            }

            protected override IServiceLocator CreateServiceLocator()
            {
                return adapter.Object;
            }

            protected override void LoadModules()
            {
            }
        }

        private sealed class TestBootstrapperTask : IBootstrapperTask
        {
            public int Order
            {
                get
                {
                    return int.MaxValue;
                }
            }

            public void Dispose()
            {
            }

            public TaskContinuation Execute(IServiceLocator serviceLocator)
            {
                return TaskContinuation.Continue;
            }
        }
    }
}