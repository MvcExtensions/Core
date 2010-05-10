#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Collections;
    using System.Web.Mvc;

    using Xunit;

    public class TypeCatalogTests
    {
        private readonly TypeCatalog catalog;

        public TypeCatalogTests()
        {
            catalog = new TypeCatalog();
        }

        [Fact]
        public void Should_be_able_to_iterate_correct_types_when_matched_with_include_filters()
        {
            catalog.Assemblies.Add(GetType().Assembly);
            catalog.IncludeFilters.Add(type => typeof(Controller).IsAssignableFrom(type));

            Assert.Contains(typeof(Dummy1Controller), catalog);
            Assert.Contains(typeof(Dummy2Controller), catalog);
        }

        [Fact]
        public void Should_be_able_to_iterate_correct_types_when_matched_with_exclude_filters()
        {
            catalog.Assemblies.Add(GetType().Assembly);
            catalog.IncludeFilters.Add(type => !typeof(Controller).IsAssignableFrom(type));

            Assert.DoesNotContain(typeof(Dummy1Controller), catalog);
            Assert.DoesNotContain(typeof(Dummy2Controller), catalog);
        }

        [Fact]
        public void Should_be_able_to_iterate_correct_types_when_matched_with_both_include_and_exclude_filters()
        {
            catalog.Assemblies.Add(GetType().Assembly);
            catalog.IncludeFilters.Add(type => typeof(Controller).IsAssignableFrom(type));
            catalog.ExcludeFilters.Add(type => type == typeof(Dummy2Controller));

            Assert.Contains(typeof(Dummy1Controller), catalog);
            Assert.DoesNotContain(typeof(Dummy2Controller), catalog);
        }

        [Fact]
        public void Should_be_able_to_iterate_correct_types_when_no_filter_is_applied()
        {
            catalog.Assemblies.Add(GetType().Assembly);

            Assert.Contains(typeof(Dummy1Controller), catalog);
            Assert.Contains(typeof(Dummy2Controller), catalog);
        }

        [Fact]
        public void Should_iterate_empty_types_when_no_assembly_is_added()
        {
            IEnumerable types = catalog;

            Assert.Empty(types);
        }
    }
}