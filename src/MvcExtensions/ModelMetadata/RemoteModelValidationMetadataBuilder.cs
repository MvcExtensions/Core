#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    /// <summary>
    /// Builds the <see cref="RemoteValidationMetadata"/>
    /// </summary>
    public class RemoteModelValidationMetadataBuilder : ModelValidationMetadataBuilder<RemoteModelValidationMetadataBuilder>
    {
        private string actionName;
        private string areaName;
        private string controllerName;

        /// <summary>
        /// Sets the name of the action.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public RemoteModelValidationMetadataBuilder Action(string name)
        {
            actionName = name;
            return this;
        }

        /// <summary>
        /// Sets the name of the area.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public RemoteModelValidationMetadataBuilder Area(string name)
        {
            areaName = name;
            return this;
        }

        /// <summary>
        /// Sets the name of the controller.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public RemoteModelValidationMetadataBuilder Controller(string name)
        {
            controllerName = name;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="IModelValidationMetadata"/>
        /// </summary>
        /// <returns></returns>
        protected override IModelValidationMetadata Build()
        {
            return new RemoteValidationMetadata
                       {
                           Area = areaName,
                           Action = actionName,
                           Controller = controllerName,
                           ErrorMessage = errorMessage,
                           ErrorMessageResourceType = errorMessageResourceType,
                           ErrorMessageResourceName = errorMessageResourceName,
                       };
        }
    }
}