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

    public class RegisterActionInvokersTests
    {
        private readonly Mock<ContainerAdapter> adapter;

        public RegisterActionInvokersTests()
        {
            var buildManager = new Mock<IBuildManager>();
            buildManager.Setup(bm => bm.ConcreteTypes).Returns(new[] { typeof(DummyActionInvoker) });

            adapter = new Mock<ContainerAdapter>();
            adapter.Setup(a => a.GetService(typeof(IBuildManager))).Returns(buildManager.Object);
        }

        [Fact]
        public void Should_register_available_action_invokers()
        {
            adapter.Setup(a => a.RegisterType(null, typeof(DummyActionInvoker), typeof(DummyActionInvoker), LifetimeType.Transient)).Verifiable();

            new RegisterActionInvokers(adapter.Object).Execute();

            adapter.Verify();
        }

        [Fact]
        public void Should_not_register_action_invokers_when_action_invoker_exists_in_ignored_list()
        {
            var registration = new RegisterActionInvokers(adapter.Object);

            registration.Ignore<DummyActionInvoker>();

            registration.Execute();

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