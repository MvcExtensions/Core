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
    using System.Linq;
    using System.Web.Mvc;

    using Moq;
    using Xunit;

    public class RegisterViewEnginesTests : IDisposable
    {
        public RegisterViewEnginesTests()
        {
            RegisterViewEngines.Excluded = false;
            RegisterViewEngines.IgnoredTypes.Clear();
        }

        public void Dispose()
        {
            RegisterViewEngines.Excluded = false;
            RegisterViewEngines.IgnoredTypes.Clear();
        }

        [Fact]
        public void Should_register_available_view_engines()
        {
            var viewEngines = new ViewEngineCollection();

            new RegisterViewEngines(viewEngines).Execute(SetupAdapter().Object);

            Assert.NotEmpty(viewEngines);
        }

        [Fact]
        public void Should_not_register_view_engine_when_excluded()
        {
            var viewEngines = new ViewEngineCollection();
            RegisterViewEngines.Excluded = true;

            new RegisterViewEngines(viewEngines).Execute(SetupAdapter().Object);

            Assert.Empty(viewEngines);
        }

        [Fact]
        public void Should_not_register_view_engine_when_view_engine_exists_in_ignored_list()
        {
            var adapter = SetupAdapter();

            RegisterViewEngines.IgnoredTypes.Add(typeof(DummyViewEngine));

            new RegisterViewEngines(new ViewEngineCollection { new Mock<IViewEngine>().Object }).Execute(adapter.Object);

            adapter.Verify(a => a.RegisterType(null, typeof(IViewEngine), typeof(DummyViewEngine), LifetimeType.Singleton), Times.Never());
        }

        [Fact]
        public void Should_not_register_already_registered_view_engine()
        {
            var viewEngines = new ViewEngineCollection { new DummyViewEngine() };

            new RegisterViewEngines(viewEngines).Execute(SetupAdapter().Object);

            Assert.Equal(1, viewEngines.Count(ve => ve.GetType() == typeof(DummyViewEngine)));
        }

        private static Mock<FakeAdapter> SetupAdapter()
        {
            var buildManager = new Mock<IBuildManager>();
            buildManager.Setup(bm => bm.ConcreteTypes).Returns(new[] { typeof(DummyViewEngine) });

            var adapter = new Mock<FakeAdapter>();

            adapter.Setup(sl => sl.GetInstance<IBuildManager>()).Returns(buildManager.Object);

            var viewEngines = new List<IViewEngine>();

            adapter.Setup(a => a.RegisterType(null, It.IsAny<Type>(), It.IsAny<Type>(), LifetimeType.Singleton)).Callback((string k, Type t1, Type t2, LifetimeType lt) => viewEngines.Add((IViewEngine)Activator.CreateInstance(t2)));
            adapter.Setup(a => a.GetAllInstances<IViewEngine>()).Returns(() => viewEngines);

            return adapter;
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