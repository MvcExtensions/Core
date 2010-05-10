#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;
    using Xunit;

    public class ExtendedAuthorizeAttributeTests
    {
        [Fact]
        public void OnAuthorization_should_not_execute_for_child_action()
        {
            var httpContext = new Mock<HttpContextBase>();

            var routeData = new RouteData();
            routeData.DataTokens.Add("ParentActionViewContext", new object());

            var requestContext = new RequestContext(httpContext.Object, routeData);
            var controllerContext = new ControllerContext(requestContext, new Mock<ControllerBase>().Object);
            var filterContext = new AuthorizationContext(controllerContext, new Mock<ActionDescriptor>().Object);

            var attribute = new ExtendedAuthorizeAttributeTestDouble(false);

            attribute.OnAuthorization(filterContext);

            Assert.Null(filterContext.Result);
        }

        [Fact]
        public void OnAuthorization_should_set_action_result_when_unauthorized()
        {
            var filterContext = new AuthorizationContext();
            var attribute = new ExtendedAuthorizeAttributeTestDouble(false);

            attribute.OnAuthorization(filterContext);

            Assert.IsType<HttpUnauthorizedResult>(filterContext.Result);
        }

        [Fact]
        public void OnAuthorization_should_return_cache_validation_status_as_valid()
        {
            var httpContext = new Mock<HttpContextBase>();

            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            var controllerContext = new ControllerContext(requestContext, new Mock<ControllerBase>().Object);
            var filterContext = new AuthorizationContext(controllerContext, new Mock<ActionDescriptor>().Object);

            HttpValidationStatus cacheValidation = HttpValidationStatus.IgnoreThisRequest;

            httpContext.Setup(c => c.Response.Cache.AddValidationCallback(It.IsAny<HttpCacheValidateHandler>(), It.IsAny<object>())).Callback((HttpCacheValidateHandler handler, object data) => handler(null, data, ref cacheValidation));

            var attribute = new ExtendedAuthorizeAttributeTestDouble(true);

            attribute.OnAuthorization(filterContext);

            Assert.Equal(HttpValidationStatus.Valid, cacheValidation);
        }

        private sealed class ExtendedAuthorizeAttributeTestDouble : ExtendedAuthorizeAttribute
        {
            private readonly bool authorized;

            public ExtendedAuthorizeAttributeTestDouble(bool authorized)
            {
                this.authorized = authorized;
            }

            public override bool IsAuthorized(AuthorizationContext filterContext)
            {
                return authorized;
            }
        }
    }
}