#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    /// Holds all configaration types
    /// </summary>
    public sealed class ConfigurationsScanner : IEnumerable<ConfigurationsScanResult>
    {
        private readonly IEnumerable<Type> types;

        /// <summary>
        /// Create an instaince of <see cref="ConfigurationsScanner"/>
        /// </summary>
        public ConfigurationsScanner(IEnumerable<Type> types)
        {
            this.types = types;
        }

        /// <summary>
        /// Get metadata configuration classes from specified assemblies
        /// </summary>
        /// <param name="assemblies">Assemlies to scan for types</param>
        /// <returns></returns>
        [NotNull]
        public static ConfigurationsScanner GetMetadataClasses(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
            {
                assemblies = Enumerable.Empty<Assembly>();
            }

            return new ConfigurationsScanner(assemblies.SelectMany(a => a.GetLoadableTypes()));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        [NotNull]
        public IEnumerator<ConfigurationsScanResult> GetEnumerator()
        {
            return GetConfigurationTypes().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [NotNull]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Performs the specified action to all of the assembly scan results.
        /// </summary>
        public void ForEach([NotNull] Action<ConfigurationsScanResult> action)
        {
            Invariant.IsNotNull(action, "action");
            foreach (var result in this)
            {
                action(result);
            }
        }

        /// <summary>
        /// Perfoms a search for all <see cref="IModelMetadataConfiguration"/> types
        /// </summary>
        [NotNull]
        public IEnumerable<ConfigurationsScanResult> GetConfigurationTypes()
        {
            return GetConfigurationTypes(types);
        }

        [NotNull]
        private static IEnumerable<ConfigurationsScanResult> GetConfigurationTypes([NotNull] IEnumerable<Type> types)
        {
            var modelMetadataConfigurationType = typeof(IModelMetadataConfiguration);
            return types
                .Where(
                    t =>
                    t.IsClass && !t.IsAbstract && t.IsVisible &&
                    !t.IsInterface && !t.IsGenericType &&
                    modelMetadataConfigurationType.IsAssignableFrom(t))
                .Select(t => new ConfigurationsScanResult(t));
        }
    }
}
