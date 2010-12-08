#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines a class that is used to filter types for one or more assemblies.
    /// </summary>
    public class TypeCatalog : IEnumerable<Type>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeCatalog"/> class.
        /// </summary>
        public TypeCatalog() : this(new List<Assembly>(), new List<Predicate<Type>>(), new List<Predicate<Type>>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeCatalog"/> class.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="includeFilters">The include filters.</param>
        /// <param name="excludeFilters">The exclude filters.</param>
        protected TypeCatalog(IList<Assembly> assemblies, IList<Predicate<Type>> includeFilters, IList<Predicate<Type>> excludeFilters)
        {
            Invariant.IsNotNull(assemblies, "assemblies");
            Invariant.IsNotNull(assemblies, "includeFilters");
            Invariant.IsNotNull(assemblies, "excludeFilters");

            Assemblies = assemblies;
            IncludeFilters = includeFilters;
            ExcludeFilters = excludeFilters;
        }

        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        /// <value>The assemblies.</value>
        public IList<Assembly> Assemblies
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            get;
            private set;
        }

        /// <summary>
        /// Gets the include type filters.
        /// </summary>
        /// <value>The include filters.</value>
        public IList<Predicate<Type>> IncludeFilters
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            get;
            private set;
        }

        /// <summary>
        /// Gets the exclude type filters.
        /// </summary>
        /// <value>The exclude filters.</value>
        public IList<Predicate<Type>> ExcludeFilters
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            get;
            private set;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Type> GetEnumerator()
        {
            IEnumerable<Type> filteredTypes = Assemblies.SelectMany(assembly => assembly.GetExportedTypes());

            if (IncludeFilters.Any())
            {
                filteredTypes = filteredTypes.Where(type => IncludeFilters.Any(filter => filter(type)));
            }

            return filteredTypes.Where(type => !ExcludeFilters.Any(filter => filter(type))).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}