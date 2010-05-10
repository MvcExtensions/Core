#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using Xunit;

    public class TypeExtensionsTests
    {
        [Fact]
        public void HasDefaultConstructor_should_return_true_for_default_constructor_type()
        {
            Assert.True(typeof(TypeExtensionsTests).HasDefaultConstructor());
        }

        [Fact]
        public void HasDefaultConstructor_should_return_false_for_non_default_constructor_type()
        {
            Assert.False(typeof(ObjectWithArgument).HasDefaultConstructor());
        }

        [Fact]
        public void Should_be_able_to_iterate_public_type_of_assembly()
        {
            Assert.NotEmpty(GetType().Assembly.PublicTypes());
        }

        [Fact]
        public void Should_be_able_to_iterate_public_type_of_assemblies()
        {
            var assemblies = new[] { typeof(ExtendedMvcApplication).Assembly, GetType().Assembly };

            Assert.NotEmpty(assemblies.PublicTypes());
        }

        [Fact]
        public void Should_be_able_to_iterate_concrete_type_of_assembly()
        {
            Assert.NotEmpty(GetType().Assembly.ConcreteTypes());
        }

        [Fact]
        public void Should_be_able_to_iterate_concrete_type_of_assemblies()
        {
            var assemblies = new[] { typeof(ExtendedMvcApplication).Assembly, GetType().Assembly };

            Assert.NotEmpty(assemblies.ConcreteTypes());
        }

        private sealed class ObjectWithArgument
        {
            public ObjectWithArgument(object argumet)
            {
            }
        }
    }
}