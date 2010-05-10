#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using Xunit;

    public class ViewDataTempDataTransferAttributeTests
    {
        [Fact]
        public void Should_be_able_to_set_default_key()
        {
            ViewDataTempDataTransferAttribute.DefaultKey = "foo";

            Assert.Equal("foo", ViewDataTempDataTransferAttribute.DefaultKey);

            ViewDataTempDataTransferAttribute.DefaultKey = typeof(ViewDataTempDataTransferAttribute).FullName;
        }

        [Fact]
        public void Key_should_be_same_as_default_key_when_new_instance_is_created()
        {
            Assert.Equal(ViewDataTempDataTransferAttribute.DefaultKey, new ViewDataTempDataTransferAttributeTestDouble().Key);
        }

        [Fact]
        public void OnActionExecuting_should_do_nothing()
        {
            Assert.DoesNotThrow(() => new ViewDataTempDataTransferAttributeTestDouble().OnActionExecuting(null));
        }

        [Fact]
        public void OnActionExecuted_should_do_nothing()
        {
            Assert.DoesNotThrow(() => new ViewDataTempDataTransferAttributeTestDouble().OnActionExecuted(null));
        }

        private sealed class ViewDataTempDataTransferAttributeTestDouble : ViewDataTempDataTransferAttribute
        {
        }
    }
}