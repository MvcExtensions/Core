namespace MvcExtensions.Tests
{
    using System;
    using System.Web.Mvc;
    using Moq;
    using Xunit;

    public class FilterProviderTests
    {
        private readonly Mock<IFilterRegistry> filterRegistryMock = new Mock<IFilterRegistry>();
        private readonly FilterProvider provider;

        public FilterProviderTests()
        {
            provider = new FilterProvider(filterRegistryMock.Object);
        }

        [Fact]
        public void Matching_should_return_matched_filters()
        {
            var controllerContext = new ControllerContext
                                        {
                                            Controller = new Dummy1Controller()
                                        };

            var controllerDescriptor = new Mock<ControllerDescriptor>();
            controllerDescriptor.SetupGet(cd => cd.ControllerName).Returns("Dummy1");

            var actionDescriptor = new Mock<ActionDescriptor>();
            actionDescriptor.SetupGet(ad => ad.ControllerDescriptor).Returns(controllerDescriptor.Object);
            actionDescriptor.SetupGet(ad => ad.ActionName).Returns("Index");

            filterRegistryMock.Setup(x => x.Items)
                .Returns(new FilterRegistryItem[]
                             {
                                 new FilterRegistryActionItem<Dummy1Controller>(x => x.Index(), new Func<IMvcFilter>[] { () => new DummyFilter2(), () => new DummyFilter3() }),
                                 new FilterRegistryControllerItem<Dummy1Controller>(new Func<IMvcFilter>[] { () => new DummyFilter1(), () => new DummyFilter4() })
                             });

            var filters = new FilterInfo(provider.GetFilters(controllerContext, actionDescriptor.Object));

            Assert.IsType<DummyFilter1>(filters.AuthorizationFilters[0]);
            Assert.IsType<DummyFilter2>(filters.ActionFilters[0]);
            Assert.IsType<DummyFilter3>(filters.ResultFilters[0]);
            Assert.IsType<DummyFilter4>(filters.ExceptionFilters[0]);
        }

        private sealed class DummyFilter1 : FilterAttribute, IAuthorizationFilter
        {
            #region IAuthorizationFilter Members

            public void OnAuthorization(AuthorizationContext filterContext)
            {
            }

            #endregion
        }

        #region Nested type: DummyFilter2

        private sealed class DummyFilter2 : FilterAttribute, IActionFilter
        {
            #region IActionFilter Members

            public void OnActionExecuting(ActionExecutingContext filterContext)
            {
            }

            public void OnActionExecuted(ActionExecutedContext filterContext)
            {
            }

            #endregion
        }

        #endregion

        #region Nested type: DummyFilter3

        private sealed class DummyFilter3 : FilterAttribute, IResultFilter
        {
            #region IResultFilter Members

            public void OnResultExecuting(ResultExecutingContext filterContext)
            {
            }

            public void OnResultExecuted(ResultExecutedContext filterContext)
            {
            }

            #endregion
        }

        #endregion

        #region Nested type: DummyFilter4

        private sealed class DummyFilter4 : FilterAttribute, IExceptionFilter
        {
            #region IExceptionFilter Members

            public void OnException(ExceptionContext filterContext)
            {
            }

            #endregion
        }

        #endregion
    }
}