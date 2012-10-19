#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines a class to store all the metadata of the models.
    /// </summary>
    public class ModelMetadataRegistry : IModelMetadataRegistry
    {
        private readonly ICollection<IPropertyMetadataConvention> conventions = new List<IPropertyMetadataConvention>();
        private readonly ConcurrentBag<Type> ignoredClassesCache = new ConcurrentBag<Type>();
        private readonly ConcurrentDictionary<Type, ModelMetadataRegistryItem> mappings = new ConcurrentDictionary<Type, ModelMetadataRegistryItem>();
        private IModelConventionAcceptor conventionAcceptor = new DefaultModelConventionAcceptor();

        /// <summary>
        /// Default acceptor for metadata classes
        /// </summary>
        public IModelConventionAcceptor ConventionAcceptor
        {
            get
            {
                return conventionAcceptor;
            }
            set
            {
                Invariant.IsNotNull(value, "value");
                conventionAcceptor = value;
            }
        }

        /// <summary>
        /// Register a new convention
        /// </summary>
        /// <param name="convention"><see cref="IPropertyMetadataConvention"/> class</param>
        public virtual void RegisterConvention(IPropertyMetadataConvention convention)
        {
            Invariant.IsNotNull(convention, "convention");

            conventions.Add(convention);
        }

        /// <summary>
        /// Registers the model type metadata.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="metadataItem">The metadata.</param>
        public virtual void RegisterModel(Type modelType, ModelMetadataItem metadataItem)
        {
            Invariant.IsNotNull(modelType, "modelType");
            Invariant.IsNotNull(metadataItem, "metadataItem");

            var item = GetOrCreate(modelType);

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

            var item = GetOrCreate(modelType);

            item.PropertiesMetadata.Clear();

            foreach (var pair in metadataItems)
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

            var item = GetModelMetadataRegistryItem(modelType);
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

            var item = GetModelMetadataRegistryItem(modelType);
            return item == null ? null : item.PropertiesMetadata;
        }

        private void ApplyMetadataConvenstions(Type modelType, ModelMetadataRegistryItem item)
        {
            var properties = modelType.GetProperties();
            foreach (var convention in conventions)
            {
                var metadataConvention = convention;
                foreach (var pi in properties.Where(metadataConvention.CanBeAccepted))
                {
                    ModelMetadataItem metadataItem;
                    if (!item.PropertiesMetadata.TryGetValue(pi.Name, out metadataItem))
                    {
                        metadataItem = new ModelMetadataItem();
                        item.PropertiesMetadata.Add(pi.Name, metadataItem);
                    }

                    var conventionMetadata = convention.CreateMetadataRules();

                    conventionMetadata.MergeTo(metadataItem);
                }
            }

            item.IsConventionsApplied = true;
        }

        private ModelMetadataRegistryItem GetModelMetadataRegistryItem(Type modelType)
        {
            Invariant.IsNotNull(modelType, "modelType");

            ModelMetadataRegistryItem item;
            if (!mappings.TryGetValue(modelType, out item))
            {
                item = mappings
                    .Where(registryItem => registryItem.Key.IsAssignableFrom(modelType))
                    .OrderBy(x => x.Key, new TypeInheritanceComparer())
                    .Select(x => x.Value)
                    .FirstOrDefault();
            }

            return ProcessExistingOrNewModelMetadataRegistryItem(modelType, item);
        }

        private ModelMetadataRegistryItem GetOrCreate(Type modelType)
        {
            ModelMetadataRegistryItem item;

            if (!mappings.TryGetValue(modelType, out item))
            {
                item = new ModelMetadataRegistryItem();
                mappings.TryAdd(modelType, item);
            }

            return item;
        }

        private ModelMetadataRegistryItem ProcessExistingOrNewModelMetadataRegistryItem(Type modelType, ModelMetadataRegistryItem item)
        {
            if (item == null && ignoredClassesCache.Contains(modelType) || item != null && item.IsConventionsApplied)
            {
                return item;
            }

            var context = new AcceptorContext(modelType, item != null);
            var canAcceptConventions = ConventionAcceptor.CanAcceptConventions(context);

            lock (this)
            {
                if (!canAcceptConventions)
                {
                    ProcessUnacceptedModelType(modelType, item);
                    return item;
                }

                if (item == null)
                {
                    // try get existing (item can be created by another thread) or create new
                    item = GetOrCreate(modelType);
                }

                // ensure convenstion is not applied yet
                if (item.IsConventionsApplied)
                {
                    return item;
                }

                ApplyMetadataConvenstions(modelType, item);
            }

            return item;
        }

        private void ProcessUnacceptedModelType(Type modelType, ModelMetadataRegistryItem item)
        {
            if (item == null)
            {
                // mark item as ignored
                if (!ignoredClassesCache.Contains(modelType))
                {
                    ignoredClassesCache.Add(modelType);
                }
            }
            else
            {
                // if we have some metadata item, 
                // just mark it as processed and do not add any conventions
                item.IsConventionsApplied = true;
            }
        }

        /// <summary>
        /// Holds metadata configuration information
        /// </summary>
        private sealed class ModelMetadataRegistryItem
        {
            /// <summary>
            /// Creates <see cref="ModelMetadataRegistryItem"/> instance
            /// </summary>
            public ModelMetadataRegistryItem()
            {
                PropertiesMetadata = new Dictionary<string, ModelMetadataItem>(StringComparer.OrdinalIgnoreCase);
            }

            /// <summary>
            /// Holds metadata for class
            /// </summary>
            public ModelMetadataItem ClassMetadata { get; set; }

            /// <summary>
            /// Identifies if convensions were applied
            /// </summary>
            public bool IsConventionsApplied { get; set; }

            /// <summary>
            /// Holds metadata for properties
            /// </summary>
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
