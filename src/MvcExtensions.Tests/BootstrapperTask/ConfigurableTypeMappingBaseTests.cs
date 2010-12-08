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

    public class ConfigurableTypeMappingBaseTests
    {
        [Fact]
        public void Should_be_able_to_configure()
        {
            var registry = new Mock<TypeMappingRegistry<Controller, IActionInvoker>>();

            registry.Setup(r => r.Register(typeof(Dummy1Controller), typeof(DummyActionInvoker))).Verifiable();

            new ConfigureActionInvokersBaseTestDouble(registry.Object).Execute();

            registry.Verify();
        }

        private class ConfigureActionInvokersBaseTestDouble : ConfigurableTypeMappingBase<Controller, IActionInvoker>
        {
            public ConfigureActionInvokersBaseTestDouble(TypeMappingRegistry<Controller, IActionInvoker> registry) : base(registry)
            {
            }

            protected override void Configure()
            {
                Registry.Register<Dummy1Controller, DummyActionInvoker>();
            }
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