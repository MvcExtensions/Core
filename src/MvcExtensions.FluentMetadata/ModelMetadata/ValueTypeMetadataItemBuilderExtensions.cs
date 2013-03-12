#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines an static class which contains extension methods of <see cref="ModelMetadataItemBuilder{TValue}"/>.
    /// </summary>
    [TypeForwardedFrom(KnownAssembly.MvcExtensions)]
    public static class ValueTypeMetadataItemBuilderExtensions
    {
        /// <summary>
        /// Shows the value in currency format in both display and edit mode.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<decimal> FormatAsCurrency([NotNull] this ModelMetadataItemBuilder<decimal> instance)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.Format("{0:c}");
        }

        /// <summary>
        /// Shows the value in currency format in both display and edit mode.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<decimal?> FormatAsCurrency([NotNull] this ModelMetadataItemBuilder<decimal?> instance)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.Format("{0:c}");
        }

        /// <summary>
        /// Shows the only the date part of the value in both display and edit mode.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<DateTime> FormatAsDateOnly([NotNull] this ModelMetadataItemBuilder<DateTime> instance)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.Format("{0:d}");
        }

        /// <summary>
        /// Shows the only the date part of the value in both display and edit mode.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<DateTime?> FormatAsDateOnly([NotNull] this ModelMetadataItemBuilder<DateTime?> instance)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.Format("{0:d}");
        }

        /// <summary>
        /// Shows the only the time part of the value in both display and edit mode.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<DateTime> FormatAsTimeOnly([NotNull] this ModelMetadataItemBuilder<DateTime> instance)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.Format("{0:t}");
        }

        /// <summary>
        /// Shows the only the time part of the value in both display and edit mode.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<DateTime?> FormatAsTimeOnly([NotNull] this ModelMetadataItemBuilder<DateTime?> instance)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.Format("{0:t}");
        }
    }
}