#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;
    using System.Web;
    using System.Web.Routing;

    using Moq;
    using Xunit;

    public class GuidConstraintTests
    {
        [Fact]
        public void Should_match_when_parameter_is_optional()
        {
            var constraint = new GuidConstraint(true);

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "id", new RouteValueDictionary(), RouteDirection.IncomingRequest);

            Assert.True(matched);
        }

        [Fact]
        public void Should_match_when_value_is_in_correct_format()
        {
            var constraint = new GuidConstraint();

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "id", new RouteValueDictionary { { "id", Guid.NewGuid() } }, RouteDirection.IncomingRequest);

            Assert.True(matched);
        }

        [Fact]
        public void Should_not_match_when_value_is_not_in_correct_format()
        {
            var constraint = new GuidConstraint();

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "id", new RouteValueDictionary { { "id", "foobar" } }, RouteDirection.IncomingRequest);

            Assert.False(matched);
        }

        [Fact]
        public void Should_not_match_when_value_is_empty()
        {
            var constraint = new GuidConstraint();

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "id", new RouteValueDictionary { { "id", Guid.Empty } }, RouteDirection.IncomingRequest);

            Assert.False(matched);
        }
    }
}