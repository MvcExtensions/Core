#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Web;
    using System.Web.Routing;

    using Moq;
    using Xunit;

    public class RangeConstraintTests
    {
        [Fact]
        public void Should_match_when_parameter_is_optional()
        {
            var constraint = new RangeConstraint<int>(1, 5, true);

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "alias", new RouteValueDictionary { { "alias", null } }, RouteDirection.IncomingRequest);

            Assert.True(matched);
        }

        [Fact]
        public void Should_match_when_value_is_in_range()
        {
            var constraint = new RangeConstraint<int>(1, 5, true);

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "page", new RouteValueDictionary { { "page", 3 } }, RouteDirection.IncomingRequest);

            Assert.True(matched);
        }

        [Fact]
        public void Should_not_match_when_value_is_not_in_range()
        {
            var constraint = new RangeConstraint<int>(1, 5);

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "page", new RouteValueDictionary { { "page", 6 } }, RouteDirection.IncomingRequest);

            Assert.False(matched);
        }

        [Fact]
        public void Should_not_match_when_unable_to_convert()
        {
            var constraint = new RangeConstraint<int>(1, 5);

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "page", new RouteValueDictionary { { "page", "xyz" } }, RouteDirection.IncomingRequest);

            Assert.False(matched);
        }
    }
}