#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Moq;
    using MvcExtensions.FluentMetadata.Tests.Resources;

    public abstract class LocalizationTestsBase : IDisposable
    {
        protected readonly Type DefaultConventionsResourceType;
        protected readonly ExtendedModelMetadataProvider Provider;
        protected readonly Mock<IModelMetadataRegistry> Registry;

        protected LocalizationTestsBase()
        {
            Registry = new Mock<IModelMetadataRegistry>();
            Provider = new ExtendedModelMetadataProvider(Registry.Object);

            DefaultConventionsResourceType = ConventionSettings.DefaultResourceType;
            ConventionSettings.DefaultResourceType = typeof(TestResource);
        }

        public void Dispose()
        {
            ConventionSettings.DefaultResourceType = DefaultConventionsResourceType;
        }

        protected string GetErrorMessageForAttributeBasedConfigiguredItem<T>(T model, string propertyName)
        {
            // arrange
            Registry.Setup(x => x.GetModelPropertyMetadata(model.GetType(), propertyName)).Returns((ModelMetadataItem)null);

            // act
            var modelTemp = model;
            var metadata = Provider.GetMetadataForProperty(() => modelTemp, model.GetType(), propertyName);
            var validator = metadata.GetValidators(new ControllerContext()).First();

            // assert
            return validator.GetClientValidationRules().First().ErrorMessage;
        }

        protected string GetErrorMessageForFluentlyConfigiguredItem<T>(
            T model,
            string propertyName,
            Func<ModelMetadataItem> getConfiguration,
            ModelValidationMetadata validationMetadata)
        {
            var propertyNameTemp = propertyName;
            Registry
                .Setup(x => x.GetModelPropertyMetadata(model.GetType(), propertyNameTemp))
                .Returns(getConfiguration());

            // act
            var modelTemp = model;
            var metadata =
                (ExtendedModelMetadata)
                Provider.GetMetadataForProperty(() => modelTemp, modelTemp.GetType(), propertyName);
            var validator = validationMetadata.CreateValidator(metadata, new ControllerContext());

            return validator.GetClientValidationRules().First().ErrorMessage;
        }
    }
}
