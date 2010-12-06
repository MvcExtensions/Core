#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Web.Mvc;

    using Moq;
    using Xunit;

    public class RegisterControllersTests
    {
        private readonly Mock<ContainerAdapter> adapter;

        public RegisterControllersTests()
        {
            var buildManager = new Mock<IBuildManager>();
            buildManager.Setup(bm => bm.ConcreteTypes).Returns(new[] { typeof(DummyController) });

            adapter = new Mock<ContainerAdapter>();
            adapter.Setup(a => a.GetService(typeof(IBuildManager))).Returns(buildManager.Object);
        }

        [Fact]
        public void Should_register_available_controllers()
        {
            adapter.Setup(a => a.RegisterType(null, typeof(DummyController), typeof(DummyController), LifetimeType.Transient)).Verifiable();

            new RegisterControllers(adapter.Object).Execute();

            adapter.Verify();
        }

        [Fact]
        public void Should_not_register_controllers_when_controller_exists_in_ignored_list()
        {
            var registration = new RegisterControllers(adapter.Object);

            registration.Ignore<DummyController>();

            registration.Execute();

            adapter.Verify(a => a.RegisterType(null, typeof(DummyController), typeof(DummyController), LifetimeType.Transient), Times.Never());
        }

        private sealed class DummyController : Controller
        {
        }
    }
}