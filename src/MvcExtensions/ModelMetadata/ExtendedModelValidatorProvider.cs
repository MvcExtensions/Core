#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a model validator provider which support fluent registration.
    /// </summary>
    public class ExtendedModelValidatorProvider : ModelValidatorProvider
    {
        /// <summary>
        /// Gets a list of validators.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="context">The context.</param>
        /// <returns>A list of validators.</returns>
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            Invariant.IsNotNull(metadata, "metadata");
            Invariant.IsNotNull(context, "context");

            ExtendedModelMetadata extendedModelMetadata = metadata as ExtendedModelMetadata;

            return (extendedModelMetadata != null) && (extendedModelMetadata.Metadata != null) ?
                   extendedModelMetadata.Metadata.Validations.Select(validationMeta => validationMeta.CreateValidator(extendedModelMetadata, context)) :
                   Enumerable.Empty<ModelValidator>();
        }
    }
}