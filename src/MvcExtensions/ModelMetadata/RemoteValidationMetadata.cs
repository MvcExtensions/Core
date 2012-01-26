#region Copyright
// Copyright (c) 2009 - 2011, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web.Mvc;

    /// <summary>
    /// Represents a class to store remote validation metadata.
    /// </summary>
    public class RemoteValidationMetadata : ModelValidationMetadata
    {
        /// <summary>
        /// Gets or sets the name of the area.
        /// </summary>
        public string Area
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the controller.
        /// </summary>
        public string Controller
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the action method.
        /// </summary>
        public string Action
        {
            get;
            set;
        }

        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <param name="modelMetadata"> The model metadata. </param>
        /// <param name="context"> The context. </param>
        /// <returns> </returns>
        protected override ModelValidator CreateValidatorCore(ExtendedModelMetadata modelMetadata, ControllerContext context)
        {
            var attribute = Area == null && Controller == null
                                ? new RemoteAttribute(Action)
                                : new RemoteAttribute(Action, Controller, Area);

            PopulateErrorMessage(attribute);
            return new DataAnnotationsModelValidator<RemoteAttribute>(modelMetadata, context, attribute);
        }
    }
}