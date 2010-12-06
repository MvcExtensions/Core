#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;
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
        public void Application_start_should_execute_bootstrapper_tasks()
        {
            bootstrapper.Setup(bs => bs.ExecuteBootstrapperTasks()).Verifiable();

            httpApplication.Application_Start();

            bootstrapper.Verify(bs => bs.ExecuteBootstrapperTasks(), Times.AtLeastOnce());
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
        public void OnBeginRequest_should_call_bootstrapper_execute_per_request_tasks()
        {
            bootstrapper.Setup(bs => bs.ExecutePerRequestTasks()).Verifiable();

            httpApplication.StartBeginRequest(new Mock<HttpContextBase>().Object);

            bootstrapper.Verify();
        }

        [Fact]
        public void OnBeginRequest_should_call_on_per_request_tasks_executed()
        {
            httpApplication.StartBeginRequest(new Mock<HttpContextBase>().Object);

            Assert.True(httpApplication.OnPerRequestTasksExecutedCalled);
        }

        [Fact]
        public void OnEndRequest_should_call_on_per_request_tasks_disposing()
        {
            httpApplication.StartEndRequest(new Mock<HttpContextBase>().Object);

            Assert.True(httpApplication.OnPerRequestTasksDisposingCalled);
        }

        [Fact]
        public void OnEndRequest_should_call_bootstrapper_dispose_per_request_tasks()
        {
            bootstrapper.Setup(bs => bs.DisposePerRequestTasks()).Verifiable();

            httpApplication.StartEndRequest(new Mock<HttpContextBase>().Object);

            bootstrapper.Verify();
        }

        [Fact]
        public void OnEndRequest_should_call_on_per_request_tasks_disposed()
        {
            httpApplication.StartEndRequest(new Mock<HttpContextBase>().Object);

            Assert.True(httpApplication.OnPerRequestTasksDisposedCalled);
        }

        [Fact]
        public void Application_end_should_dispose_bootstrapper_tasks()
        {
            bootstrapper.Setup(bs => bs.DisposeBootstrapperTasks()).Verifiable();

            httpApplication.Application_End();

            bootstrapper.Verify(bs => bs.DisposeBootstrapperTasks(), Times.AtLeastOnce());
        }

        [Fact]
        public void Application_end_should_call_on_end()
        {
            httpApplication.Application_End();

            Assert.True(httpApplication.OnEndCalled);
        }

        public class ExtendedMvcApplicationTestDouble : ExtendedMvcApplication
        {
            private IBootstrapper bootstrapper;

            public ExtendedMvcApplicationTestDouble(IBootstrapper bootstrapper)
            {
                this.bootstrapper = bootstrapper;
            }

            public bool OnStartCalled { get; private set; }

            public bool OnEndCalled { get; private set; }

            public bool OnPerRequestTasksExecutingCalled { get; private set; }

            public bool OnPerRequestTasksExecutedCalled { get; private set; }

            public bool OnPerRequestTasksDisposingCalled { get; private set; }

            public bool OnPerRequestTasksDisposedCalled { get; private set; }

            public void StartBeginRequest(HttpContextBase httpContext)
            {
                base.OnBeginRequest(httpContext);
            }

            public void StartEndRequest(HttpContextBase httpContext)
            {
                base.OnEndRequest(httpContext);
            }

            protected override IBootstrapper CreateBootstrapper()
            {
                return bootstrapper;
            }

            protected override void OnStart()
            {
                base.OnStart();
                OnStartCalled = true;
            }

            protected override void OnEnd()
            {
                base.OnEnd();
                OnEndCalled = true;
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
        }
    }
}