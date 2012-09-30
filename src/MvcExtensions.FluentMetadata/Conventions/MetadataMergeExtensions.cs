#region Copyright
// Copyright (c) 2009 - 2011, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Merges <see cref="ModelMetadataItem"/> classes
    /// </summary>
    internal static class MetadataMergeExtensions
    {
        /// <summary>
        /// Merge metadata items
        /// </summary>
        public static void MergeTo(this ModelMetadataItem metadataFrom, ModelMetadataItem metadataTo)
        {
            SetMetadataProperties(metadataFrom, metadataTo);
            SyncValidations(metadataFrom, metadataTo);
            SyncAdditionalSettings(metadataFrom, metadataTo);
        }

        private static void SetMetadataProperties(ModelMetadataItem metadataFrom, ModelMetadataItem metadataTo)
        {
            // by default the value is true, so will not set it to true if original value was set to false
            if (metadataFrom.ShowForDisplay == false)
            {
                metadataTo.ShowForDisplay = metadataFrom.ShowForDisplay;
            }

            // by default the value is false, so will not set it to false if original value was set to true
            if (metadataFrom.ApplyFormatInEditMode)
            {
                metadataTo.ApplyFormatInEditMode = metadataFrom.ApplyFormatInEditMode;
            }

            if (metadataTo.DisplayName == null && metadataFrom.DisplayName != null)
            {
                metadataTo.DisplayName = metadataFrom.DisplayName;
            }

            if (metadataTo.ShortDisplayName == null && metadataFrom.ShortDisplayName != null)
            {
                metadataTo.ShortDisplayName = metadataFrom.ShortDisplayName;
            }

            if (string.IsNullOrEmpty(metadataTo.TemplateName) && !string.IsNullOrEmpty(metadataFrom.TemplateName))
            {
                metadataTo.TemplateName = metadataFrom.TemplateName;
            }

            if (metadataTo.Description == null && metadataFrom.Description != null)
            {
                metadataTo.Description = metadataFrom.Description;
            }

            if (metadataTo.NullDisplayText == null && metadataFrom.NullDisplayText != null)
            {
                metadataTo.NullDisplayText = metadataFrom.NullDisplayText;
            }

            if (metadataTo.Watermark == null && metadataFrom.Watermark != null)
            {
                metadataTo.Watermark = metadataFrom.Watermark;
            }

            if (metadataTo.HideSurroundingHtml == null && metadataFrom.HideSurroundingHtml.HasValue)
            {
                metadataTo.HideSurroundingHtml = metadataFrom.HideSurroundingHtml.Value;
            }

            if (metadataTo.RequestValidationEnabled == null && metadataFrom.RequestValidationEnabled.HasValue)
            {
                metadataTo.RequestValidationEnabled = metadataFrom.RequestValidationEnabled.Value;
            }

            if (metadataTo.IsReadOnly == null && metadataFrom.IsReadOnly.HasValue)
            {
                metadataTo.IsReadOnly = metadataFrom.IsReadOnly.Value;
            }

            if (metadataTo.ShowForEdit == null && metadataFrom.ShowForEdit.HasValue)
            {
                metadataTo.ShowForEdit = metadataFrom.ShowForEdit.Value;
            }

            if (metadataTo.EditFormat == null && metadataFrom.EditFormat != null)
            {
                metadataTo.EditFormat = metadataFrom.EditFormat;
            }

            if (metadataTo.Order == null && metadataFrom.Order.HasValue)
            {
                metadataTo.Order = metadataFrom.Order;
            }

            if (metadataTo.DisplayFormat == null && metadataFrom.DisplayFormat != null)
            {
                metadataTo.DisplayFormat = metadataFrom.DisplayFormat;
            }

            if (metadataTo.ConvertEmptyStringToNull == null && metadataFrom.ConvertEmptyStringToNull.HasValue)
            {
                metadataTo.ConvertEmptyStringToNull = metadataFrom.ConvertEmptyStringToNull;
            }
        }

        private static void SyncValidations(ModelMetadataItem metadataFrom, ModelMetadataItem metadataTo)
        {
            if (metadataFrom.Validations.Count > 0)
            {
                var types = new HashSet<Type>();
                foreach (var metadata in metadataTo.Validations)
                {
                    var type = metadata.GetType();
                    if (!types.Contains(type))
                    {
                        types.Add(type);
                    }
                }

                //TODO: how about Deleged validation? just rewrite it completely?

                foreach (var validation in metadataFrom.Validations)
                {
                    if (!types.Contains(validation.GetType()))
                    {
                        metadataTo.Validations.Add(validation);
                    }
                }
            }

            if (metadataTo.IsRequired == null && metadataFrom.IsRequired.HasValue && metadataFrom.IsRequired.Value)
            {
                metadataTo.IsRequired = metadataFrom.IsRequired.Value;
            }

            // ensure that Required attribute is removed
            if (metadataFrom.IsRequired.HasValue && !metadataFrom.IsRequired.Value)
            {
                var requiredValidation = metadataTo.GetValidation<RequiredValidationMetadata>();
                if (requiredValidation != null)
                {
                    metadataTo.Validations.Remove(requiredValidation);
                }
                metadataTo.IsRequired = false;
            }
        }

        private static void SyncAdditionalSettings(ModelMetadataItem metadataFrom, ModelMetadataItem metadataTo)
        {
            if (metadataFrom.AdditionalSettings.Count > 0)
            {
                var additionalSettingsTypes = new HashSet<Type>();
                foreach (var setting in metadataTo.AdditionalSettings)
                {
                    var type = setting.GetType();
                    if (!additionalSettingsTypes.Contains(type))
                    {
                        additionalSettingsTypes.Add(type);
                    }
                }

                foreach (var setting in metadataFrom.AdditionalSettings)
                {
                    if (!additionalSettingsTypes.Contains(setting.GetType()))
                    {
                        metadataTo.AdditionalSettings.Add(setting);
                    }
                }
            }
        }
    }
}
