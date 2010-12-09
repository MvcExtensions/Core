#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Moq;
    using Xunit;

    public class RegisterViewEnginesTests
    {
        private readonly Mock<ContainerAdapter> adapter;

        public RegisterViewEnginesTests()
        {
            var buildManager = new Mock<IBuildManager>();
            buildManager.Setup(bm => bm.ConcreteTypes).Returns(new[] { typeof(DummyViewEngine) });

            var viewEngines = new List<IViewEngine>();

            adapter = new Mock<ContainerAdapter>();
            adapter.Setup(a => a.GetService(typeof(IBuildManager))).Returns(buildManager.Object);
            adapter.Setup(a => a.RegisterType(null, It.IsAny<Type>(), It.IsAny<Type>(), LifetimeType.Singleton)).Callback((string k, Type t1, Type t2, LifetimeType lt) => viewEngines.Add((IViewEngine)Activator.CreateInstance(t2)));
            adapter.Setup(a => a.GetServices(typeof(IViewEngine))).Returns(() => viewEngines);
        }

        [Fact]
        public void Should_register_available_view_engines()
        {
            adapter.Setup(a => a.RegisterType(null, typeof(IViewEngine), typeof(DummyViewEngine), LifetimeType.Singleton)).Verifiable();

            new RegisterViewEngines(adapter.Object).Execute();

            adapter.Verify();
        }

        [Fact]
        public void Should_not_register_view_engine_when_view_engine_exists_in_ignored_list()
        {
            var registration = new RegisterViewEngines(adapter.Object);

            registration.Ignore<DummyViewEngine>();

            registration.Execute();

            adapter.Verify(a => a.RegisterType(null, typeof(IViewEngine), typeof(DummyViewEngine), LifetimeType.Singleton), Times.Never());
        }

        private sealed class DummyViewEngine : IViewEngine
        {
            public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
            {
                return null;
            }

            public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
            {
                return null;
            }

            public void ReleaseView(ControllerContext controllerContext, IView view)
            {
            }
        }
    }
}