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

    using Xunit;

    public class ContainerAdapterTests
    {
        private ContainerAdapterTestDouble adapter;

        public ContainerAdapterTests()
        {
            adapter = new ContainerAdapterTestDouble();
        }

        [Fact]
        public void Should_be_able_to_dispose()
        {
            adapter.Dispose();

            Assert.True(adapter.Disposed);
        }

        [Fact]
        public void Should_be_able_to_finalize()
        {
            adapter = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private sealed class ContainerAdapterTestDouble : ContainerAdapter
        {
            public bool Disposed { get; private set; }

            public override IServiceRegistrar RegisterType(string key, Type serviceType, Type implementationType, LifetimeType lifetime)
            {
                return null;
            }

            public override IServiceRegistrar RegisterInstance(string key, Type serviceType, object instance)
            {
                return null;
            }

            public override void Inject(object instance)
            {
            }

            protected override object DoGetService(Type serviceType, string key)
            {
                return null;
            }

            protected override IEnumerable<object> DoGetServices(Type serviceType)
            {
                return null;
            }

            protected override void DisposeCore()
            {
                base.DisposeCore();

                Disposed = true;
            }
        }
    }
}