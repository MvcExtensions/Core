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

    public class RegexConstraintTests
    {
        [Fact]
        public void Should_match_when_parameter_is_optional()
        {
            var constraint = new RegexConstraint(@"^[a-zA-Z0-9]+$", true);

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "alias", new RouteValueDictionary(), RouteDirection.IncomingRequest);

            Assert.True(matched);
        }

        [Fact]
        public void Should_match_when_pattern_matches()
        {
            var constraint = new RegexConstraint(@"^[a-zA-Z0-9]+$");

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "alias", new RouteValueDictionary { { "alias", "abc" } }, RouteDirection.IncomingRequest);

            Assert.True(matched);
        }

        [Fact]
        public void Should_not_match_when_pattern_does_not_match()
        {
            var constraint = new RegexConstraint(@"^[a-zA-Z0-9]+$");

            var matched = constraint.Match(new Mock<HttpContextBase>().Object, new Route("{controller}/{action}", new Mock<IRouteHandler>().Object), "alias", new RouteValueDictionary { { "alias", "@#%" } }, RouteDirection.IncomingRequest);

            Assert.False(matched);
        }
    }
}