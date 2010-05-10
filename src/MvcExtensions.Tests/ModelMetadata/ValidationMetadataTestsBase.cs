#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;
    using System.Web.Mvc;

    using Moq;

    public abstract class ValidationMetadataTestsBase
    {
        protected ExtendedModelMetadata CreateMetadata()
        {
            Func<object> accessor = () => new DummyObject();

            return new Mock<ExtendedModelMetadata>(new Mock<ModelMetadataProvider>().Object, GetType(), accessor, typeof(DummyObject), string.Empty, new Mock<ModelMetadataItem>().Object).Object;
        }

        public class DummyObject
        {
        }
    }
}