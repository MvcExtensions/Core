#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Web.Mvc;

    using Moq;
    using Xunit;

    public class RegisterModelBindersTests
    {
        private readonly Mock<ContainerAdapter> adapter;

        public RegisterModelBindersTests()
        {
            var buildManager = new Mock<IBuildManager>();
            buildManager.Setup(bm => bm.ConcreteTypes).Returns(new[] { typeof(DummyModelBinder) });

            adapter = new Mock<ContainerAdapter>();

            adapter.Setup(a => a.GetService(typeof(IBuildManager))).Returns(buildManager.Object);
        }

        [Fact]
        public void Should_register_available_model_binders()
        {
            adapter.Setup(a => a.RegisterType(null, typeof(DummyModelBinder), typeof(DummyModelBinder), LifetimeType.Transient)).Verifiable();

            new RegisterModelBinders(adapter.Object).Execute();

            adapter.Verify();
        }

        [Fact]
        public void Should_not_register_model_binder_when_exists_in_ignored_list()
        {
            var registration = new RegisterModelBinders(adapter.Object);

            registration.Ignore<DummyModelBinder>();

            registration.Execute();

            adapter.Verify(a => a.RegisterType(null, typeof(DummyModelBinder), typeof(DummyModelBinder), LifetimeType.Transient), Times.Never());
        }

        private sealed class DummyModelBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                return null;
            }
        }
    }
}