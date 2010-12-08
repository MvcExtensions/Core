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

    using Xunit;

    public class ActionInvokerRegistryTest
    {
        private readonly TypeMappingRegistry<Controller, IActionInvoker> registry;

        public ActionInvokerRegistryTest()
        {
            registry = new TypeMappingRegistry<Controller, IActionInvoker>();
        }

        [Fact]
        public void Should_be_able_to_register()
        {
            registry.Register(typeof(Dummy1Controller), typeof(DummyActionInvoker));

            Assert.True(registry.IsRegistered(typeof(Dummy1Controller)));
        }

        [Fact]
        public void Register_should_throw_exception_for_non_controller_type()
        {
            Assert.Throws<ArgumentException>(() => registry.Register(typeof(object), typeof(DummyActionInvoker)));
        }

        [Fact]
        public void Register_should_throw_exception_for_non_action_invoker_type()
        {
            Assert.Throws<ArgumentException>(() => registry.Register(typeof(Dummy1Controller), typeof(object)));
        }

        [Fact]
        public void IsRegistered_should_return_false_for_non_registered_controller_types()
        {
            Assert.False(registry.IsRegistered(typeof(Dummy2Controller)));
        }

        [Fact]
        public void Should_be_able_to_get_matching_action_invoker_type()
        {
            registry.Register(typeof(Dummy1Controller), typeof(DummyActionInvoker));

            Assert.Same(typeof(DummyActionInvoker), registry.Matching(typeof(Dummy1Controller)));
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