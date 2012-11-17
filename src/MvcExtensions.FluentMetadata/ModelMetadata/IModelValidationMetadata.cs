#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.CompilerServices;
    using System.Web.Mvc;

    /// <summary>
    /// Represents an interface to store validation metadata.
    /// </summary>
    [TypeForwardedFrom(KnownAssembly.MvcExtensions)]
    public interface IModelValidationMetadata
    {
        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        ModelValidator CreateValidator(ExtendedModelMetadata metadata, ControllerContext context);

        /// <summary>
        /// Creates validation attribute
        /// </summary>
        /// <returns>Instance of ValidationAttribute type</returns>
        ValidationAttribute CreateValidationAttribute();
    }
}