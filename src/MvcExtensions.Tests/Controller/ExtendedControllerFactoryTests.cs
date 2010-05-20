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
    using System.Web.Routing;

    using Microsoft.Practices.ServiceLocation;

    using Moq;
    using Xunit;

    public class ExtendedControllerFactoryTests
    {
        [Fact]
        public void Should_be_able_to_create_controller()
        {
            var adapter = new Mock<ContainerAdapter>();
            var controllerFactory = new ExtendedControllerFactoryTestDouble(adapter.Object);

            var actionInvoker = new Mock<IActionInvoker>();
            var controller = new Mock<Controller>();

            adapter.Setup(a => a.GetInstance(It.Is<Type>(type => typeof(Controller).IsAssignableFrom(type)))).Returns(controller.Object);
            adapter.Setup(a => a.GetInstance<IActionInvoker>()).Returns(actionInvoker.Object);

            controllerFactory.PublicGetControllerInstance(null, controller.Object.GetType());

            Assert.Same(actionInvoker.Object, controller.Object.ActionInvoker);
        }

        private sealed class ExtendedControllerFactoryTestDouble : ExtendedControllerFactory
        {
            public ExtendedControllerFactoryTestDouble(IServiceLocator locator) : base(locator)
            {
            }

            public void PublicGetControllerInstance(RequestContext requestContext, Type controllerType)
            {
                GetControllerInstance(requestContext, controllerType);
            }
        }
    }
}