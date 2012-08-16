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
    using System.Linq;

    /// <summary>
    /// Defines a class to store all the metadata of the models.
    /// </summary>
    public class ModelMetadataRegistry : IModelMetadataRegistry
    {
        private readonly IDictionary<Type, ModelMetadataRegistryItem> mappings = new Dictionary<Type, ModelMetadataRegistryItem>();

        /// <summary>
        /// Registers the model type metadata.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="metadataItem">The metadata.</param>
        public virtual void RegisterModel(Type modelType, ModelMetadataItem metadataItem)
        {
            Invariant.IsNotNull(modelType, "modelType");
            Invariant.IsNotNull(metadataItem, "metadataItem");

            ModelMetadataRegistryItem item = GetOrCreate(modelType);

            item.ClassMetadata = metadataItem;
        }

        /// <summary>
        /// Registers the specified model type properties metadata.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="metadataItems">The metadata dictionary.</param>
        public virtual void RegisterModelProperties(Type modelType, IDictionary<string, ModelMetadataItem> metadataItems)
        {
            Invariant.IsNotNull(modelType, "modelType");
            Invariant.IsNotNull(metadataItems, "metadataItems");

            ModelMetadataRegistryItem item = GetOrCreate(modelType);

            item.PropertiesMetadata.Clear();

            foreach (KeyValuePair<string, ModelMetadataItem> pair in metadataItems)
            {
                item.PropertiesMetadata.Add(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Gets the model metadata.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns></returns>
        public ModelMetadataItem GetModelMetadata(Type modelType)
        {
            var item = GetModelMetadataRegistryItem(modelType);
            return item != null ? item.ClassMetadata : null;
        }

        /// <summary>
        /// Gets the model property metadata.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public virtual ModelMetadataItem GetModelPropertyMetadata(Type modelType, string propertyName)
        {
            Invariant.IsNotNull(modelType, "modelType");

            ModelMetadataRegistryItem item = GetModelMetadataRegistryItem(modelType);
            if (item == null)
            {
                return null;
            }

            ModelMetadataItem propertyMetadata;

            return item.PropertiesMetadata.TryGetValue(propertyName, out propertyMetadata)
                       ? propertyMetadata
                       : null;
        }

        /// <summary>
        /// Gets the model properties metadata.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns></returns>
        public virtual IDictionary<string, ModelMetadataItem> GetModelPropertiesMetadata(Type modelType)
        {
            Invariant.IsNotNull(modelType, "modelType");

            ModelMetadataRegistryItem item = GetModelMetadataRegistryItem(modelType);
            return item == null ? null : item.PropertiesMetadata;
        }

        private ModelMetadataRegistryItem GetOrCreate(Type modelType)
        {
            ModelMetadataRegistryItem item;

            if (!mappings.TryGetValue(modelType, out item))
            {
                item = new ModelMetadataRegistryItem();
                mappings.Add(modelType, item);
            }

            return item;
        }

        private ModelMetadataRegistryItem GetModelMetadataRegistryItem(Type modelType)
        {
            Invariant.IsNotNull(modelType, "modelType");

            ModelMetadataRegistryItem item;

            if (mappings.TryGetValue(modelType, out item))
            {
                return item;
            }

            item = mappings
                .Where(registryItem => registryItem.Key.IsAssignableFrom(modelType))
                .OrderBy(x => x.Key, new TypeInheritanceComparer())
                .Select(x => x.Value)
                .FirstOrDefault();

            return item;
        }

        private sealed class ModelMetadataRegistryItem
        {
            public ModelMetadataRegistryItem()
            {
                PropertiesMetadata = new Dictionary<string, ModelMetadataItem>(StringComparer.OrdinalIgnoreCase);
            }

            public ModelMetadataItem ClassMetadata { get; set; }

            public IDictionary<string, ModelMetadataItem> PropertiesMetadata { get; private set; }
        }

        private sealed class TypeInheritanceComparer : IComparer<Type>
        {
            public int Compare(Type x, Type y)
            {
                return x == y ? 0 : (x.IsAssignableFrom(y) ? 1 : -1);
            }
        }
    }
}