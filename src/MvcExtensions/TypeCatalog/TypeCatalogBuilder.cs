#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines a class to fluently build <seealso cref="TypeCatalog"/>.
    /// </summary>
    public class TypeCatalogBuilder : IFluentSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeCatalogBuilder"/> class.
        /// </summary>
        public TypeCatalogBuilder() : this(new TypeCatalog())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeCatalogBuilder"/> class.
        /// </summary>
        /// <param name="typeCatalog">The type catalog.</param>
        public TypeCatalogBuilder(TypeCatalog typeCatalog)
        {
            Invariant.IsNotNull(typeCatalog, "typeCatalog");

            TypeCatalog = typeCatalog;
        }

        /// <summary>
        /// Gets the internal type catalog.
        /// </summary>
        /// <value>The type catalog.</value>
        public TypeCatalog TypeCatalog
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            get;
            private set;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="MvcExtensions.TypeCatalogBuilder"/> to <see cref="MvcExtensions.TypeCatalog"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TypeCatalog(TypeCatalogBuilder builder)
        {
            return ToTypeCatalog(builder);
        }

        /// <summary>
        /// Adds the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns></returns>
        public TypeCatalogBuilder Add(params Assembly[] assemblies)
        {
            Invariant.IsNotNull(assemblies, "assemblies");

            if (assemblies.Any())
            {
                foreach (Assembly assembly in assemblies.Where(assembly => !TypeCatalog.Assemblies.Contains(assembly)))
                {
                    TypeCatalog.Assemblies.Add(assembly);
                }
            }

            return this;
        }

        /// <summary>
        /// Adds the specified assemblies that matches the specified names. This method comes into action when the assembly is available in the application but does not have any direct reference.
        /// </summary>
        /// <param name="assemblyNames">The assembly names.</param>
        /// <returns></returns>
        public TypeCatalogBuilder Add(params string[] assemblyNames)
        {
            Invariant.IsNotNull(assemblyNames, "assemblyNames");

            if (assemblyNames.Any())
            {
                Add(assemblyNames.Select(name => Assembly.Load(name)).ToArray());
            }

            return this;
        }

        /// <summary>
        /// Includes the specified types.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        public TypeCatalogBuilder Include(params Type[] types)
        {
            Invariant.IsNotNull(types, "types");

            if (types.Any())
            {
                types.Each(type => Include(scannedType => scannedType == type));
            }

            return this;
        }

        /// <summary>
        /// Includes the types that matches specified names. This method comes into action when the type is available in the application but does not have any direct reference.
        /// </summary>
        /// <param name="typeNames">The type names.</param>
        /// <returns></returns>
        public TypeCatalogBuilder Include(params string[] typeNames)
        {
            Invariant.IsNotNull(typeNames, "typeNames");

            if (typeNames.Any())
            {
                Include(typeNames.Select(name => Type.GetType(name)).ToArray());
            }

            return this;
        }

        /// <summary>
        /// Includes the types that matches specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public TypeCatalogBuilder Include(Predicate<Type> filter)
        {
            Invariant.IsNotNull(filter, "filter");

            TypeCatalog.IncludeFilters.Add(filter);

            return this;
        }

        /// <summary>
        /// Excludes the specified types.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        public TypeCatalogBuilder Exclude(params Type[] types)
        {
            Invariant.IsNotNull(types, "types");

            if (types.Any())
            {
                types.Each(type => Exclude(scannedType => scannedType == type));
            }

            return this;
        }

        /// <summary>
        /// Excludes the types that matches specified names. This method comes into action when the type is available in the application but does not have any direct reference.
        /// </summary>
        /// <param name="typeNames">The type names.</param>
        /// <returns></returns>
        public TypeCatalogBuilder Exclude(params string[] typeNames)
        {
            Invariant.IsNotNull(typeNames, "typeNames");

            if (typeNames.Any())
            {
                Exclude(typeNames.Select(name => Type.GetType(name)).ToArray());
            }

            return this;
        }

        /// <summary>
        /// Excludes the types that matches specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public TypeCatalogBuilder Exclude(Predicate<Type> filter)
        {
            Invariant.IsNotNull(filter, "filter");

            TypeCatalog.ExcludeFilters.Add(filter);

            return this;
        }

        private static TypeCatalog ToTypeCatalog(TypeCatalogBuilder builder)
        {
            Invariant.IsNotNull(builder, "builder");

            return builder.TypeCatalog;
        }
    }
}