#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;

    /// <summary>
    /// Represents a base class to bild an <see cref="IModelValidationMetadata"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ModelValidationMetadataBuilder<T> : IModelValidationMetadataBuilder
        where T : ModelValidationMetadataBuilder<T>
    {
        protected Func<string> errorMessage;
        protected string errorMessageResourceName;
        protected Type errorMessageResourceType;

        private T This
        {
            get { return (T) this; }
        }

        IModelValidationMetadata IModelValidationMetadataBuilder.Build()
        {
            return Build();
        }

        /// <summary>
        /// Sets the error message.
        /// </summary>
        /// <param name="text">The error message.</param>
        public T ErrorMessage(string text)
        {
            errorMessage = () => text;
            return This;
        }

        /// <summary>
        /// Sets the error message.
        /// </summary>
        /// <param name="text">The error message.</param>
        public T ErrorMessage(Func<string> text)
        {
            errorMessage = text;
            return This;
        }

        /// <summary>
        /// Sets the error message.
        /// </summary>
        /// <param name="resourceType">The name of the error message resource.</param>
        /// <param name="resourceName">The type of the error message resource.</param>
        public T ErrorMessage(Type resourceType, string resourceName)
        {
            errorMessageResourceType = resourceType;
            errorMessageResourceName = resourceName;
            return This;
        }

        /// <summary>
        /// Builds the <see cref="IModelValidationMetadata"/>
        /// </summary>
        /// <returns></returns>
        protected abstract IModelValidationMetadata Build();
    }
}