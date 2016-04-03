#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using Xunit;

    public class ModelMetadataRegistryTests
    {
        private readonly ModelMetadataRegistry registry;

        public ModelMetadataRegistryTests()
        {
            registry = new ModelMetadataRegistry();
        }

        [Fact]
        public void GetModelPropertyMetadata_should_return_null_when_property_is_not_registered()
        {
            Assert.Null(registry.GetModelPropertyMetadata(typeof(object), "foo"));
        }

        #region Nested type: Dummy
        private class Dummy : DummyParent
        {
        }
        #endregion

        #region Nested type: DummyGrandParent
        private class DummyGrandParent
        {
        }
        #endregion

        #region Nested type: DummyParent
        private class DummyParent : DummyGrandParent
        {
        }
        #endregion
    }
}