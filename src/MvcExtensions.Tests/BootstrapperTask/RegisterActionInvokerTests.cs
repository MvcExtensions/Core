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

    public class RegisterActionInvokerTests : IDisposable
    {
        private readonly Mock<ContainerAdapter> adapter;

        public RegisterActionInvokerTests()
        {
            RegisterActionInvoker.Excluded = false;

            adapter = new Mock<ContainerAdapter>();
        }

        public void Dispose()
        {
            RegisterActionInvoker.Excluded = false;
        }

        [Fact]
        public void Should_be_able_to_register_action_invoker()
        {
            adapter.Setup(a => a.RegisterType(null, typeof(IActionInvoker), typeof(ExtendedControllerActionInvoker), LifetimeType.Transient)).Verifiable();

            new RegisterActionInvoker(adapter.Object).Execute();

            adapter.Verify();
        }

        [Fact]
        public void Should_not_register_action_invoker_when_excluded()
        {
            RegisterActionInvoker.Excluded = true;

            new RegisterActionInvoker(adapter.Object).Execute();

            adapter.Verify(a => a.RegisterType(null, typeof(IActionInvoker), typeof(ExtendedControllerActionInvoker), LifetimeType.Transient), Times.Never());
        }
    }
}