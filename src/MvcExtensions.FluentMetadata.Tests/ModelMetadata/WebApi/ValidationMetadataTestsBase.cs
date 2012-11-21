#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion
#if !MVC_3
namespace MvcExtensions.FluentMetadata.Tests.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Metadata;
    using System.Web.Http.Validation;
    using Moq;
    using MvcExtensions.WebApi;

    public abstract class ValidationMetadataTestsBase
    {
        protected ExtendedModelMetadata CreateMetadata(string propertyName = "")
        {
            Func<object> accessor = () => new DummyObject();

            return new Mock<ExtendedModelMetadata>(new Mock<ModelMetadataProvider>().Object, GetType(), accessor, typeof(DummyObject), propertyName, new Mock<ModelMetadataItem>().Object).Object;
        }

        public IEnumerable<ModelValidatorProvider> GetProviders()
        {
            return Enumerable.Empty<ModelValidatorProvider>();
        }

        public class DummyObject
        {
        }
    }
}
#endif