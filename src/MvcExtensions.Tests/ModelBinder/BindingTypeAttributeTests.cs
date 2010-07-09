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

    using Xunit;

    public class BindingTypeAttributeTests
    {
        [Fact]
        public void ModelType_should_be_same_which_passed_in_constructor()
        {
            var attribute = new BindingTypeAttribute(typeof(BindingTypeAttributeTests));
            Assert.Same(typeof(BindingTypeAttributeTests), attribute.ModelType);
        }

        [Fact]
        public void Inherited_should_be_same_which_passed_in_constructor()
        {
            var attribute = new BindingTypeAttribute(typeof(BindingTypeAttributeTests), true);

            Assert.True(attribute.Inherited);
        }

        [Fact]
        public void Should_throw_exception_when_invalid_type_is_passed_and_inherited_is_false()
        {
            Assert.Throws<ArgumentException>(() => new BindingTypeAttribute(typeof(IActionFilter)));
        }

        [Fact]
        public void Should_not_throw_exception_for_primitive_type()
        {
            Assert.DoesNotThrow(() => new BindingTypeAttribute(typeof(DateTime)));
        }
    }
}