#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;
    using Xunit;

    public class CompressAttributeTests
    {
        [Fact]
        public void Order_should_be_set_to_maximum_value()
        {
            Assert.Equal(int.MaxValue, new CompressAttribute().Order);
        }

        [Fact]
        public void OnResultExecuting_should_do_nothing()
        {
            new CompressAttribute().OnResultExecuting(null);
        }

        [Fact]
        public void Should_not_compress_for_child_action()
        {
            var httpContext = new Mock<HttpContextBase>();

            var routeData = new RouteData();
            routeData.DataTokens.Add("ParentActionViewContext", new object());

            var requestContext = new RequestContext(httpContext.Object, routeData);
            var controllerContext = new ControllerContext(requestContext, new Mock<ControllerBase>().Object);
            var resultContext = new ResultExecutedContext(controllerContext, new Mock<ActionResult>().Object, false, null);

            var attribute = new CompressAttribute();

            httpContext.Setup(c => c.Response.AppendHeader(It.IsAny<string>(), It.IsAny<string>()));

            attribute.OnResultExecuted(resultContext);

            httpContext.Verify(c => c.Response.AppendHeader(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void Should_not_compress_when_action_is_canceled()
        {
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            var controllerContext = new ControllerContext(requestContext, new Mock<ControllerBase>().Object);
            var resultContext = new ResultExecutedContext(controllerContext, new Mock<ActionResult>().Object, true, null);

            var attribute = new CompressAttribute();

            httpContext.Setup(c => c.Response.AppendHeader(It.IsAny<string>(), It.IsAny<string>()));

            attribute.OnResultExecuted(resultContext);

            httpContext.Verify(c => c.Response.AppendHeader(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void Should_not_compress_when_exception_occurrs_and_exception_is_not_handled()
        {
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            var controllerContext = new ControllerContext(requestContext, new Mock<ControllerBase>().Object);
            var resultContext = new ResultExecutedContext(controllerContext, new Mock<ActionResult>().Object, false, new InvalidOperationException());

            var attribute = new CompressAttribute();

            httpContext.Setup(c => c.Response.AppendHeader(It.IsAny<string>(), It.IsAny<string>()));

            attribute.OnResultExecuted(resultContext);

            httpContext.Verify(c => c.Response.AppendHeader(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Fact(Skip = "for build debug")]
        public void Should_be_able_to_compress()
        {
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            var controllerContext = new ControllerContext(requestContext, new Mock<ControllerBase>().Object);
            var resultContext = new ResultExecutedContext(controllerContext, new Mock<ActionResult>().Object, false, null);

            httpContext.SetupGet(c => c.Request.Browser.MajorVersion).Returns(7);
            httpContext.SetupGet(c => c.Request.Headers).Returns(new NameValueCollection { { "Accept-Encoding", "*, deflate, gzip" } });
            httpContext.SetupGet(c => c.Response.Filter).Returns(new MemoryStream());

            var attribute = new CompressAttribute();

            httpContext.Setup(c => c.Response.AppendHeader("Content-encoding", "gzip")).Verifiable();
            httpContext.SetupSet(c => c.Response.Filter).Verifiable();

            attribute.OnResultExecuted(resultContext);

            httpContext.Verify();
        }
    }
}