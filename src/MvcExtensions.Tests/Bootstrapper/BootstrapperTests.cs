#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Moq;
    using Xunit;

    public class BootstrapperTests : IDisposable
    {
        private readonly Mock<ContainerAdapter> adapter;
        private readonly Bootstrapper bootstrapper;
        private readonly Mock<IBootstrapperTasksRegistry> bootstrapperTasksRegistry;
        private readonly Mock<IBuildManager> buildManager;
        private readonly Mock<IPerRequestTasksRegistry> perRequestTasksRegistry;

        public BootstrapperTests()
        {
            buildManager = new Mock<IBuildManager>();
            bootstrapperTasksRegistry = new Mock<IBootstrapperTasksRegistry>();
            perRequestTasksRegistry = new Mock<IPerRequestTasksRegistry>();
            adapter = new Mock<ContainerAdapter>();

            adapter.Setup(a => a.RegisterType(It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<LifetimeType>())).Returns(adapter.Object);
            adapter.Setup(a => a.RegisterInstance(It.IsAny<Type>(), It.IsAny<object>())).Returns(adapter.Object);

            bootstrapper = new BootstrapperTestDouble(adapter, buildManager.Object, bootstrapperTasksRegistry.Object, perRequestTasksRegistry.Object);
        }

        public void Dispose()
        {
            DependencyResolver.SetResolver(new DefaultDependencyResolver());
        }

        [Fact]
        public void Container_should_be_set()
        {
            Assert.Same(adapter.Object, bootstrapper.Adapter);
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
        public void Should_be_able_to_break_tasks()
        {
            var task1 = new Mock<BootstrapperTask>();
            var task2 = new Mock<BootstrapperTask>();

            task1.Setup(t => t.Execute()).Returns(TaskContinuation.Break);

            adapter.Setup(a => a.GetServices(typeof(BootstrapperTask))).Returns(new[] { task1.Object, task2.Object }).Verifiable();

            bootstrapper.ExecuteBootstrapperTasks();

            task2.Verify(t => t.Execute(), Times.Never());
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
        public void Should_register_action_invoker_registry()
        {
            adapter.Setup(a => a.RegisterInstance(typeof(TypeMappingRegistry<Controller, IActionInvoker>), It.IsAny<object>())).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_adapter()
        {
            adapter.Setup(a => a.RegisterInstance(typeof(ContainerAdapter), adapter.Object)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_bootstrapper_tasks_as_singleton()
        {
            var task = new Mock<BootstrapperTask>().Object;

            buildManager.Setup(bm => bm.ConcreteTypes).Returns(new[] { task.GetType() });

            adapter.Setup(a => a.RegisterType(task.GetType(), task.GetType(), LifetimeType.Singleton)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_build_manager()
        {
            adapter.Setup(a => a.RegisterInstance(typeof(IBuildManager), buildManager.Object)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_filter_registry_as_singleton()
        {
            adapter.Setup(a => a.RegisterType(typeof(IFilterRegistry), typeof(FilterRegistry), LifetimeType.Singleton)).Returns(adapter.Object);

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_injector()
        {
            adapter.Setup(a => a.RegisterInstance(typeof(IServiceInjector), adapter.Object)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_registrar()
        {
            adapter.Setup(a => a.RegisterInstance(typeof(IServiceRegistrar), adapter.Object)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_route_collection()
        {
            adapter.Setup(a => a.RegisterInstance(typeof(RouteCollection), RouteTable.Routes)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_service_locator()
        {
            adapter.Setup(a => a.RegisterInstance(typeof(IDependencyResolver), adapter.Object)).Returns(adapter.Object).Verifiable();

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        [Fact]
        public void Should_register_value_provider_factories_as_singleton()
        {
            adapter.Setup(a => a.RegisterType(typeof(ValueProviderFactoryCollection), typeof(ValueProviderFactoryCollection), LifetimeType.Singleton)).Returns(
                adapter.Object);

            Assert.NotNull(bootstrapper.Adapter);

            adapter.Verify();
        }

        private sealed class BootstrapperTestDouble : Bootstrapper
        {
            private readonly Mock<ContainerAdapter> adapter;

            public BootstrapperTestDouble(
                Mock<ContainerAdapter> adapter,
                IBuildManager buildManager,
                IBootstrapperTasksRegistry bootstrapperTasksRegistry,
                IPerRequestTasksRegistry perRequestTasksRegistry) : base(buildManager, bootstrapperTasksRegistry, perRequestTasksRegistry)
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

        private class DefaultDependencyResolver : IDependencyResolver
        {
            [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
                Justification = "This method might throw exceptions whose type we cannot strongly link against; namely, ActivationException from common service locator")]
            public object GetService(Type serviceType)
            {
                try
                {
                    return Activator.CreateInstance(serviceType);
                }
                catch
                {
                    return null;
                }
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return Enumerable.Empty<object>();
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
