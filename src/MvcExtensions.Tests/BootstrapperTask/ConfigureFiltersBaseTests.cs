#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Moq;
    using Xunit;

    public class ConfigureFiltersBaseTests
    {
        [Fact]
        public void Should_be_able_to_configure()
        {
            var registry = new Mock<IFilterRegistry>();

            registry.Setup(r => r.Register<DummyController, FilterAttribute>(It.IsAny<IList<Func<FilterAttribute>>>())).Verifiable();

            new ConfigureFiltersBaseTestDouble(registry.Object).Execute();

            registry.Verify();
        }

        private class ConfigureFiltersBaseTestDouble : ConfigureFiltersBase
        {
            public ConfigureFiltersBaseTestDouble(IFilterRegistry registry) : base(registry)
            {
            }

            protected override void Configure()
            {
                Registry.Register<DummyController, DummyFilter>();
            }
        }

        private sealed class DummyController : Controller
        {
        }

        private sealed class DummyFilter : FilterAttribute
        {
        }
    }
}