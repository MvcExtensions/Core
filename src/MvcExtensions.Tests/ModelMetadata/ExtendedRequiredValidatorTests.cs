#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using Xunit;

    public class ExtendedRequiredValidatorTests : ValidatorTestsBase
    {
        [Fact]
        public void Should_be_able_to_get_client_rules()
        {
            var validator = new ExtendedRequiredValidatorTestDouble(CreateModelMetadataWithModel(), new ControllerContext(), new RequiredValidationMetadata { ErrorMessage = () => "Value must be present." });

            var rules = validator.GetClientValidationRules();

            Assert.NotEmpty(rules);
            Assert.IsType<ModelClientValidationRequiredRule>(rules.First());
        }

        public class ExtendedRequiredValidatorTestDouble : ExtendedRequiredValidator
        {
            public ExtendedRequiredValidatorTestDouble(ModelMetadata metadata, ControllerContext controllerContext, IModelValidationMetadata validationMetadata) : base(metadata, controllerContext, validationMetadata)
            {
            }

            public RequiredAttribute PublicAttribute
            {
                get
                {
                    return Attribute;
                }
            }
        }
    }
}