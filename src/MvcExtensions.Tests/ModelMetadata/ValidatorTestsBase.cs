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

    public abstract class ValidatorTestsBase
    {
        protected ModelMetadata CreateModelMetadataWithModel()
        {
            Func<object> accessor = () => new DummyObject();

            return new Mock<ModelMetadata>(new Mock<ModelMetadataProvider>().Object, GetType(), accessor, typeof(DummyObject), string.Empty).Object;
        }

        protected ModelMetadata CreateModelMetadataWithoutModel()
        {
            return new Mock<ModelMetadata>(new Mock<ModelMetadataProvider>().Object, GetType(), null, typeof(DummyObject), string.Empty).Object;
        }

        public class DummyObject
        {
            public string Property1 { get; set; }

            public int Property2 { get; set; }
        }
    }
}