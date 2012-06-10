#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>, AlexBar <abarbashin@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web.Mvc;

    /// <summary>
    /// Represents RemoteValidationMetadata class
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
        /// Gets or sets the additional fields that are required for validation.
        /// </summary>
        /// <returns>The additional fields that are required for validation</returns>
        public string AdditionalFields
        {
            get;
            set;
        }

        /// <summary>
        /// The route name
        /// </summary>
        public string RouteName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HTTP method used for remote validation.
        /// </summary>
        /// <returns>The HTTP method used for remote validation. The default value is "Get".</returns>
        public string HttpMethod
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
                                ? new RemoteAttribute(RouteName)
                                : new RemoteAttribute(Action, Controller, Area);

            attribute.AdditionalFields = AdditionalFields;
            attribute.HttpMethod = HttpMethod;

            PopulateErrorMessage(attribute);
            return new DataAnnotationsModelValidator<RemoteAttribute>(modelMetadata, context, attribute);
        }
    }
}