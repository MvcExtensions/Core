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
    using System.Diagnostics;

    /// <summary>
    /// Defines a class to store all the metadata of the models.
    /// </summary>
    public class ModelMetadataRegistry : IModelMetadataRegistry
    {
        private readonly IDictionary<Type, IDictionary<string, ModelMetadataItem>> configurations = new Dictionary<Type, IDictionary<string, ModelMetadataItem>>();

        /// <summary>
        /// Gets the configurations.
        /// </summary>
        /// <value>The configurations.</value>
        protected virtual IDictionary<Type, IDictionary<string, ModelMetadataItem>> Configurations
        {
            [DebuggerStepThrough]
            get
            {
                return configurations;
            }
        }

        /// <summary>
        /// Registers the specified model.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="metadataDictionary">The metadata dictionary.</param>
        public virtual void Register(Type modelType, IDictionary<string, ModelMetadataItem> metadataDictionary)
        {
            Invariant.IsNotNull(modelType, "modelType");
            Invariant.IsNotNull(metadataDictionary, "metadataDictionary");

            Configurations.Add(modelType, metadataDictionary);
        }

        /// <summary>
        /// Determines whether the specified model type is registered.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>
        /// <c>true</c> if the specified model type is registered; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsRegistered(Type modelType)
        {
            Invariant.IsNotNull(modelType, "modelType");

            return configurations.ContainsKey(modelType);
        }

        /// <summary>
        /// Determines whether the specified model type with the property name is registered.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// <c>true</c> if the specified model type with property name is registered; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsRegistered(Type modelType, string propertyName)
        {
            Invariant.IsNotNull(modelType, "modelType");

            IDictionary<string, ModelMetadataItem> properties;

            return configurations.TryGetValue(modelType, out properties) && properties.ContainsKey(propertyName);
        }

        /// <summary>
        /// Gets the Matching metadata of the given model.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns></returns>
        public virtual IDictionary<string, ModelMetadataItem> Matching(Type modelType)
        {
            Invariant.IsNotNull(modelType, "modelType");

            IDictionary<string, ModelMetadataItem> properties;

            return configurations.TryGetValue(modelType, out properties) ? properties : null;
        }

        /// <summary>
        /// Gets the Matching metadata of the given model property.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public virtual ModelMetadataItem Matching(Type modelType, string propertyName)
        {
            Invariant.IsNotNull(modelType, "modelType");
            Invariant.IsNotNull(propertyName, "propertyName");

            IDictionary<string, ModelMetadataItem> properties;

            if (!configurations.TryGetValue(modelType, out properties))
            {
                return null;
            }

            ModelMetadataItem propertyMetadata;

            return properties.TryGetValue(propertyName, out propertyMetadata) ? propertyMetadata : null;
        }
    }
}