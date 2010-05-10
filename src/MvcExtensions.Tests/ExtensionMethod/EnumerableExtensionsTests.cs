#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using Xunit;

    public class EnumerableExtensionsTests
    {
        [Fact]
        public void Each_should_call_the_provided_action()
        {
            var list = new[] { 4 };
            bool called = false;

            list.Each(i => called = true);

            Assert.True(called);
        }
    }
}