#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework.Tests
{
    using Xunit;

    public class EntitySetMappingTests
    {
        private readonly EntitySetMapping mapping;

        public EntitySetMappingTests()
        {
            mapping = new EntitySetMapping(typeof(object), typeof(int));
        }

        [Fact]
        public void EntityType_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Same(typeof(object), mapping.EntityType);
        }

        [Fact]
        public void KeyType_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Same(typeof(int), mapping.KeyType);
        }
    }
}