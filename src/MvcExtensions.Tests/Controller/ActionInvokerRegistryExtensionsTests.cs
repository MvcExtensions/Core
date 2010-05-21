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

    public class ActionInvokerRegistryExtensionsTests
    {
        private readonly Mock<IActionInvokerRegistry> registry;

        public ActionInvokerRegistryExtensionsTests()
        {
            registry = new Mock<IActionInvokerRegistry>();
        }

        [Fact]
        public void Should_be_able_to_register_one_controller()
        {
            registry.Object.Register<Dummy1Controller, DummyActionInvoker>();

            registry.Verify(r => r.Register(It.Is<Type>(t => typeof(Controller).IsAssignableFrom(t)), typeof(DummyActionInvoker)), Times.Once());
        }

        [Fact]
        public void Should_be_able_to_register_two_controller()
        {
            registry.Object.Register<Dummy1Controller, Dummy2Controller, DummyActionInvoker>();

            registry.Verify(r => r.Register(It.Is<Type>(t => typeof(Controller).IsAssignableFrom(t)), typeof(DummyActionInvoker)), Times.Exactly(2));
        }

        [Fact]
        public void Should_be_able_to_register_three_controller()
        {
            registry.Object.Register<Dummy1Controller, Dummy2Controller, Dummy3Controller, DummyActionInvoker>();

            registry.Verify(r => r.Register(It.Is<Type>(t => typeof(Controller).IsAssignableFrom(t)), typeof(DummyActionInvoker)), Times.Exactly(3));
        }

        [Fact]
        public void Should_be_able_to_register_four_controller()
        {
            registry.Object.Register<Dummy1Controller, Dummy2Controller, Dummy3Controller, Dummy4Controller, DummyActionInvoker>();

            registry.Verify(r => r.Register(It.Is<Type>(t => typeof(Controller).IsAssignableFrom(t)), typeof(DummyActionInvoker)), Times.Exactly(4));
        }

        [Fact]
        public void Should_be_able_to_register_type_catalog()
        {
            var catalog = new TypeCatalogBuilder().Add(GetType().Assembly)
                                                  .Include(t => typeof(Controller).IsAssignableFrom(t));

            registry.Object.Register<DummyActionInvoker>(catalog);

            registry.Verify(r => r.Register(It.Is<Type>(t => typeof(Controller).IsAssignableFrom(t)), typeof(DummyActionInvoker)), Times.Exactly(2));
        }

        [Fact]
        public void Should_throw_exception_when_catalog_contains_non_controller_type()
        {
            Assert.Throws<ArgumentException>(() => registry.Object.Register<DummyActionInvoker>(new TypeCatalogBuilder().Add(GetType().Assembly).Include(t => typeof(object).IsAssignableFrom(t))));
        }

        private sealed class Dummy3Controller : Controller
        {
        }

        private sealed class Dummy4Controller : Controller
        {
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