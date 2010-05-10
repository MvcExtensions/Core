#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Web.Mvc;

    using Xunit;

    public class TypeCatalogBuilderTests
    {
        private readonly TypeCatalogBuilder builder;

        public TypeCatalogBuilderTests()
        {
            builder = new TypeCatalogBuilder();
        }

        [Fact]
        public void Should_be_able_to_cast_implicitly_to_type_catalog()
        {
            TypeCatalog catalog = builder;

            Assert.NotNull(catalog);
        }

        [Fact]
        public void Should_be_able_to_add_assembly()
        {
            TypeCatalog catalog = builder.Add(GetType().Assembly);

            Assert.Contains(GetType().Assembly, catalog.Assemblies);
        }

        [Fact]
        public void Should_be_able_to_add_assembly_by_name()
        {
            TypeCatalog catalog = builder.Add(GetType().Assembly.FullName);

            Assert.Contains(GetType().Assembly, catalog.Assemblies);
        }

        [Fact]
        public void Should_be_able_to_include_types()
        {
            TypeCatalog catalog = builder.Include(typeof(Controller));

            Assert.Equal(1, catalog.IncludeFilters.Count);
        }

        [Fact]
        public void Should_be_able_to_include_type_by_name()
        {
            TypeCatalog catalog = builder.Include(typeof(Controller).FullName);

            Assert.Equal(1, catalog.IncludeFilters.Count);
        }

        [Fact]
        public void Should_be_able_to_include_type_flter()
        {
            TypeCatalog catalog = builder.Include(type => typeof(Controller).IsAssignableFrom(type));

            Assert.Equal(1, catalog.IncludeFilters.Count);
        }

        [Fact]
        public void Should_be_able_to_exclude_types()
        {
            TypeCatalog catalog = builder.Exclude(typeof(Controller));

            Assert.Equal(1, catalog.ExcludeFilters.Count);
        }

        [Fact]
        public void Should_be_able_to_exclude_type_by_name()
        {
            TypeCatalog catalog = builder.Exclude(typeof(Controller).FullName);

            Assert.Equal(1, catalog.ExcludeFilters.Count);
        }

        [Fact]
        public void Should_be_able_to_exclude_type_flter()
        {
            TypeCatalog catalog = builder.Exclude(type => typeof(Controller).IsAssignableFrom(type));

            Assert.Equal(1, catalog.ExcludeFilters.Count);
        }
    }
}