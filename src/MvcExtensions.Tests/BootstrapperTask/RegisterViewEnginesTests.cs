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
        private readonly Mock<ContainerAdapter> adapter;

        public RegisterViewEnginesTests()
        {
            RegisterViewEngines.Excluded = false;
            RegisterViewEngines.IgnoredTypes.Clear();

            var buildManager = new Mock<IBuildManager>();
            buildManager.Setup(bm => bm.ConcreteTypes).Returns(new[] { typeof(DummyViewEngine) });

            var viewEngines = new List<IViewEngine>();

            adapter = new Mock<ContainerAdapter>();
            adapter.Setup(a => a.GetInstance<IBuildManager>()).Returns(buildManager.Object);
            adapter.Setup(a => a.RegisterType(null, It.IsAny<Type>(), It.IsAny<Type>(), LifetimeType.Singleton)).Callback((string k, Type t1, Type t2, LifetimeType lt) => viewEngines.Add((IViewEngine)Activator.CreateInstance(t2)));
            adapter.Setup(a => a.GetAllInstances<IViewEngine>()).Returns(() => viewEngines);
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

            new RegisterViewEngines(adapter.Object, viewEngines).Execute();

            Assert.NotEmpty(viewEngines);
        }

        [Fact]
        public void Should_not_register_view_engine_when_excluded()
        {
            var viewEngines = new ViewEngineCollection();
            RegisterViewEngines.Excluded = true;

            new RegisterViewEngines(adapter.Object, viewEngines).Execute();

            Assert.Empty(viewEngines);
        }

        [Fact]
        public void Should_not_register_view_engine_when_view_engine_exists_in_ignored_list()
        {
            RegisterViewEngines.IgnoredTypes.Add(typeof(DummyViewEngine));

            new RegisterViewEngines(adapter.Object, new ViewEngineCollection { new Mock<IViewEngine>().Object }).Execute();

            adapter.Verify(a => a.RegisterType(null, typeof(IViewEngine), typeof(DummyViewEngine), LifetimeType.Singleton), Times.Never());
        }

        [Fact]
        public void Should_not_register_already_registered_view_engine()
        {
            var viewEngines = new ViewEngineCollection { new DummyViewEngine() };

            new RegisterViewEngines(adapter.Object, viewEngines).Execute();

            Assert.Equal(1, viewEngines.Count(ve => ve.GetType() == typeof(DummyViewEngine)));
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