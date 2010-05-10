#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;
    using Xunit;

    public class ExtendedJsonResultTests
    {
        private readonly Mock<HttpContextBase> httpContext;
        private readonly ControllerContext controllerContext;

        private readonly ExtendedJsonResult actionResult;

        public ExtendedJsonResultTests()
        {
            httpContext = new Mock<HttpContextBase>();
            controllerContext = new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);

            actionResult = new ExtendedJsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [Fact]
        public void Should_throw_exception_when_httpGet_is_denied_and_request_is_httpGet()
        {
            actionResult.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            httpContext.SetupGet(c => c.Request.HttpMethod).Returns("GET");

            Assert.Throws<InvalidOperationException>(() => actionResult.ExecuteResult(controllerContext));
        }

        [Fact]
        public void ExecuteResult_should_set_content_type()
        {
            httpContext.SetupSet(c => c.Response.ContentType = "application/json").Verifiable();

            actionResult.ExecuteResult(controllerContext);

            httpContext.Verify();
        }

        [Fact]
        public void ExecuteResult_should_set_content_type_if_there_is_any()
        {
            httpContext.SetupSet(c => c.Response.ContentEncoding = Encoding.UTF8).Verifiable();

            actionResult.ContentEncoding = Encoding.UTF8;

            actionResult.ExecuteResult(controllerContext);

            httpContext.Verify();
        }

        [Fact]
        public void ExecuteResult_should_write_json_in_response()
        {
            httpContext.Setup(c => c.Response.Write(It.IsAny<string>())).Verifiable();

            actionResult.Data = new { foo = "bar" };
            actionResult.ExecuteResult(controllerContext);

            httpContext.Verify();
        }
    }
}