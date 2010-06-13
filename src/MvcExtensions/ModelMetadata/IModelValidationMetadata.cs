#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Represents an interface to store validation metadata.
    /// </summary>
    public interface IModelValidationMetadata
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        Func<string> ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the error message resource.
        /// </summary>
        /// <value>The type of the error message resource.</value>
        Type ErrorMessageResourceType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the error message resource.
        /// </summary>
        /// <value>The name of the error message resource.</value>
        string ErrorMessageResourceName
        {
            get;
            set;
        }

        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <param name="modelMetadata">The model metadata.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        ModelValidator CreateValidator(ExtendedModelMetadata modelMetadata, ControllerContext context);
    }
}