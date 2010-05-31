#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework.Tests
{
    using System;
    using System.Web;
    using System.Web.Routing;

    using Moq;
    using Xunit;

    public class ScaffoldedControllerFactoryTests
    {
        private readonly Mock<IEntityFrameworkMetadataProvider> provider;
        private readonly ScaffoldedControllerFactoryTestDouble factory;

        public ScaffoldedControllerFactoryTests()
        {
            provider = new Mock<IEntityFrameworkMetadataProvider>();
            factory = new ScaffoldedControllerFactoryTestDouble(new Mock<ContainerAdapter>().Object, new Mock<IActionInvokerRegistry>().Object, provider.Object);
        }

        [Fact]
        public void Should_return_scaffolded_controller_type_when_controller_name_matches_with_entityset_name()
        {
            var metadata = new Mock<EntityMetadata>("categories", typeof(Category));

            metadata.SetupGet(m => m.KeyType).Returns(typeof(int));

            provider.Setup(p => p.GetMetadata(It.IsAny<string>())).Returns(metadata.Object);

            var type = factory.PublicGetControllerType(new RequestContext(), "categories");

            Assert.Same(typeof(ScaffoldedController<Category, int>), type);
        }

        [Fact]
        public void Should_throw_exception_when_not_running_in_webserver_and_controller_name_does_not_match_with_entityset_name()
        {
            var requestContext = new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());

            Assert.Throws<InvalidOperationException>(() => factory.PublicGetControllerType(requestContext, "Dummy"));
        }

        private sealed class ScaffoldedControllerFactoryTestDouble : ScaffoldedControllerFactory
        {
            public ScaffoldedControllerFactoryTestDouble(ContainerAdapter container, IActionInvokerRegistry actionInvokerRegistry, IEntityFrameworkMetadataProvider metadataProvider) : base(container, actionInvokerRegistry, metadataProvider)
            {
            }

            public Type PublicGetControllerType(RequestContext requestContext, string controllerName)
            {
                return GetControllerType(requestContext, controllerName);
            }
        }

        private sealed class Category
        {
        }
    }
}