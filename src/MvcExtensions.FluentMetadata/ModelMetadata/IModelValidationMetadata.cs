#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Web.Mvc;
#if !MVC_3
    using ModelValidatorProvider = System.Web.Http.Validation.ModelValidatorProvider;
#endif

    /// <summary>
    /// Represents an interface to store validation metadata.
    /// </summary>
    [TypeForwardedFrom(KnownAssembly.MvcExtensions)]
    public interface IModelValidationMetadata
    {
        /// <summary>
        /// Creates the Mvc validator.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        ModelValidator CreateValidator(ExtendedModelMetadata metadata, ControllerContext context);

#if !MVC_3
        /// <summary>
        /// Creates the WebApi validator.
        /// </summary>
        /// <param name="validatorProviders">WebApi validator providers.</param>
        /// <returns></returns>
        System.Web.Http.Validation.ModelValidator CreateWebApiValidator(IEnumerable<ModelValidatorProvider> validatorProviders);
#endif

    }
}
