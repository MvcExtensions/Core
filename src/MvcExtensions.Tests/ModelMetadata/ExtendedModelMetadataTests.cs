#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Web.Mvc;

    using Moq;
    using Xunit;

    public class ExtendedModelMetadataTests
    {
        [Fact]
        public void Metadata_should_be_same_which_is_passed_in_constructor()
        {
            var metadata = new Mock<ModelMetadataItem>();
            var modelMetadata = new ExtendedModelMetadata(new Mock<ModelMetadataProvider>().Object, typeof(object), () => null, typeof(object), "foo", metadata.Object);

            Assert.Same(metadata.Object, modelMetadata.Metadata);
        }
    }
}