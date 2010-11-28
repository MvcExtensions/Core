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

    public class RegisterActionInvokersTests : IDisposable
    {
        private readonly Mock<ContainerAdapter> adapter;

        public RegisterActionInvokersTests()
        {
            RegisterActionInvokers.Excluded = false;
            RegisterActionInvokers.IgnoredTypes.Clear();

            var buildManager = new Mock<IBuildManager>();
            buildManager.Setup(bm => bm.ConcreteTypes).Returns(new[] { typeof(DummyActionInvoker) });

            adapter = new Mock<ContainerAdapter>();
            adapter.Setup(a => a.GetService(typeof(IBuildManager))).Returns(buildManager.Object);
        }

        public void Dispose()
        {
            RegisterActionInvokers.Excluded = false;
            RegisterActionInvokers.IgnoredTypes.Clear();
        }

        [Fact]
        public void Should_register_available_controllers()
        {
            adapter.Setup(a => a.RegisterType(null, typeof(DummyActionInvoker), typeof(DummyActionInvoker), LifetimeType.Transient)).Verifiable();

            new RegisterActionInvokers(adapter.Object).Execute();

            adapter.Verify();
        }

        [Fact]
        public void Should_not_register_action_invoker_when_excluded()
        {
            RegisterActionInvokers.Excluded = true;

            new RegisterActionInvokers(adapter.Object).Execute();

            adapter.Verify(a => a.RegisterType(null, It.IsAny<Type>(), It.IsAny<Type>(), LifetimeType.Transient), Times.Never());
        }

        [Fact]
        public void Should_not_register_controllers_when_controller_exists_in_ignored_list()
        {
            RegisterActionInvokers.IgnoredTypes.Add(typeof(DummyActionInvoker));

            new RegisterActionInvokers(adapter.Object).Execute();

            adapter.Verify(a => a.RegisterType(null, It.IsAny<Type>(), It.IsAny<Type>(), LifetimeType.Transient), Times.Never());
        }

        private sealed class DummyActionInvoker : IActionInvoker
        {
            public bool InvokeAction(ControllerContext controllerContext, string actionName)
            {
                return false;
            }
        }
    }
}