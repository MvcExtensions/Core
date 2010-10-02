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
    using System.Reflection;
    using System.Web;

    using Moq;
    using Xunit;

    public class ExtendedMvcApplicationTests : IDisposable
    {
        private static readonly FieldInfo privateBootStrapper = typeof(ExtendedMvcApplication).GetFields(BindingFlags.Static | BindingFlags.NonPublic).Single(f => f.Name == "bootstrapper");

        private readonly ExtendedMvcApplicationTestDouble httpApplication;
        private readonly Mock<IBootstrapper> bootstrapper;
        private readonly Mock<ContainerAdapter> container;

        public ExtendedMvcApplicationTests()
        {
            container = new Mock<ContainerAdapter>();

            bootstrapper = new Mock<IBootstrapper>();
            bootstrapper.Setup(bs => bs.Adapter).Returns(container.Object);

            httpApplication = new ExtendedMvcApplicationTestDouble(bootstrapper.Object);
        }

        public void Dispose()
        {
            privateBootStrapper.SetValue(httpApplication, null);
        }

        [Fact]
        public void Bootstrapper_should_not_be_null()
        {
            Assert.NotNull(httpApplication.Bootstrapper);
        }

        [Fact]
        public void Should_be_able_to_init()
        {
            Assert.DoesNotThrow(() => httpApplication.Init());
        }

        [Fact]
        public void Application_start_should_execute_bootstrapper()
        {
            httpApplication.Application_Start();

            bootstrapper.Verify(bs => bs.Execute(), Times.AtLeastOnce());
        }

        [Fact]
        public void Application_start_should_call_on_start()
        {
            httpApplication.Application_Start();

            Assert.True(httpApplication.OnStartCalled);
        }

        [Fact]
        public void OnBeginRequest_should_call_on_per_request_tasks_executing()
        {
            httpApplication.StartBeginRequest(new Mock<HttpContextBase>().Object);

            Assert.True(httpApplication.OnPerRequestTasksExecutingCalled);
        }

        [Fact]
        public void OnBeginRequest_should_call_on_per_request_tasks_executed()
        {
            httpApplication.StartBeginRequest(new Mock<HttpContextBase>().Object);

            Assert.True(httpApplication.OnPerRequestTasksExecutedCalled);
        }

        [Fact]
        public void OnBeginRequest_should_call_execute_of_per_request_task_in_ascending_order()
        {
            var executionList = new Queue<PerRequestTask>();

            var task1 = new DummyTask(99, executionList);
            var task2 = new DummyTask(100, executionList);

            container.Setup(a => a.GetServices<PerRequestTask>()).Returns(new[] { task1, task2 });

            httpApplication.StartBeginRequest(new Mock<HttpContextBase>().Object);

            Assert.Same(task1, executionList.Dequeue());
            Assert.Same(task2, executionList.Dequeue());
        }

        [Fact]
        public void OnBeginRequest_should_skip_next_task_when_previous_task_returns_continuation_as_skip()
        {
            var task1 = new Mock<PerRequestTask>();
            task1.Setup(t => t.Execute()).Returns(TaskContinuation.Skip);

            var task2 = new Mock<PerRequestTask>();
            var task3 = new Mock<PerRequestTask>();

            container.Setup(a => a.GetServices<PerRequestTask>()).Returns(new[] { task1.Object, task2.Object, task3.Object });

            httpApplication.StartBeginRequest(new Mock<HttpContextBase>().Object);

            task2.Verify(t => t.Execute(), Times.Never());
            task3.Verify(t => t.Execute(), Times.AtLeastOnce());
        }

        [Fact]
        public void OnBeginRequest_should_break_the_consequent_task_execution_when_previous_task_returns_continuation_as_break()
        {
            var task1 = new Mock<PerRequestTask>();
            task1.Setup(t => t.Execute()).Returns(TaskContinuation.Break);

            var task2 = new Mock<PerRequestTask>();
            var task3 = new Mock<PerRequestTask>();

            container.Setup(a => a.GetServices<PerRequestTask>()).Returns(new[] { task1.Object, task2.Object, task3.Object });

            httpApplication.StartBeginRequest(new Mock<HttpContextBase>().Object);

            task2.Verify(t => t.Execute(), Times.Never());
            task3.Verify(t => t.Execute(), Times.Never());
        }

        [Fact]
        public void OnEndRequest_should_call_on_per_request_tasks_disposing()
        {
            httpApplication.StartEndRequest(new Mock<HttpContextBase>().Object);

            Assert.True(httpApplication.OnPerRequestTasksDisposingCalled);
        }

        [Fact]
        public void OnEndRequest_should_call_on_per_request_tasks_disposed()
        {
            httpApplication.StartEndRequest(new Mock<HttpContextBase>().Object);

            Assert.True(httpApplication.OnPerRequestTasksDisposedCalled);
        }

        [Fact]
        public void OnEndRequest_should_call_dispose_of_per_request_task_in_descending_order()
        {
            var executionList = new Queue<PerRequestTask>();

            var task1 = new DummyTask(99, executionList);
            var task2 = new DummyTask(100, executionList);

            container.Setup(a => a.GetServices<PerRequestTask>()).Returns(new[] { task1, task2 });

            httpApplication.StartEndRequest(new Mock<HttpContextBase>().Object);

            Assert.Equal(task2, executionList.Dequeue());
            Assert.Equal(task1, executionList.Dequeue());
        }

        [Fact]
        public void Application_end_should_dispose_bootstrapper()
        {
            httpApplication.Application_End();

            bootstrapper.Verify(bs => bs.Dispose(), Times.AtLeastOnce());
        }

        [Fact]
        public void Application_end_should_call_on_start()
        {
            httpApplication.Application_End();

            Assert.True(httpApplication.OnEndCalled);
        }

        public class DummyTask : PerRequestTask
        {
            private readonly Queue<PerRequestTask> executionList;

            public DummyTask(int order, Queue<PerRequestTask> executionList)
            {
                Order = order;
                this.executionList = executionList;
            }

            public override TaskContinuation Execute()
            {
                executionList.Enqueue(this);
                return TaskContinuation.Continue;
            }

            protected override void DisposeCore()
            {
                executionList.Enqueue(this);
            }
        }

        private sealed class ExtendedMvcApplicationTestDouble : ExtendedMvcApplication
        {
            private readonly IBootstrapper mockedBootstrapper;

            public ExtendedMvcApplicationTestDouble(IBootstrapper bootstrapper)
            {
                mockedBootstrapper = bootstrapper;
            }

            public bool OnStartCalled { get; private set; }

            public bool OnEndCalled { get; private set; }

            public bool OnPerRequestTasksExecutingCalled { get; private set; }

            public bool OnPerRequestTasksExecutedCalled { get; private set; }

            public bool OnPerRequestTasksDisposingCalled { get; private set; }

            public bool OnPerRequestTasksDisposedCalled { get; private set; }

            public void StartBeginRequest(HttpContextBase context)
            {
                OnBeginRequest(context);
            }

            public void StartEndRequest(HttpContextBase context)
            {
                OnEndRequest(context);
            }

            protected override IBootstrapper CreateBootstrapper()
            {
                return mockedBootstrapper;
            }

            protected override void OnStart()
            {
                base.OnStart();
                OnStartCalled = true;
            }

            protected override void OnPerRequestTasksExecuting()
            {
                base.OnPerRequestTasksExecuting();
                OnPerRequestTasksExecutingCalled = true;
            }

            protected override void OnPerRequestTasksExecuted()
            {
                base.OnPerRequestTasksExecuted();
                OnPerRequestTasksExecutedCalled = true;
            }

            protected override void OnPerRequestTasksDisposing()
            {
                base.OnPerRequestTasksDisposing();
                OnPerRequestTasksDisposingCalled = true;
            }

            protected override void OnPerRequestTasksDisposed()
            {
                base.OnPerRequestTasksDisposed();
                OnPerRequestTasksDisposedCalled = true;
            }

            protected override void OnEnd()
            {
                base.OnEnd();
                OnEndCalled = true;
            }
        }
    }
}