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
        private readonly Mock<ContainerAdapter> adapter;
        private readonly Mock<IBuildManager> buildManager;
        private readonly Bootstrapper bootstrapper;

        public BootstrapperTests()
        {
            buildManager = new Mock<IBuildManager>();
            adapter = new Mock<ContainerAdapter>();

            adapter.Setup(a => a.RegisterType(It.IsAny<string>(), It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<LifetimeType>())).Returns(adapter.Object);
            adapter.Setup(a => a.RegisterInstance(It.IsAny<string>(), It.IsAny<Type>(), It.IsAny<object>())).Returns(adapter.Object);

            bootstrapper = new BootstrapperTestDouble(adapter, buildManager.Object);
        }

        [Fact]
        public void Execute_should_execute_bootstrapper_tasks()
        {
            var task = new Mock<BootstrapperTask>();
            task.Setup(t => t.Execute()).Verifiable();

            adapter.Setup(a => a.GetAllInstances<BootstrapperTask>()).Returns(new[] { task.Object }).Verifiable();

            bootstrapper.Execute();

            task.VerifyAll();
            adapter.VerifyAll();
        }

        [Fact]
        public void Dispose_should_dispose_bootstrapper_tasks()
        {
            var task = new DummyTask();

            adapter.Setup(a => a.GetAllInstances<BootstrapperTask>()).Returns(new[] { task }).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            bootstrapper.Dispose();

            Assert.True(task.Disposed);
        }

        [Fact]
        public void Container_should_be_set()
        {
            Assert.Same(adapter.Object, bootstrapper.Adapter);
        }

        [Fact]
        public void Should_register_route_collection()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(RouteCollection), RouteTable.Routes)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_controller_builder()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(ControllerBuilder), ControllerBuilder.Current)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_model_binder_dictionary()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(ModelBinderDictionary), ModelBinders.Binders)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_view_engine_collection()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(ViewEngineCollection), ViewEngines.Engines)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_build_manager()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(IBuildManager), buildManager.Object)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_registrar()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(IServiceRegistrar), adapter.Object)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_service_locator()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(IServiceLocator), adapter.Object)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_injector()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(IServiceInjector), adapter.Object)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_adapter()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(ContainerAdapter), adapter.Object)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_filter_registry_as_singleton()
        {
            adapter.Setup(a => a.RegisterType(null, typeof(IFilterRegistry), typeof(FilterRegistry), LifetimeType.Singleton)).Returns(adapter.Object);

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_action_invoker_registry_as_singleton()
        {
            adapter.Setup(a => a.RegisterType(null, typeof(IActionInvokerRegistry), typeof(ActionInvokerRegistry), LifetimeType.Singleton)).Returns(adapter.Object);

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_value_provider_factories_as_singleton()
        {
            adapter.Setup(a => a.RegisterType(null, typeof(ValueProviderFactoryCollection), typeof(ValueProviderFactoryCollection), LifetimeType.Singleton)).Returns(adapter.Object);

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_bootstrapper_tasks_as_singleton()
        {
            var task = new Mock<BootstrapperTask>().Object;

            buildManager.Setup(bm => bm.ConcreteTypes).Returns(new[] { task.GetType() });

            adapter.Setup(a => a.RegisterType(null, typeof(BootstrapperTask), task.GetType(), LifetimeType.Singleton)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_be_able_to_skip_tasks()
        {
            var task1 = new Mock<BootstrapperTask>();
            var task2 = new Mock<BootstrapperTask>();

            task1.Setup(t => t.Execute()).Returns(TaskContinuation.Skip);

            adapter.Setup(a => a.GetAllInstances<BootstrapperTask>()).Returns(new[] { task1.Object, task2.Object }).Verifiable();

            bootstrapper.Execute();

            task2.Verify(t => t.Execute(), Times.Never());
        }

        [Fact]
        public void Should_be_able_to_break_tasks()
        {
            var task1 = new Mock<BootstrapperTask>();
            var task2 = new Mock<BootstrapperTask>();

            task1.Setup(t => t.Execute()).Returns(TaskContinuation.Break);

            adapter.Setup(a => a.GetAllInstances<BootstrapperTask>()).Returns(new[] { task1.Object, task2.Object }).Verifiable();

            bootstrapper.Execute();

            task2.Verify(t => t.Execute(), Times.Never());
        }

        private sealed class BootstrapperTestDouble : Bootstrapper
        {
            private readonly Mock<ContainerAdapter> adapter;

            public BootstrapperTestDouble(Mock<ContainerAdapter> adapter, IBuildManager buildManager) : base(buildManager)
            {
                this.adapter = adapter;
            }

            protected override ContainerAdapter CreateAdapter()
            {
                return adapter.Object;
            }

            protected override void LoadModules()
            {
            }
        }

        private sealed class DummyTask : BootstrapperTask
        {
            public bool Disposed { get; private set; }

            public override TaskContinuation Execute()
            {
                return TaskContinuation.Continue;
            }

            protected override void DisposeCore()
            {
                base.DisposeCore();
                Disposed = true;
            }
        }
    }
}