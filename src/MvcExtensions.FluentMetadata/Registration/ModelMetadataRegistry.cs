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
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines a class to store all the metadata of the models.
    /// </summary>
    [TypeForwardedFrom(KnownAssembly.MvcExtensions)]
    public class ModelMetadataRegistry : IModelMetadataRegistry
    {
        private readonly ICollection<IPropertyModelMetadataConvention> conventions = new List<IPropertyModelMetadataConvention>();
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
        /// <param name="convention"><see cref="IPropertyModelMetadataConvention"/> class</param>
        public virtual void RegisterConvention([NotNull] IPropertyModelMetadataConvention convention)
        {
            Invariant.IsNotNull(convention, "convention");

            conventions.Add(convention);
        }

        /// <summary>
        /// Registers the model type metadata.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="metadataItem">The metadata.</param>
        public virtual void RegisterModel([NotNull] Type modelType, [NotNull] ModelMetadataItem metadataItem)
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
        public virtual void RegisterModelProperties([NotNull] Type modelType, [NotNull] IDictionary<string, Func<ModelMetadataItem>> metadataItems)
        {
            Invariant.IsNotNull(modelType, "modelType");
            Invariant.IsNotNull(metadataItems, "metadataItems");

            var item = GetOrCreate(modelType);

            item.PropertiesMetadata.Clear();

            foreach (var pair in metadataItems)
            {
                item.PropertiesMetadata.Add(pair.Key, pair.Value());
            }
        }

        /// <summary>
        /// Gets the model metadata.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns></returns>
        public ModelMetadataItem GetModelMetadata([NotNull] Type modelType)
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
        public virtual ModelMetadataItem GetModelPropertyMetadata([NotNull] Type modelType, string propertyName)
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
        public virtual IDictionary<string, ModelMetadataItem> GetModelPropertiesMetadata([NotNull] Type modelType)
        {
            Invariant.IsNotNull(modelType, "modelType");

            var item = GetModelMetadataRegistryItem(modelType);
            return item == null ? null : item.PropertiesMetadata;
        }

        private ModelMetadataRegistryItem GetModelMetadataRegistryItem([NotNull] Type modelType)
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

            return CheckMetadataAndApplyConvetions(modelType, item);
        }

        private ModelMetadataRegistryItem CheckMetadataAndApplyConvetions([NotNull] Type modelType, ModelMetadataRegistryItem item)
        {
            if (conventions.Count == 0 || NoNeedToApplyConvetionsFor(modelType, item))
            {
                return item;
            }

            lock (this)
            {
                if (NoNeedToApplyConvetionsFor(modelType, item))
                {
                    return item;
                }

                ModelMetadataRegistryItem registeredItem;
                // ensure that conventions were not appied by another thread
                if (mappings.TryGetValue(modelType, out registeredItem) && registeredItem.IsConventionsApplied)
                {
                    return registeredItem;
                }

                var context = new AcceptorContext(modelType, item != null);
                var canAcceptConventions = ConventionAcceptor.CanAcceptConventions(context);

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

        private bool NoNeedToApplyConvetionsFor(Type modelType, ModelMetadataRegistryItem item)
        {
            return item == null && ignoredClassesCache.Contains(modelType) || item != null && item.IsConventionsApplied;
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

        private void ApplyMetadataConvenstions([NotNull] Type modelType, [NotNull] ModelMetadataRegistryItem item)
        {
            var properties = modelType.GetProperties();
            foreach (var convention in conventions)
            {
                var metadataConvention = convention;
                foreach (var pi in properties.Where(metadataConvention.IsApplicable))
                {
                    ModelMetadataItem metadataItem;
                    if (!item.PropertiesMetadata.TryGetValue(pi.Name, out metadataItem))
                    {
                        metadataItem = new ModelMetadataItem();
                        item.PropertiesMetadata.Add(pi.Name, metadataItem);
                    }

                    var conventionalItem = new ModelMetadataItem();
                    convention.Apply(pi, conventionalItem);
                    conventionalItem.MergeTo(metadataItem);
                }
            }

            item.IsConventionsApplied = true;
        }

        private ModelMetadataRegistryItem GetOrCreate([NotNull] Type modelType)
        {
            ModelMetadataRegistryItem item;

            if (!mappings.TryGetValue(modelType, out item))
            {
                item = new ModelMetadataRegistryItem();
                mappings.TryAdd(modelType, item);
            }

            return item;
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

        sealed class TypeInheritanceComparer : IComparer<Type>
        {
            public int Compare([NotNull] Type x, Type y)
            {
                if (x == y) return 0;
                if (x.IsAssignableFrom(y)) return 1;
                if (y.IsAssignableFrom(x)) return -1;
                return 0;
            }
        }
    }
}
