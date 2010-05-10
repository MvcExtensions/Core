#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;
    using System.Web.Mvc;

    using Moq;
    using Xunit;

    public class RegisterModelBindersTests : IDisposable
    {
        private readonly ModelBinderDictionary modelBinders = new ModelBinderDictionary();
        private readonly RegisterModelBinders registration;
        private readonly Mock<FakeAdapter> adapter;

        public RegisterModelBindersTests()
        {
            RegisterModelBinders.Excluded = false;
            RegisterModelBinders.IgnoredTypes.Clear();

            modelBinders = new ModelBinderDictionary();
            adapter = new Mock<FakeAdapter>();

            registration = new RegisterModelBinders(modelBinders);
        }

        private interface IModel
        {
        }

        public void Dispose()
        {
            RegisterModelBinders.Excluded = false;
            RegisterModelBinders.IgnoredTypes.Clear();
        }

        [Fact]
        public void Should_not_register_model_binders_when_excluded()
        {
            var buildManager = SetupBuildManager();

            adapter.Setup(sl => sl.GetInstance<IBuildManager>()).Returns(buildManager.Object);
            adapter.Setup(sl => sl.GetAllInstances<IModelBinder>()).Returns(new IModelBinder[] { new FakeModelBinder1(), new FakeModelBinder2(), new FakeModelBinder3() });

            RegisterModelBinders.Excluded = true;

            registration.Execute(adapter.Object);

            Assert.Empty(modelBinders);
        }

        [Fact]
        public void Should_not_register_model_binders_when_model_binder_exists_in_ignore_list()
        {
            var buildManager = SetupBuildManager();

            adapter.Setup(sl => sl.GetInstance<IBuildManager>()).Returns(buildManager.Object);

            RegisterModelBinders.IgnoredTypes.Add(typeof(FakeModelBinder3));

            registration.Execute(adapter.Object);

            adapter.Verify(sr => sr.RegisterType(null, typeof(IModelBinder), typeof(FakeModelBinder3), LifetimeType.Singleton), Times.Never());
            adapter.Verify(sr => sr.RegisterType(null, typeof(IModelBinder), typeof(FakeModelBinder1), LifetimeType.Singleton), Times.Once());
            adapter.Verify(sr => sr.RegisterType(null, typeof(IModelBinder), typeof(FakeModelBinder2), LifetimeType.Singleton), Times.Once());
        }

        [Fact]
        public void Should_be_able_to_register_model_binders_for_model_and_its_inheriters()
        {
            var modelBinder = new FakeModelBinder1();

            var buildManager = SetupBuildManager();

            adapter.Setup(sl => sl.GetInstance<IBuildManager>()).Returns(buildManager.Object);
            adapter.Setup(sl => sl.GetAllInstances<IModelBinder>()).Returns(new[] { modelBinder });

            registration.Execute(adapter.Object);

            Assert.Same(modelBinders[typeof(DummyModel)], modelBinder);
            Assert.Same(modelBinders[typeof(DummyModel1)], modelBinder);
        }

        [Fact]
        public void Should_be_able_to_register_model_binders_for_model_and_its_implementers()
        {
            var modelBinder = new FakeModelBinder2();

            var buildManager = SetupBuildManager();

            adapter.Setup(sl => sl.GetInstance<IBuildManager>()).Returns(buildManager.Object);
            adapter.Setup(sl => sl.GetAllInstances<IModelBinder>()).Returns(new[] { modelBinder });

            registration.Execute(adapter.Object);

            Assert.Same(modelBinders[typeof(DummyModel2)], modelBinder);
            Assert.False(modelBinders.ContainsKey(typeof(IModel)));
        }

        [Fact]
        public void Should_be_able_to_register_model_binders_for_only_model()
        {
            var modelBinder = new FakeModelBinder3();

            var buildManager = SetupBuildManager();

            adapter.Setup(sl => sl.GetInstance<IBuildManager>()).Returns(buildManager.Object);
            adapter.Setup(sl => sl.GetAllInstances<IModelBinder>()).Returns(new[] { modelBinder });

            registration.Execute(adapter.Object);

            Assert.Same(modelBinders[typeof(DummyModel3)], modelBinder);
        }

        [Fact]
        public void Should_throw_exception_when_more_than_one_model_binder_exists_for_same_model()
        {
            var buildManager = SetupBuildManager();

            adapter.Setup(sl => sl.GetInstance<IBuildManager>()).Returns(buildManager.Object);
            adapter.Setup(sl => sl.GetAllInstances<IModelBinder>()).Returns(new IModelBinder[] { new FakeModelBinder3(), new FakeModelBinder4() });

            Assert.Throws<InvalidOperationException>(() => registration.Execute(adapter.Object));
        }

        private static Mock<IBuildManager> SetupBuildManager()
        {
            var buildManager = new Mock<IBuildManager>();

            buildManager.SetupGet(bm => bm.ConcreteTypes).Returns(new[] { typeof(DummyModel), typeof(DummyModel1), typeof(DummyModel2), typeof(DummyModel3), typeof(FakeModelBinder1), typeof(FakeModelBinder2), typeof(FakeModelBinder3) });

            return buildManager;
        }

        [BindingType(typeof(DummyModel), true)]
        private sealed class FakeModelBinder1 : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                return null;
            }
        }

        [BindingType(typeof(IModel), true)]
        private sealed class FakeModelBinder2 : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                return null;
            }
        }

        [BindingType(typeof(DummyModel3))]
        private sealed class FakeModelBinder3 : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                return null;
            }
        }

        [BindingType(typeof(DummyModel3))]
        private sealed class FakeModelBinder4 : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                return null;
            }
        }

        private class DummyModel
        {
        }

        private sealed class DummyModel1 : DummyModel
        {
        }

        private sealed class DummyModel2 : IModel
        {
        }

        private sealed class DummyModel3
        {
        }
    }
}