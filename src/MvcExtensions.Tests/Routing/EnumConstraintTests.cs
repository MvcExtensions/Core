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

    public class EnumConstraintTests
    {
        private enum ResponseFormat
        {
            None = 0,
            Html = 1,
            Text = 2,
            Xml = 3,
            Json = 4
        }

        [Fact]
        public void Should_match_when_parameter_is_optional()
        {
            var constraint = new EnumConstraint<ResponseFormat>(true);

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "format", new RouteValueDictionary(), RouteDirection.IncomingRequest);

            Assert.True(matched);
        }

        [Fact]
        public void Should_match_when_value_matches_with_enum_name()
        {
            var constraint = new EnumConstraint<ResponseFormat>();

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "format", new RouteValueDictionary { { "format", "Xml" } }, RouteDirection.IncomingRequest);

            Assert.True(matched);
        }

        [Fact]
        public void Should_not_match_when_value_does_not_match_with_enum_name()
        {
            var constraint = new EnumConstraint<ResponseFormat>();

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "format", new RouteValueDictionary { { "format", "bson" } }, RouteDirection.IncomingRequest);

            Assert.False(matched);
        }

        [Fact]
        public void Should_not_match_for_enum_value()
        {
            var constraint = new EnumConstraint<ResponseFormat>();

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "format", new RouteValueDictionary { { "format", 4 } }, RouteDirection.IncomingRequest);

            Assert.False(matched);
        }
    }
}