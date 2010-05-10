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

    public class PositiveIntegerConstraintTests
    {
        [Fact]
        public void Should_match_when_parameter_is_optional()
        {
            var constraint = new PositiveIntegerConstraint(true);

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "id", new RouteValueDictionary(), RouteDirection.IncomingRequest);

            Assert.True(matched);
        }

        [Fact]
        public void Should_match_when_parameter_value_is_positive()
        {
            var constraint = new PositiveIntegerConstraint();

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "id", new RouteValueDictionary { { "id", 7 } }, RouteDirection.IncomingRequest);

            Assert.True(matched);
        }

        [Fact]
        public void Should_not_match_when_parameter_value_is_zero()
        {
            var constraint = new PositiveIntegerConstraint();

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "id", new RouteValueDictionary { { "id", 0 } }, RouteDirection.IncomingRequest);

            Assert.False(matched);
        }

        [Fact]
        public void Should_not_match_when_parameter_value_is_negative()
        {
            var constraint = new PositiveIntegerConstraint();

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "id", new RouteValueDictionary { { "id", -7 } }, RouteDirection.IncomingRequest);

            Assert.False(matched);
        }
    }
}