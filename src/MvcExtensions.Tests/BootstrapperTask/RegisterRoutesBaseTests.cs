#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Web.Routing;

    using Moq;
    using Xunit;

    public class RegisterRoutesBaseTests
    {
        [Fact]
        public void Should_be_able_to_register_routes()
        {
            var adapter = new Mock<FakeAdapter>();

            adapter.Setup(a => a.GetInstance<RouteCollection>()).Returns(new RouteCollection());

            var registration = new RegisterRoutesBaseTestDouble();

            registration.Execute(adapter.Object);

            Assert.True(registration.IsRegistered);
        }

        private sealed class RegisterRoutesBaseTestDouble : RegisterRoutesBase
        {
            public bool IsRegistered
            {
                get;
                private set;
            }

            protected override void Register(RouteCollection routes)
            {
                IsRegistered = true;
            }
        }
    }
}