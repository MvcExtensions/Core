#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.WebApi
{
    using System.Collections.Generic;
    using System.Web.Http.Metadata;
    using System.Web.Http.Validation;

    /// <summary>
    /// WebApiModelValidationProvider
    /// </summary>
    public class WebApiModelValidationProvider : ModelValidatorProvider
    {
        /// <summary>
        /// Gets a list of validators associated with this <see cref="T:System.Web.Http.Validation.ModelValidatorProvider"/>.
        /// </summary>
        /// <returns>
        /// The list of validators.
        /// </returns>
        /// <param name="metadata">The metadata.</param>
        /// <param name="validatorProviders">The validator providers.</param>
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders)
        {
            Invariant.IsNotNull(metadata, "metadata");
            Invariant.IsNotNull(validatorProviders, "validatorProviders");

            var extendedModelMetadata = metadata as ExtendedModelMetadata;

            if (extendedModelMetadata == null || extendedModelMetadata.Metadata == null)
            {
                yield break;
            }

            foreach (var validation in extendedModelMetadata.Metadata.Validations)
            {
                var validator = validation.CreateWebApiValidator(validatorProviders);
                if (validator != null)
                {
                    yield return validator;
                }
            }
        }
    }
}
