namespace MvcExtensions.WebApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Metadata;
    using System.Web.Http.Validation;

    /// <summary>
    /// Defines a class which is used to maintain multiple model validator provider.
    /// </summary>
    public class WebApiCompositeModelValidatorProvider : ModelValidatorProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiCompositeModelValidatorProvider"/> class.
        /// </summary>
        /// <param name="providers">The providers.</param>
        public WebApiCompositeModelValidatorProvider(params ModelValidatorProvider[] providers)
        {
            Invariant.IsNotNull(providers, "providers");

            Providers = providers;
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <value>The providers.</value>
        public IEnumerable<ModelValidatorProvider> Providers { get; private set; }

        /// <summary>
        /// Gets a list of validators.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="validatorProviders">The list of model validator providers</param>
        /// <returns>A list of validators.</returns>
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders)
        {
            Invariant.IsNotNull(metadata, "metadata");
            Invariant.IsNotNull(validatorProviders, "validatorProviders");

            IEnumerable<ModelValidator> validators = null;

            foreach (ModelValidatorProvider provider in Providers)
            {
                validators = provider.GetValidators(metadata, validatorProviders);

                if (validators.Any())
                {
                    break;
                }
            }

            return validators ?? Enumerable.Empty<ModelValidator>();
        }
    }
}