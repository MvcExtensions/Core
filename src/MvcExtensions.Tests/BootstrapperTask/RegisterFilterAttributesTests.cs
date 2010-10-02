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

    public class RegisterFilterAttributesTests : IDisposable
    {
        private readonly Mock<ContainerAdapter> adapter;

        public RegisterFilterAttributesTests()
        {
            RegisterFilterAttributes.Excluded = false;
            RegisterFilterAttributes.IgnoredTypes.Clear();

            var buildManager = new Mock<IBuildManager>();
            buildManager.Setup(bm => bm.ConcreteTypes).Returns(new[] { typeof(DummyFilter) });

            adapter = new Mock<ContainerAdapter>();

            adapter.Setup(a => a.GetService<IBuildManager>()).Returns(buildManager.Object);
        }

        public void Dispose()
        {
            RegisterFilterAttributes.Excluded = false;
            RegisterFilterAttributes.IgnoredTypes.Clear();
        }

        [Fact]
        public void Should_register_available_action_filters()
        {
            adapter.Setup(a => a.RegisterType(null, typeof(DummyFilter), typeof(DummyFilter), LifetimeType.Transient)).Verifiable();

            new RegisterFilterAttributes(adapter.Object).Execute();

            adapter.Verify();
        }

        [Fact]
        public void Should_not_register_filters_when_excluded()
        {
            RegisterFilterAttributes.Excluded = true;

            new RegisterFilterAttributes(adapter.Object).Execute();

            adapter.Verify(a => a.RegisterType(null, typeof(DummyFilter), typeof(DummyFilter), LifetimeType.Transient), Times.Never());
        }

        [Fact]
        public void Should_not_register_filters_when_filter_exists_in_ignored_list()
        {
            RegisterFilterAttributes.IgnoredTypes.Add(typeof(DummyFilter));

            new RegisterFilterAttributes(adapter.Object).Execute();

            adapter.Verify(a => a.RegisterType(null, typeof(DummyFilter), typeof(DummyFilter), LifetimeType.Transient), Times.Never());
        }

        private sealed class DummyFilter : FilterAttribute
        {
        }
    }
}