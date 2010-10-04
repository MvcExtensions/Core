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

    using Moq;
    using Xunit;

    public class RegisterPerRequestTasksTests : IDisposable
    {
        private readonly Mock<ContainerAdapter> adapter;

        public RegisterPerRequestTasksTests()
        {
            RegisterPerRequestTasks.Excluded = false;
            RegisterPerRequestTasks.IgnoredTypes.Clear();

            var buildManager = new Mock<IBuildManager>();
            buildManager.Setup(bm => bm.ConcreteTypes).Returns(new[] { typeof(DummyPerRequestTask) });

            adapter = new Mock<ContainerAdapter>();
            adapter.Setup(a => a.GetService(typeof(IBuildManager))).Returns(buildManager.Object);
        }

        public void Dispose()
        {
            RegisterPerRequestTasks.Excluded = false;
            RegisterPerRequestTasks.IgnoredTypes.Clear();
        }

        [Fact]
        public void Should_register_available_tasks()
        {
            adapter.Setup(a => a.RegisterType(null, typeof(PerRequestTask), typeof(DummyPerRequestTask), LifetimeType.PerRequest)).Verifiable();

            new RegisterPerRequestTasks(adapter.Object).Execute();

            adapter.Verify();
        }

        [Fact]
        public void Should_not_register_tasks_when_excluded()
        {
            RegisterPerRequestTasks.Excluded = true;

            new RegisterPerRequestTasks(adapter.Object).Execute();

            adapter.Verify(a => a.RegisterType(null, typeof(PerRequestTask), typeof(DummyPerRequestTask), LifetimeType.PerRequest), Times.Never());
        }

        [Fact]
        public void Should_not_register_tasks_when_task_exists_in_ignored_list()
        {
            RegisterPerRequestTasks.IgnoredTypes.Add(typeof(DummyPerRequestTask));

            new RegisterPerRequestTasks(adapter.Object).Execute();

            adapter.Verify(a => a.RegisterType(null, typeof(PerRequestTask), typeof(DummyPerRequestTask), LifetimeType.PerRequest), Times.Never());
        }

        private sealed class DummyPerRequestTask : PerRequestTask
        {
            public override TaskContinuation Execute()
            {
                return TaskContinuation.Break;
            }
        }
    }
}