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
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;
    using Xunit;

    public class BootstrapperTests
    {
        private readonly Mock<ContainerAdapter> adapter;
        private readonly Mock<IBuildManager> buildManager;
        private readonly Mock<IBootstrapperTasksRegistry> bootstrapperTasksRegistry;
        private readonly Mock<IPerRequestTasksRegistry> perRequestTasksRegistry;

        private readonly Bootstrapper bootstrapper;

        public BootstrapperTests()
        {
            buildManager = new Mock<IBuildManager>();
            bootstrapperTasksRegistry = new Mock<IBootstrapperTasksRegistry>();
            perRequestTasksRegistry = new Mock<IPerRequestTasksRegistry>();
            adapter = new Mock<ContainerAdapter>();

            adapter.Setup(a => a.RegisterType(It.IsAny<string>(), It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<LifetimeType>())).Returns(adapter.Object);
            adapter.Setup(a => a.RegisterInstance(It.IsAny<string>(), It.IsAny<Type>(), It.IsAny<object>())).Returns(adapter.Object);

            bootstrapper = new BootstrapperTestDouble(adapter, buildManager.Object, bootstrapperTasksRegistry.Object, perRequestTasksRegistry.Object);
        }

        [Fact]
        public void ExecuteBootstrapperTasks_should_execute_bootstrapper_tasks()
        {
            var task = new Mock<BootstrapperTask>();
            task.Setup(t => t.Execute()).Verifiable();

            var config = new KeyValuePair<Type, Action<object>>(task.GetType(), null);

            bootstrapperTasksRegistry.Setup(r => r.TaskConfigurations).Returns(new[] { config });
            adapter.Setup(a => a.GetService(It.IsAny<Type>())).Returns(task.Object).Verifiable();

            bootstrapper.ExecuteBootstrapperTasks();

            task.VerifyAll();
            adapter.VerifyAll();
        }

        [Fact]
        public void DisposeBootstrapperTasks_should_dispose_bootstrapper_tasks()
        {
            var task = new DummyTask();
            var config = new KeyValuePair<Type, Action<object>>(task.GetType(), null);

            bootstrapperTasksRegistry.Setup(r => r.TaskConfigurations).Returns(new[] { config });
            adapter.Setup(a => a.GetService(It.IsAny<Type>())).Returns(task).Verifiable();

            bootstrapper.DisposeBootstrapperTasks();

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
            adapter.Setup(a => a.RegisterInstance(null, typeof(IDependencyResolver), adapter.Object)).Returns(adapter.Object).Verifiable();

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
        public void Should_register_action_invoker_registry()
        {
            adapter.Setup(a => a.RegisterInstance(null, typeof(TypeMappingRegistry<Controller, IActionInvoker>), It.IsAny<object>())).Returns(adapter.Object).Verifiable();

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

            adapter.Setup(a => a.RegisterType(null, task.GetType(), task.GetType(), LifetimeType.Singleton)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_be_able_to_skip_tasks()
        {
            var task1 = new Mock<BootstrapperTask>();
            var task2 = new Mock<BootstrapperTask>();

            task1.Setup(t => t.Execute()).Returns(TaskContinuation.Skip);

            adapter.Setup(a => a.GetServices(typeof(BootstrapperTask))).Returns(new[] { task1.Object, task2.Object }).Verifiable();

            bootstrapper.ExecuteBootstrapperTasks();

            task2.Verify(t => t.Execute(), Times.Never());
        }

        [Fact]
        public void Should_be_able_to_break_tasks()
        {
            var task1 = new Mock<BootstrapperTask>();
            var task2 = new Mock<BootstrapperTask>();

            task1.Setup(t => t.Execute()).Returns(TaskContinuation.Break);

            adapter.Setup(a => a.GetServices(typeof(BootstrapperTask))).Returns(new[] { task1.Object, task2.Object }).Verifiable();

            bootstrapper.ExecuteBootstrapperTasks();

            task2.Verify(t => t.Execute(), Times.Never());
        }

        private sealed class BootstrapperTestDouble : Bootstrapper
        {
            private readonly Mock<ContainerAdapter> adapter;

            public BootstrapperTestDouble(Mock<ContainerAdapter> adapter, IBuildManager buildManager, IBootstrapperTasksRegistry bootstrapperTasksRegistry, IPerRequestTasksRegistry perRequestTasksRegistry) : base(buildManager, bootstrapperTasksRegistry, perRequestTasksRegistry)
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