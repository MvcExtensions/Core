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
    using System.Runtime.CompilerServices;
    using System.Web.Mvc;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines a class which is used to maintain multiple model validator provider.
    /// </summary>
    [TypeForwardedFrom(KnownAssembly.MvcExtensions)]
    public class CompositeModelValidatorProvider : ModelValidatorProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeModelValidatorProvider"/> class.
        /// </summary>
        /// <param name="providers">The providers.</param>
        public CompositeModelValidatorProvider([NotNull] params ModelValidatorProvider[] providers)
        {
            Invariant.IsNotNull(providers, "providers");

            Providers = providers;
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <value>The providers.</value>
        public IEnumerable<ModelValidatorProvider> Providers
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a list of validators.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="context">The context.</param>
        /// <returns>A list of validators.</returns>
        [NotNull]
        public override IEnumerable<ModelValidator> GetValidators([NotNull] ModelMetadata metadata, [NotNull] ControllerContext context)
        {
            Invariant.IsNotNull(metadata, "metadata");
            Invariant.IsNotNull(context, "context");

            IEnumerable<ModelValidator> validators = null;

            foreach (ModelValidatorProvider provider in Providers)
            {
                validators = provider.GetValidators(metadata, context);

                if (validators.Any())
                {
                    break;
                }
            }

            return validators ?? Enumerable.Empty<ModelValidator>();
        }
    }
}