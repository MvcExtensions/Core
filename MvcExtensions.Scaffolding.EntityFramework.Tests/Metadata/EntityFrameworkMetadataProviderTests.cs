#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework.Tests
{
    using System;

    using Xunit;
    using Xunit.Extensions;

    public class EntityFrameworkMetadataProviderTests
    {
        private readonly EntityFrameworkMetadataProvider provider;

        public EntityFrameworkMetadataProviderTests()
        {
            provider = new EntityFrameworkMetadataProvider(new Northwind());
        }

        [Theory]
        [InlineData("categories", typeof(Category), typeof(int))]
        [InlineData("customers", typeof(Customer), typeof(string))]
        [InlineData("employees", typeof(Employee), typeof(int))]
        [InlineData("suppliers", typeof(Supplier), typeof(int))]
        public void Should_be_able_to_map_entitysets(string entitySetName, Type entityType, Type keyType)
        {
            var mapping = provider.GetMetadata(entitySetName);

            Assert.Same(entityType, mapping.EntityType);
            Assert.Same(keyType, mapping.GetKeyTypes()[0]);
        }
    }
}