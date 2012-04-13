#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using System.Reflection;
    using Xunit;

    public class FromTests
    {
        private readonly Assembly currentAssembly;

        public FromTests()
        {
            
            currentAssembly = GetType().Assembly;
        }

        [Fact]
        public void Show_return_types_from_this_assembly()
        {
            var assemblies = From.ThisAssembly();
            
            Assert.Contains(currentAssembly, assemblies);
        }

        [Fact]
        public void Show_return_types_from_assembly_containing_type()
        {
            var assemblies = From.AssemblyContainingType(GetType());
            Assert.Contains(currentAssembly, assemblies);
        }

        [Fact]
        public void Show_return_types_from_assembly_containing_generic_type()
        {
            var assemblies = From.AssemblyContainingType<FromTests>();
            Assert.Contains(currentAssembly, assemblies);
        }
    }
}