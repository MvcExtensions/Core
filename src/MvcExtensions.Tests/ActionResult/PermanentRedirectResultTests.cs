#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;
    using Xunit;

    public class PermanentRedirectResultTests
    {
        private readonly Mock<HttpContextBase> httpContext;
        private readonly ControllerContext controllerContext;
        private readonly PermanentRedirectResult actionResult;

        public PermanentRedirectResultTests()
        {
            httpContext = new Mock<HttpContextBase>();
            controllerContext = new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);

            actionResult = new PermanentRedirectResult("http://dummyUrl.com/");
        }

        [Fact]
        public void ExecuteResult_should_clear_response()
        {
            httpContext.Setup(c => c.Response.Clear()).Verifiable();

            actionResult.ExecuteResult(controllerContext);

            httpContext.Verify();
        }

        [Fact]
        public void ExecuteResult_should_set_response_status_code_to_301()
        {
            httpContext.SetupSet(c => c.Response.StatusCode);
            httpContext.SetupSet(c => c.Response.Status);

            actionResult.ExecuteResult(controllerContext);

            httpContext.VerifySet(c => c.Response.StatusCode = (int)HttpStatusCode.MovedPermanently);
            httpContext.VerifySet(c => c.Response.Status = "301 Moved Permanently");
        }

        [Fact]
        public void ExecuteResult_should_suppress_content()
        {
            httpContext.SetupSet(c => c.Response.SuppressContent);

            actionResult.ExecuteResult(controllerContext);

            httpContext.VerifySet(c => c.Response.SuppressContent = true);
        }

        [Fact]
        public void ExecuteResult_should_set_redirect_location()
        {
            httpContext.SetupSet(c => c.Response.RedirectLocation);

            actionResult.ExecuteResult(controllerContext);

            httpContext.VerifySet(c => c.Response.RedirectLocation = actionResult.Url);
        }
    }
}