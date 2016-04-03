#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents an interface to store all the metadata of the models.
    /// </summary>
    [TypeForwardedFrom(KnownAssembly.MvcExtensions)]
    public interface IModelMetadataRegistry
    {
        /// <summary>
        /// Register a new convention
        /// </summary>
        /// <param name="convention"><see cref="IPropertyModelMetadataConvention"/> class</param>
        void RegisterConvention(IPropertyModelMetadataConvention convention);

        /// <summary>
        /// Registers an <see cref="IModelMetadataConfiguration"/>
        /// </summary>
        /// <param name="configuration"></param>
        void RegisterConfiguration(IModelMetadataConfiguration configuration);

        /// <summary>
        /// Gets the model property metadata.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        ModelMetadataItem GetModelPropertyMetadata(Type modelType, string propertyName);

        /// <summary>
        /// Gets the model properties metadata.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns></returns>
        IDictionary<string, ModelMetadataItem> GetModelPropertiesMetadata(Type modelType);

    }
}