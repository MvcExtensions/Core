#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a base class that is used to validate model value.
    /// </summary>
    public abstract class ExtendedValidator : ModelValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedValidator"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="controllerContext">The controller context.</param>
        protected ExtendedValidator(ModelMetadata metadata, ControllerContext controllerContext)
            : base(metadata, controllerContext)
        {
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented in a derived class, validates the object.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>A list of validation results.</returns>
        public override sealed IEnumerable<ModelValidationResult> Validate(object container)
        {
            if (!ValidateCore(Metadata.Model))
            {
                yield return new ModelValidationResult
                                 {
                                     Message = string.Format(CultureInfo.CurrentCulture, ErrorMessage, Metadata.GetDisplayName())
                                 };
            }
        }

        /// <summary>
        /// When implemented in a derived class, validates the object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c></returns>
        protected abstract bool ValidateCore(object value);
    }
}