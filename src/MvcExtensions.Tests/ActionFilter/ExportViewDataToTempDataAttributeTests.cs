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

    public class ExportViewDataToTempDataAttributeTests
    {
        [Fact]
        public void Order_should_be_set_to_maximum_value()
        {
            Assert.Equal(int.MaxValue, new ExportViewDataToTempDataAttribute().Order);
        }

        [Fact]
        public void Should_not_export_when_action_is_canceled()
        {
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());

            var controller = new Mock<ControllerBase>();
            controller.Object.ViewData = new ViewDataDictionary();
            controller.Object.TempData = new TempDataDictionary();

            var controllerContext = new ControllerContext(requestContext, controller.Object);
            var actionContext = new ActionExecutedContext(controllerContext, new Mock<ActionDescriptor>().Object, true, null);

            var attribute = new ExportViewDataToTempDataAttribute();

            attribute.OnActionExecuted(actionContext);

            Assert.False(actionContext.Controller.TempData.ContainsKey(attribute.Key));
        }

        [Fact]
        public void Should_not_export_when_exception_occurrs_and_exception_is_not_handled()
        {
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());

            var controller = new Mock<ControllerBase>();
            controller.Object.ViewData = new ViewDataDictionary();
            controller.Object.TempData = new TempDataDictionary();

            var controllerContext = new ControllerContext(requestContext, controller.Object);
            var actionContext = new ActionExecutedContext(controllerContext, new Mock<ActionDescriptor>().Object, false, new InvalidOperationException());

            var attribute = new ExportViewDataToTempDataAttribute();

            attribute.OnActionExecuted(actionContext);

            Assert.False(actionContext.Controller.TempData.ContainsKey(attribute.Key));
        }

        [Fact]
        public void Should_not_export_for_ajax_request()
        {
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());

            var controller = new Mock<ControllerBase>();
            controller.Object.ViewData = new ViewDataDictionary();
            controller.Object.TempData = new TempDataDictionary();

            var controllerContext = new ControllerContext(requestContext, controller.Object);
            var actionContext = new ActionExecutedContext(controllerContext, new Mock<ActionDescriptor>().Object, false, null);

            httpContext.SetupGet(c => c.Request.Headers).Returns(new NameValueCollection { { "X-Requested-With", "XMLHttpRequest" } });

            var attribute = new ExportViewDataToTempDataAttribute();

            attribute.OnActionExecuted(actionContext);

            Assert.False(actionContext.Controller.TempData.ContainsKey(attribute.Key));
        }

        [Fact]
        public void Should_export_for_redirect_result()
        {
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());

            var controller = new Mock<ControllerBase>();
            controller.Object.ViewData = new ViewDataDictionary();
            controller.Object.TempData = new TempDataDictionary();

            var controllerContext = new ControllerContext(requestContext, controller.Object);
            var actionContext = new ActionExecutedContext(controllerContext, new Mock<ActionDescriptor>().Object, false, null)
                                    {
                                        Result = new RedirectResult("http://dummyurl.com")
                                    };

            httpContext.SetupGet(c => c.Request.Headers).Returns(new NameValueCollection());

            var attribute = new ExportViewDataToTempDataAttribute();

            attribute.OnActionExecuted(actionContext);

            Assert.True(actionContext.Controller.TempData.ContainsKey(attribute.Key));
        }

        [Fact]
        public void Should_export_for_redirect_to_route_result()
        {
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());

            var controller = new Mock<ControllerBase>();
            controller.Object.ViewData = new ViewDataDictionary();
            controller.Object.TempData = new TempDataDictionary();

            var controllerContext = new ControllerContext(requestContext, controller.Object);
            var actionContext = new ActionExecutedContext(controllerContext, new Mock<ActionDescriptor>().Object, false, null)
                                    {
                                        Result = new RedirectToRouteResult("foo", null)
                                    };

            httpContext.SetupGet(c => c.Request.Headers).Returns(new NameValueCollection());

            var attribute = new ExportViewDataToTempDataAttribute();

            attribute.OnActionExecuted(actionContext);

            Assert.True(actionContext.Controller.TempData.ContainsKey(attribute.Key));
        }
    }
}