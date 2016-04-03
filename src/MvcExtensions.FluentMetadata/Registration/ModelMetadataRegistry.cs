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
        readonly ICollection<IPropertyModelMetadataConvention> conventions = new List<IPropertyModelMetadataConvention>();
        readonly IDictionary<Type, IModelMetadataConfiguration> configurations = new Dictionary<Type, IModelMetadataConfiguration>();
        readonly ConcurrentDictionary<Type, ModelMetadataRegistryItem> mappings = new ConcurrentDictionary<Type, ModelMetadataRegistryItem>();
        IModelConventionAcceptor conventionAcceptor = new DefaultModelConventionAcceptor();

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
        /// Gets the model property metadata.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public virtual ModelMetadataItem GetModelPropertyMetadata([NotNull] Type modelType, string propertyName)
        {
            Invariant.IsNotNull(modelType, "modelType");

            var item = mappings.GetOrAdd(modelType, t => CreateModelMetadataRegistryItem(t));
            if (item != null) return item.GetPropertyMetadata(propertyName);
            return null;
        }

        /// <summary>
        /// Gets the model properties metadata.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns></returns>
        public virtual IDictionary<string, ModelMetadataItem> GetModelPropertiesMetadata([NotNull] Type modelType)
        {
            Invariant.IsNotNull(modelType, "modelType");

            var item = mappings.GetOrAdd(modelType, t => CreateModelMetadataRegistryItem(t));
            if (item != null) return item.PropertiesMetadata;
            return null;
        }

        private ModelMetadataRegistryItem CreateModelMetadataRegistryItem([NotNull] Type modelType)
        {
            Invariant.IsNotNull(modelType, "modelType");

            IModelMetadataConfiguration configuration;

            if (!configurations.TryGetValue(modelType, out configuration))
            {
                configuration = configurations
                    .Where(kvp => kvp.Key.IsAssignableFrom(modelType))
                    .OrderBy(x => x.Key, new TypeInheritanceComparer())
                    .Select(x => x.Value)
                    .FirstOrDefault();
            }

            var canAcceptConventions = ConventionAcceptor.CanAcceptConventions(new AcceptorContext(modelType, configuration != null));

            if (configuration == null && !canAcceptConventions)
            {
                return null;
            }

            var item = new ModelMetadataRegistryItem();

            if (canAcceptConventions)
            {
                var properties = modelType.GetProperties();
                foreach (var convention in conventions)
                {
                    foreach (var pi in properties)
                    {
                        if (convention.IsApplicable(pi))
                        {
                            var propertyMetadata = item.GetPropertyMetadataOrCreateNew(pi.Name);
                            convention.Apply(pi, propertyMetadata);
                        }
                    }
                }
            }

            if (configuration != null)
            {
                foreach (var pair in configuration.Configurations)
                {
                    var name = pair.Key;
                    var configurator = pair.Value;
                    var propertyMetadata = item.GetPropertyMetadataOrCreateNew(name);
                    configurator.Configure(propertyMetadata);
                }
            }

            return item;
        }

        /// <summary>
        /// Holds metadata configuration information
        /// </summary>
        private sealed class ModelMetadataRegistryItem
        {
            readonly IDictionary<string, ModelMetadataItem> propertiesMetadata;

            public ModelMetadataRegistryItem()
            {
                propertiesMetadata = new Dictionary<string, ModelMetadataItem>(StringComparer.OrdinalIgnoreCase);
            }

            /// <summary>
            /// Holds metadata for properties
            /// </summary>
            public IDictionary<string, ModelMetadataItem> PropertiesMetadata
            {
                get
                {
                    return propertiesMetadata;
                }
            }

            public ModelMetadataItem GetPropertyMetadataOrCreateNew(string name)
            {
                ModelMetadataItem propertyMetadata;
                if (!PropertiesMetadata.TryGetValue(name, out propertyMetadata))
                {
                    propertyMetadata = new ModelMetadataItem();
                    PropertiesMetadata.Add(name, propertyMetadata);
                }

                return propertyMetadata;
            }

            public ModelMetadataItem GetPropertyMetadata(string propertyName)
            {
                ModelMetadataItem propertyMetadata;
                PropertiesMetadata.TryGetValue(propertyName, out propertyMetadata);
                return propertyMetadata;
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public void RegisterConfiguration(IModelMetadataConfiguration configuration)
        {
            Invariant.IsNotNull(configuration, "configuration");

            configurations.Add(configuration.ModelType, configuration);
        }
    }
}
