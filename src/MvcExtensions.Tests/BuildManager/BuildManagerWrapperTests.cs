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

    public class BuildManagerWrapperTests
    {
        private readonly BuildManagerWrapper buildManagerWrapper;

        public BuildManagerWrapperTests()
        {
            buildManagerWrapper = new BuildManagerWrapper();
        }

        [Fact]
        public void Current_should_not_be_null()
        {
            Assert.NotNull(buildManagerWrapper);
        }

        [Fact]
        public void Assemblies_should_throw_exception_when_not_running_in_web_server()
        {
            Exception exception = null;

            try
            {
                var assemblies = buildManagerWrapper.Assemblies;
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.NotNull(exception);
        }

        [Fact]
        public void PublicTypes_should_throw_exception_when_not_running_in_web_server()
        {
            Exception exception = null;

            try
            {
                var publicTypes = buildManagerWrapper.PublicTypes;
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.NotNull(exception);
        }

        [Fact]
        public void ConcreteTypes_should_throw_exception_when_not_running_in_web_server()
        {
            Exception exception = null;

            try
            {
                var concreteTypes = buildManagerWrapper.ConcreteTypes;
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.NotNull(exception);
        }
    }
}