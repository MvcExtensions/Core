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

    public class RegisterPerRequestTasksTests : IDisposable
    {
        public RegisterPerRequestTasksTests()
        {
            RegisterPerRequestTasks.Excluded = false;
            RegisterPerRequestTasks.IgnoredTypes.Clear();
        }

        public void Dispose()
        {
            RegisterPerRequestTasks.Excluded = false;
            RegisterPerRequestTasks.IgnoredTypes.Clear();
        }

        [Fact]
        public void Should_register_available_tasks()
        {
            var adapter = SetupAdapter();

            adapter.Setup(sr => sr.RegisterType(null, typeof(IPerRequestTask), typeof(DummyPerRequestTask), LifetimeType.PerRequest)).Verifiable();

            new RegisterPerRequestTasks().Execute(adapter.Object);

            adapter.Verify();
        }

        [Fact]
        public void Should_not_register_tasks_when_excluded()
        {
            var adapter = SetupAdapter();

            RegisterPerRequestTasks.Excluded = true;

            new RegisterPerRequestTasks().Execute(adapter.Object);

            adapter.Verify(sr => sr.RegisterType(null, typeof(IPerRequestTask), typeof(DummyPerRequestTask), LifetimeType.PerRequest), Times.Never());
        }

        [Fact]
        public void Should_not_register_tasks_when_task_exists_in_ignored_list()
        {
            var adapter = SetupAdapter();

            RegisterPerRequestTasks.IgnoredTypes.Add(typeof(DummyPerRequestTask));

            new RegisterPerRequestTasks().Execute(adapter.Object);

            adapter.Verify(sr => sr.RegisterType(null, typeof(IPerRequestTask), typeof(DummyPerRequestTask), LifetimeType.PerRequest), Times.Never());
        }

        private static Mock<FakeAdapter> SetupAdapter()
        {
            var buildManager = new Mock<IBuildManager>();
            buildManager.Setup(bm => bm.ConcreteTypes).Returns(new[] { typeof(DummyPerRequestTask) });

            var adapter = new Mock<FakeAdapter>();

            adapter.Setup(a => a.GetInstance<IBuildManager>()).Returns(buildManager.Object);

            return adapter;
        }

        private sealed class DummyPerRequestTask : PerRequestTask
        {
            protected override TaskContinuation ExecuteCore(PerRequestExecutionContext executionContext)
            {
                return TaskContinuation.Break;
            }
        }
    }
}