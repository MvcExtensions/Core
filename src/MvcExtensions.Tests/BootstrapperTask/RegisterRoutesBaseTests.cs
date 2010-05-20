#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Web.Routing;
    using Xunit;

    public class RegisterRoutesBaseTests
    {
        [Fact]
        public void Should_be_able_to_register_routes()
        {
            var registration = new RegisterRoutesBaseTestDouble(new RouteCollection());

            registration.Execute();

            Assert.True(registration.Registered);
        }

        private sealed class RegisterRoutesBaseTestDouble : RegisterRoutesBase
        {
            public RegisterRoutesBaseTestDouble(RouteCollection routes) : base(routes)
            {
            }

            public bool Registered
            {
                get;
                private set;
            }

            protected override void Register()
            {
                Registered = true;
            }
        }
    }
}