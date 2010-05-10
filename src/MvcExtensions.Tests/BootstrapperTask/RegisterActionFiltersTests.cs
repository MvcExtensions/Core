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

    public class RegisterActionFiltersTests : IDisposable
    {
        public RegisterActionFiltersTests()
        {
            RegisterActionFilters.Excluded = false;
            RegisterActionFilters.IgnoredTypes.Clear();
        }

        public void Dispose()
        {
            RegisterActionFilters.Excluded = false;
            RegisterActionFilters.IgnoredTypes.Clear();
        }

        [Fact]
        public void Should_register_available_action_filters()
        {
            var adapter = SetupAdapter();

            adapter.Setup(sr => sr.RegisterType(null, typeof(DummyFilter), typeof(DummyFilter), LifetimeType.Transient)).Verifiable();

            new RegisterActionFilters().Execute(adapter.Object);

            adapter.Verify();
        }

        [Fact]
        public void Should_not_register_filters_when_excluded()
        {
            var adapter = SetupAdapter();

            RegisterActionFilters.Excluded = true;

            new RegisterActionFilters().Execute(adapter.Object);

            adapter.Verify(sr => sr.RegisterType(null, typeof(DummyFilter), typeof(DummyFilter), LifetimeType.Transient), Times.Never());
        }

        [Fact]
        public void Should_not_register_filters_when_filter_exists_in_ignored_list()
        {
            var adapter = SetupAdapter();

            RegisterActionFilters.IgnoredTypes.Add(typeof(DummyFilter));

            new RegisterActionFilters().Execute(adapter.Object);

            adapter.Verify(sr => sr.RegisterType(null, typeof(DummyFilter), typeof(DummyFilter), LifetimeType.Transient), Times.Never());
        }

        private static Mock<FakeAdapter> SetupAdapter()
        {
            var buildManager = new Mock<IBuildManager>();
            buildManager.Setup(bm => bm.ConcreteTypes).Returns(new[] { typeof(DummyFilter) });

            var adapter = new Mock<FakeAdapter>();

            adapter.Setup(sl => sl.GetInstance<IBuildManager>()).Returns(buildManager.Object);

            return adapter;
        }

        private sealed class DummyFilter : FilterAttribute
        {
        }
    }
}