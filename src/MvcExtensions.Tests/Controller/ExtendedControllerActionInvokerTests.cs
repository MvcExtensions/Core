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

    public class ExtendedControllerActionInvokerTests
    {
        private readonly Mock<ContainerAdapter> adapter;
        private readonly Mock<IFilterRegistry> filterRegistry;
        private readonly ExtendedControllerActionInvokerTestDouble controllerActionInvoker;

        public ExtendedControllerActionInvokerTests()
        {
            adapter = new Mock<ContainerAdapter>();
            filterRegistry = new Mock<IFilterRegistry>();

            adapter.Setup(a => a.GetInstance<IFilterRegistry>()).Returns(filterRegistry.Object);

            controllerActionInvoker = new ExtendedControllerActionInvokerTestDouble(adapter.Object);
        }

        [Fact]
        public void GetFilters_should_merge_filters_in_ascending_order()
        {
            var controller = new FakeController();

            var decoratedAuthorizationFilter = new DummyFilter1 { Order = 2 };
            var decoratedActionFilter = new DummyFilter2 { Order = 2 };
            var decoratedResultFilter = new DummyFilter3 { Order = 2 };
            var decoratedExceptionFilter = new DummyFilter4 { Order = 2 };

            var decoratedFilters = new FilterInfo();

            decoratedFilters.AuthorizationFilters.Add(decoratedAuthorizationFilter);
            decoratedFilters.ActionFilters.Add(decoratedActionFilter);
            decoratedFilters.ResultFilters.Add(decoratedResultFilter);
            decoratedFilters.ExceptionFilters.Add(decoratedExceptionFilter);

            decoratedFilters.AuthorizationFilters.Add(controller);
            decoratedFilters.ActionFilters.Add(controller);
            decoratedFilters.ResultFilters.Add(controller);
            decoratedFilters.ExceptionFilters.Add(controller);

            var actionDescriptor = new Mock<ActionDescriptor>();
            //TODO: This is obsolete, need to replace it with new version.
            // actionDescriptor.Setup(ad => ad.GetFilters()).Returns(decoratedFilters);

            var registeredAuthorizationFilter = new DummyFilter5 { Order = 1 };
            var registeredActionFilter = new DummyFilter6 { Order = 1 };
            var registeredResultFilter = new DummyFilter7 { Order = 1 };
            var registeredExceptionFilter = new DummyFilter8 { Order = 1 };

            var registeredFilters = new FilterInfo();

            registeredFilters.AuthorizationFilters.Add(registeredAuthorizationFilter);
            registeredFilters.ActionFilters.Add(registeredActionFilter);
            registeredFilters.ResultFilters.Add(registeredResultFilter);
            registeredFilters.ExceptionFilters.Add(registeredExceptionFilter);

            filterRegistry.Setup(fr => fr.Matching(It.IsAny<ControllerContext>(), It.IsAny<ActionDescriptor>())).Returns(registeredFilters);

            var controllerContext = new ControllerContext(new RequestContext(new Mock<HttpContextBase>().Object, new RouteData()), new Mock<ControllerBase>().Object);

            var mergedFilters = controllerActionInvoker.PublicGetFilters(controllerContext, actionDescriptor.Object);

            Assert.Same(controller, mergedFilters.AuthorizationFilters[0]);
            Assert.Same(registeredAuthorizationFilter, mergedFilters.AuthorizationFilters[1]);
            Assert.Same(decoratedAuthorizationFilter, mergedFilters.AuthorizationFilters[2]);

            Assert.Same(controller, mergedFilters.ActionFilters[0]);
            Assert.Same(registeredActionFilter, mergedFilters.ActionFilters[1]);
            Assert.Same(decoratedActionFilter, mergedFilters.ActionFilters[2]);

            Assert.Same(controller, mergedFilters.ResultFilters[0]);
            Assert.Same(registeredResultFilter, mergedFilters.ResultFilters[1]);
            Assert.Same(decoratedResultFilter, mergedFilters.ResultFilters[2]);

            Assert.Same(controller, mergedFilters.ExceptionFilters[0]);
            Assert.Same(registeredExceptionFilter, mergedFilters.ExceptionFilters[1]);
            Assert.Same(decoratedExceptionFilter, mergedFilters.ExceptionFilters[2]);
        }

        private sealed class ExtendedControllerActionInvokerTestDouble : ExtendedControllerActionInvoker
        {
            public ExtendedControllerActionInvokerTestDouble(ContainerAdapter container) : base(container)
            {
            }

            public FilterInfo PublicGetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
            {
                return GetFilters(controllerContext, actionDescriptor);
            }
        }

        private sealed class FakeController : Controller
        {
        }

        private sealed class DummyFilter1 : FilterAttribute, IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationContext filterContext)
            {
            }
        }

        private sealed class DummyFilter2 : FilterAttribute, IActionFilter
        {
            public void OnActionExecuting(ActionExecutingContext filterContext)
            {
            }

            public void OnActionExecuted(ActionExecutedContext filterContext)
            {
            }
        }

        private sealed class DummyFilter3 : FilterAttribute, IResultFilter
        {
            public void OnResultExecuting(ResultExecutingContext filterContext)
            {
            }

            public void OnResultExecuted(ResultExecutedContext filterContext)
            {
            }
        }

        private sealed class DummyFilter4 : FilterAttribute, IExceptionFilter
        {
            public void OnException(ExceptionContext filterContext)
            {
            }
        }

        private sealed class DummyFilter5 : FilterAttribute, IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationContext filterContext)
            {
            }
        }

        private sealed class DummyFilter6 : FilterAttribute, IActionFilter
        {
            public void OnActionExecuting(ActionExecutingContext filterContext)
            {
            }

            public void OnActionExecuted(ActionExecutedContext filterContext)
            {
            }
        }

        private sealed class DummyFilter7 : FilterAttribute, IResultFilter
        {
            public void OnResultExecuting(ResultExecutingContext filterContext)
            {
            }

            public void OnResultExecuted(ResultExecutedContext filterContext)
            {
            }
        }

        private sealed class DummyFilter8 : FilterAttribute, IExceptionFilter
        {
            public void OnException(ExceptionContext filterContext)
            {
            }
        }
    }
}