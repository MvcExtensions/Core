#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;
    using Xunit;

    public class ImportViewDataFromTempDataAttributeTests
    {
        [Fact]
        public void Replace_existing_should_be_true()
        {
            Assert.True(new ImportViewDataFromTempDataAttribute().ReplaceExisting);
        }

        [Fact]
        public void Should_not_import_for_child_action()
        {
            var attribute = new ImportViewDataFromTempDataAttribute();

            var httpContext = new Mock<HttpContextBase>();

            var routeData = new RouteData();
            routeData.DataTokens.Add("ParentActionViewContext", new object());

            var requestContext = new RequestContext(httpContext.Object, routeData);

            var controller = new Mock<ControllerBase>();
            controller.Object.ViewData = new ViewDataDictionary();
            controller.Object.TempData = new TempDataDictionary { { attribute.Key, new ViewDataDictionary(new { foo = "bar" }) } };

            var controllerContext = new ControllerContext(requestContext, controller.Object);
            var actionContext = new ActionExecutingContext(controllerContext, new Mock<ActionDescriptor>().Object, new Dictionary<string, object>());

            attribute.OnActionExecuting(actionContext);

            Assert.False(actionContext.Controller.ViewData.ContainsKey("foo"));
        }

        [Fact]
        public void Should_not_import_when_temp_data_does_not_contain_previously_exported_view_data()
        {
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());

            var controller = new Mock<ControllerBase>();
            controller.Object.ViewData = new ViewDataDictionary();
            controller.Object.TempData = new TempDataDictionary();

            var controllerContext = new ControllerContext(requestContext, controller.Object);
            var actionContext = new ActionExecutingContext(controllerContext, new Mock<ActionDescriptor>().Object, new Dictionary<string, object>());

            var attribute = new ImportViewDataFromTempDataAttribute();

            attribute.OnActionExecuting(actionContext);

            Assert.False(actionContext.Controller.ViewData.ContainsKey("foo"));
        }

        [Fact]
        public void Should_import_complete_view_data()
        {
            var attribute = new ImportViewDataFromTempDataAttribute();

            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());

            var previousModel = new object();
            var previousViewData = new ViewDataDictionary(previousModel) { { @"foo", @"bar" } };

            previousViewData.ModelState.AddModelError(@"foo", @"bar");

            var controller = new Mock<ControllerBase>();
            controller.Object.ViewData = new ViewDataDictionary();
            controller.Object.TempData = new TempDataDictionary { { attribute.Key, previousViewData } };

            var controllerContext = new ControllerContext(requestContext, controller.Object);
            var actionContext = new ActionExecutingContext(controllerContext, new Mock<ActionDescriptor>().Object, new Dictionary<string, object>());

            attribute.OnActionExecuting(actionContext);

            Assert.True(actionContext.Controller.ViewData.ContainsKey("foo"));
            Assert.True(actionContext.Controller.ViewData.ModelState.ContainsKey("foo"));
            Assert.Same(previousModel, actionContext.Controller.ViewData.Model);
        }

        [Fact]
        public void Import_should_not_replace_when_replace_existing_is_set_to_false()
        {
            var attribute = new ImportViewDataFromTempDataAttribute { ReplaceExisting = false };

            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());

            var previousViewData = new ViewDataDictionary { { @"foo", @"bar" } };

            previousViewData.ModelState.AddModelError(@"foo", @"bar");

            var controller = new Mock<ControllerBase>();

            var currentViewData = new ViewDataDictionary { { @"foo", @"baz" } };
            currentViewData.ModelState.AddModelError(@"foo", @"baz");

            controller.Object.ViewData = currentViewData;
            controller.Object.TempData = new TempDataDictionary { { attribute.Key, previousViewData } };

            var controllerContext = new ControllerContext(requestContext, controller.Object);
            var actionContext = new ActionExecutingContext(controllerContext, new Mock<ActionDescriptor>().Object, new Dictionary<string, object>());

            attribute.OnActionExecuting(actionContext);

            Assert.Equal(actionContext.Controller.ViewData["foo"], "baz");
            Assert.Equal(actionContext.Controller.ViewData.ModelState["foo"].Errors[0].ErrorMessage, "baz");
        }
    }
}