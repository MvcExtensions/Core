namespace MvcExtensions
{
    using System;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    /// <summary>
    /// 
    /// </summary>
    [TypeForwardedFrom(KnownAssembly.MvcExtensions)]
    public static class RangeMetadataItemBuilderExtensions
    {
        /// <summary>
        /// Sets the range of value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<TValue> Range<TValue>([NotNull] this ModelMetadataItemBuilder<TValue> self, TValue minimum, TValue maximum)
            where TValue : IComparable
        {
            return self.Range(minimum, maximum, null, null, null);
        }

        /// <summary>
        /// Sets the range of value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<TValue> Range<TValue>([NotNull] this ModelMetadataItemBuilder<TValue> self, TValue minimum, TValue maximum, string errorMessage)
            where TValue : IComparable
        {
            return self.Range(minimum, maximum, () => errorMessage);
        }

        /// <summary>
        /// Sets the range of value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<TValue> Range<TValue>([NotNull] this ModelMetadataItemBuilder<TValue> self, TValue minimum, TValue maximum, Func<string> errorMessage)
            where TValue : IComparable
        {
            return self.Range(minimum, maximum, errorMessage, null, null);
        }

        /// <summary>
        /// Sets the range of value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<TValue> Range<TValue>([NotNull] this ModelMetadataItemBuilder<TValue> self, TValue minimum, TValue maximum, Type errorMessageResourceType, string errorMessageResourceName)
            where TValue : IComparable
        {
            return self.Range(minimum, maximum, null, errorMessageResourceType, errorMessageResourceName);
        }

        /// <summary>
        /// Sets the range of value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<TValue?> Range<TValue>([NotNull] this ModelMetadataItemBuilder<TValue?> self, TValue minimum, TValue maximum)
            where TValue : struct, IComparable 
        {
            return self.Range(minimum, maximum, null, null, null);
        }

        /// <summary>
        /// Sets the range of value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<TValue?> Range<TValue>([NotNull] this ModelMetadataItemBuilder<TValue?> self, TValue minimum, TValue maximum, string errorMessage)
            where TValue : struct, IComparable
        {
            return self.Range(minimum, maximum, () => errorMessage);
        }

        /// <summary>
        /// Sets the range of value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<TValue?> Range<TValue>([NotNull] this ModelMetadataItemBuilder<TValue?> self, TValue minimum, TValue maximum, Func<string> errorMessage)
            where TValue : struct, IComparable
        {
            return self.Range(minimum, maximum, errorMessage, null, null);
        }

        /// <summary>
        /// Sets the range of value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<TValue?> Range<TValue>([NotNull] this ModelMetadataItemBuilder<TValue?> self, TValue minimum, TValue maximum, Type errorMessageResourceType, string errorMessageResourceName)
            where TValue : struct, IComparable
        {
            return self.Range(minimum, maximum, null, errorMessageResourceType, errorMessageResourceName);
        }

        /// <summary>
        /// Sets the range of value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        [NotNull]
        private static ModelMetadataItemBuilder<TValue> Range<TValue>([NotNull] this ModelMetadataItemBuilder<TValue> self, TValue minimum, TValue maximum, Func<string> errorMessage, Type errorMessageResourceType, string errorMessageResourceName)
            where TValue : IComparable
        {
            self.AddAction(
                m =>
                {
                    var rangeValidation = m.GetValidationOrCreateNew<RangeValidationMetadata<TValue>>();

                    rangeValidation.Minimum = minimum;
                    rangeValidation.Maximum = maximum;
                    rangeValidation.ErrorMessage = errorMessage;
                    rangeValidation.ErrorMessageResourceType = errorMessageResourceType;
                    rangeValidation.ErrorMessageResourceName = errorMessageResourceName;
                });
 
            return self;
        }

        /// <summary>
        /// Sets the range of value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        [NotNull]
        private static ModelMetadataItemBuilder<TValue?> Range<TValue>([NotNull] this ModelMetadataItemBuilder<TValue?> self, TValue minimum, TValue maximum, Func<string> errorMessage, Type errorMessageResourceType, string errorMessageResourceName)
            where TValue : struct, IComparable
        {
            self.AddAction(
                m =>
                {
                    var rangeValidation = m.GetValidationOrCreateNew<RangeValidationMetadata<TValue>>();

                    rangeValidation.Minimum = minimum;
                    rangeValidation.Maximum = maximum;
                    rangeValidation.ErrorMessage = errorMessage;
                    rangeValidation.ErrorMessageResourceType = errorMessageResourceType;
                    rangeValidation.ErrorMessageResourceName = errorMessageResourceName;
                });

            return self;
        }
    }
}