#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Linq;

    internal static class ModelMetadataItemExtensions
    {
        public static TValidationMetadata GetValidationOrCreateNew<TValidationMetadata>(this ModelMetadataItem item)
            where TValidationMetadata : class, IModelValidationMetadata, new()
        {
            TValidationMetadata validation = item.GetValidation<TValidationMetadata>();

            if (validation == null)
            {
                validation = new TValidationMetadata();
                item.Validations.Add(validation);
            }

            return validation;
        }

        public static TValidationMetadata GetValidation<TValidationMetadata>(this ModelMetadataItem item)
            where TValidationMetadata : IModelValidationMetadata
        {
            return item.Validations.OfType<TValidationMetadata>().SingleOrDefault();
        }

        public static TSetting GetAdditionalSettingOrCreateNew<TSetting>(this ModelMetadataItem item)
            where TSetting : class, IModelMetadataAdditionalSetting, new()
        {
            TSetting setting = item.GetAdditionalSetting<TSetting>();

            if (setting == null)
            {
                setting = new TSetting();
                item.AdditionalSettings.Add(setting);
            }

            return setting;
        }

        public static TSetting GetAdditionalSetting<TSetting>(this ModelMetadataItem item)
            where TSetting : IModelMetadataAdditionalSetting
        {
            return item.AdditionalSettings.OfType<TSetting>().FirstOrDefault();
        }
    }
}