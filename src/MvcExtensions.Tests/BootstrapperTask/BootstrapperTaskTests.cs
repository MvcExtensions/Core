#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using Microsoft.Practices.ServiceLocation;

    using Moq;
    using Xunit;

    public class BootstrapperTaskTests
    {
        [Fact]
        public void Should_be_able_to_execute()
        {
            var task = new BootstrapperTaskTestDouble();

            var continuation = task.Execute(new Mock<IServiceLocator>().Object);

            Assert.Equal(TaskContinuation.Continue, continuation);
        }

        private sealed class BootstrapperTaskTestDouble : BootstrapperTask
        {
            protected override TaskContinuation ExecuteCore(IServiceLocator locator)
            {
                return TaskContinuation.Continue;
            }
        }
    }
}