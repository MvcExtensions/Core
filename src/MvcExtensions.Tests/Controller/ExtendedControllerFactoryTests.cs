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

    using Moq;
    using Xunit;

    public class ExtendedControllerFactoryTests
    {
        private readonly Mock<Controller> controller;
        private readonly Mock<ContainerAdapter> container;
        private readonly Mock<IActionInvokerRegistry> registry;
        private readonly ExtendedControllerFactoryTestDouble factory;

        public ExtendedControllerFactoryTests()
        {
            controller = new Mock<Controller>();

            container = new Mock<ContainerAdapter>();
            container.Setup(c => c.GetInstance(It.Is<Type>(type => typeof(Controller).IsAssignableFrom(type)))).Returns(controller.Object);

            registry = new Mock<IActionInvokerRegistry>();

            factory = new ExtendedControllerFactoryTestDouble(container.Object, registry.Object);
        }

        [Fact]
        public void ActionInvoker_should_be_extended_action_invoker_when_controller_is_not_register_in_action_invoker_registry()
        {
            container.Setup(a => a.GetInstance(typeof(ExtendedControllerActionInvoker))).Verifiable();

            factory.PublicGetControllerInstance(null, controller.Object.GetType());

            container.Verify();
        }

        [Fact]
        public void ActionInvoker_should_be_mapped_action_invoker_when_controller_is_registered()
        {
            var actionInvoker = new Mock<IActionInvoker>();

            registry.Setup(r => r.IsRegistered(controller.Object.GetType())).Returns(true);
            registry.Setup(r => r.Matching(controller.Object.GetType())).Returns(actionInvoker.Object.GetType());

            container.Setup(a => a.GetInstance(actionInvoker.Object.GetType())).Verifiable();

            factory.PublicGetControllerInstance(null, controller.Object.GetType());

            container.Verify();
        }

        private sealed class ExtendedControllerFactoryTestDouble : ExtendedControllerFactory
        {
            public ExtendedControllerFactoryTestDouble(ContainerAdapter container, IActionInvokerRegistry actionInvokerRegistry) : base(container, actionInvokerRegistry)
            {
            }

            public void PublicGetControllerInstance(RequestContext requestContext, Type controllerType)
            {
                GetControllerInstance(requestContext, controllerType);
            }
        }
    }
}