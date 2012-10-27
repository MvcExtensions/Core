#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion
namespace MvcExtensions.FluentMetadata.Tests
{
    using System;
    using System.Web.Mvc;
    using Moq;
    using Xunit;

    public class NoConventionsResourceSetTests : IDisposable
    {
        public NoConventionsResourceSetTests()
        {
            ConventionSettings.DefaultResourceType = null;
            ConventionSettings.ConventionsActive = true;
        }

        [Fact]
        public void Should_not_throw_if_сonventions_is_enabled_but_resurce_is_not_set()
        {
            var metadata = new RequiredValidationMetadata();
            var validator = metadata.CreateValidator(CreateMetadata(), new ControllerContext());

            Assert.NotNull(validator);
            Assert.DoesNotThrow(() => validator.GetClientValidationRules());
        }

        public void Dispose()
        {
            ConventionSettings.DefaultResourceType = null;
            ConventionSettings.ConventionsActive = false;
        }

        protected ExtendedModelMetadata CreateMetadata()
        {
            Func<object> accessor = () => new ValidationMetadataTestsBase.DummyObject();

            return new Mock<ExtendedModelMetadata>(new Mock<ModelMetadataProvider>().Object, GetType(), accessor, typeof(ValidationMetadataTestsBase.DummyObject), string.Empty, new Mock<ModelMetadataItem>().Object).Object;
        }

        public class DummyObject
        {
        }
    }
}