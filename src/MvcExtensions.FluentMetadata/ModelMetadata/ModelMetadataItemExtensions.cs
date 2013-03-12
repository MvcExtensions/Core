#region Copyright
// Copyright (c) 2009 - 2011, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines an static class which contains extension methods of <see cref="ModelMetadataItem"/>.
    /// </summary>
    [TypeForwardedFrom(KnownAssembly.MvcExtensions)]
    public static class ModelMetadataItemExtensions
    {
        /// <summary>
        /// Returns model validation metadata of type <typeparamref name="TValidationMetadata"/> associated with this <paramref name="item"/>. 
        /// New validation will be created if no one is found. 
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="TValidationMetadata"></typeparam>
        /// <returns>Model validation metadata of type <typeparamref name="TValidationMetadata"/></returns>
        [NotNull]
        public static TValidationMetadata GetValidationOrCreateNew<TValidationMetadata>([NotNull] this ModelMetadataItem item)
            where TValidationMetadata : class, IModelValidationMetadata, new()
        {
            var validation = item.GetValidation<TValidationMetadata>();

            if (validation == null)
            {
                validation = new TValidationMetadata();
                item.Validations.Add(validation);
            }

            return validation;
        }

        /// <summary>
        /// Returns model validation metadata of type <typeparamref name="TValidationMetadata"/> associated with this <paramref name="item"/> or null.
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="TValidationMetadata"></typeparam>
        /// <returns>Model validation metadata of type <typeparamref name="TValidationMetadata"/> or null</returns>
        [CanBeNull]
        public static TValidationMetadata GetValidation<TValidationMetadata>([NotNull] this ModelMetadataItem item)
            where TValidationMetadata : IModelValidationMetadata
        {
            return item.Validations.OfType<TValidationMetadata>().SingleOrDefault();
        }

        /// <summary>
        /// Returns model metadata additional setting of type <typeparamref name="TSetting"/> associated with this <paramref name="item"/>. 
        /// New model setting will be created if no one is found. 
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="TSetting"></typeparam>
        /// <returns>Model validation metadata of type <typeparamref name="TSetting"/></returns>
        [NotNull]
        public static TSetting GetAdditionalSettingOrCreateNew<TSetting>([NotNull] this ModelMetadataItem item)
            where TSetting : class, IModelMetadataAdditionalSetting, new()
        {
            var setting = item.GetAdditionalSetting<TSetting>();

            if (setting == null)
            {
                setting = new TSetting();
                item.AdditionalSettings.Add(setting);
            }

            return setting;
        }

        /// <summary>
        /// Returns model metadata additional setting of type <typeparamref name="TSetting"/> associated with this <paramref name="item"/> or null. 
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="TSetting"></typeparam>
        /// <returns>Model validation metadata of type <typeparamref name="TSetting"/> or null</returns>
        [CanBeNull]
        public static TSetting GetAdditionalSetting<TSetting>([NotNull] this ModelMetadataItem item)
            where TSetting : IModelMetadataAdditionalSetting
        {
            return item.AdditionalSettings.OfType<TSetting>().FirstOrDefault();
        }
    }
}