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

    public class DisposableTests
    {
        private DisposableTestDouble disposable;

        public DisposableTests()
        {
            disposable = new DisposableTestDouble();
        }

        [Fact]
        public void Should_be_able_to_dispose()
        {
            Assert.DoesNotThrow(() => disposable.Dispose());
        }

        [Fact]
        public void Should_finalize()
        {
            disposable = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private sealed class DisposableTestDouble : Disposable
        {
        }
    }
}