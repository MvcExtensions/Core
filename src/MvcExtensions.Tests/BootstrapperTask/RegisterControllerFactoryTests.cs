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

    public class RegisterControllerFactoryTests : IDisposable
    {
        private readonly Mock<ContainerAdapter> adapter;
        private readonly Mock<IControllerFactory> controllerFactory;

        public RegisterControllerFactoryTests()
        {
            RegisterControllerFactory.Excluded = false;

            adapter = new Mock<ContainerAdapter>();
            controllerFactory = new Mock<IControllerFactory>();

            adapter.Setup(a => a.GetInstance<IControllerFactory>()).Returns(controllerFactory.Object);

            MvcServiceLocator.SetCurrent(adapter.Object);
        }

        public void Dispose()
        {
            RegisterControllerFactory.Excluded = false;
        }

        [Fact]
        public void Should_be_able_to_set_controller_factory()
        {
            RegisterControllerFactory.ControllerFactoryType = typeof(object);

            Assert.Equal(typeof(object), RegisterControllerFactory.ControllerFactoryType);

            RegisterControllerFactory.ControllerFactoryType = typeof(ExtendedControllerFactory);
        }

        [Fact]
        public void Should_be_able_to_register_controller_factory()
        {
            var builder = new ControllerBuilder();

            new RegisterControllerFactory(adapter.Object, builder).Execute();

            Assert.Same(controllerFactory.Object, builder.GetControllerFactory());
        }

        [Fact]
        public void Should_not_register_controller_factory_when_excluded()
        {
            RegisterControllerFactory.Excluded = true;
            MvcServiceLocator.SetCurrent(null);

            var builder = new ControllerBuilder();

            new RegisterControllerFactory(adapter.Object, builder).Execute();

            Assert.NotSame(controllerFactory.Object, builder.GetControllerFactory());
        }
    }
}