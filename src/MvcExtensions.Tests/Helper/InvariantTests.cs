#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;

    using Xunit;

    public class InvariantTests
    {
        [Fact]
        public void IsNotNull_should_throw_exception_when_passing_null_value()
        {
            Assert.Throws<ArgumentNullException>(() => Invariant.IsNotNull(null, "x"));
        }

        [Fact]
        public void IsNotNull_should_not_throw_exception_when_passing_non_null_value()
        {
            Assert.DoesNotThrow(() => Invariant.IsNotNull(new object(), "x"));
        }
    }
}