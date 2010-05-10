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

    public class FilterRegistryControllerItemTests
    {
        private readonly FilterRegistryControllerItem<FakeController> controllerItem;

        public FilterRegistryControllerItemTests()
        {
            controllerItem = new FilterRegistryControllerItem<FakeController>(new[] { new Func<FilterAttribute>(() => null) });
        }

        [Fact]
        public void IsMatching_should_return_true_for_same_controller()
        {
            var controllerContext = new ControllerContext
                                        {
                                            Controller = new FakeController()
                                        };

            Assert.True(controllerItem.IsMatching(controllerContext, new Mock<ActionDescriptor>().Object));
        }

        private sealed class FakeController : Controller
        {
        }
    }
}