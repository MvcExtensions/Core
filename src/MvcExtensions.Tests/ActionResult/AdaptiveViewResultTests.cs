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
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;
    using Xunit;

    public class AdaptiveViewResultTests
    {
        private readonly Mock<HttpContextBase> httpContext;
        private readonly ControllerContext controllerContext;

        private readonly AdaptiveViewResult actionResult;

        public AdaptiveViewResultTests()
        {
            httpContext = new Mock<HttpContextBase>();
            controllerContext = new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);

            actionResult = new AdaptiveViewResult();
        }

        [Fact]
        public void ExecuteResult_should_return_json_result_when_request_is_ajax()
        {
            httpContext.SetupGet(c => c.Request.Headers).Returns(new NameValueCollection { { "X-Requested-With", "XMLHttpRequest" } });

            httpContext.Setup(c => c.Response.Clear()).Verifiable();
            httpContext.SetupSet(c => c.Response.ContentType = "application/json").Verifiable();
            httpContext.Setup(c => c.Response.Write(It.IsAny<string>())).Verifiable();

            actionResult.ExecuteResult(controllerContext);

            httpContext.Verify();
        }

        [Fact]
        public void ExecuteResult_should_render_regular_view_when_request_is_not_ajax()
        {
            httpContext.SetupGet(c => c.Request.Headers).Returns(new NameValueCollection());

            // Regular view generates exception in web environment
            Assert.Throws<InvalidOperationException>(() => actionResult.ExecuteResult(controllerContext));
        }
    }
}