#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Web.Mvc;
    using JetBrains.Annotations;

    /// <summary>
    /// Transforms <see cref="ModelMetadata"/> to apply convensions
    /// </summary>
    public static class FluentModelMetadataTransformer
    {
        private const string DescriptionSuffix = "_Description";
        private const string ShortDisplayNameSuffix = "_ShortName";
        private const string PromptSuffix = "_Prompt";
        
        /// <summary>
        /// Tranform <see cref="ModelMetadata"/>
        /// </summary>
        /// <param name="metadata"></param>
        public static void Transform([NotNull] ModelMetadata metadata)
        {
            Invariant.IsNotNull(metadata, "metadata");

            var containerType = metadata.ContainerType;
            if (!LocalizationConventions.Enabled || containerType == null || string.IsNullOrEmpty(metadata.PropertyName))
            {
                return;
            }

            // flent configuration does not have ResourceType, so get it from type
            var resourceType = LocalizationConventions.GetDefaultResourceType(containerType);
            var propertyName = metadata.PropertyName;
            if (resourceType != null && !string.IsNullOrEmpty(propertyName))
            {
                var key = ResourceUtil.GetResourceKey(containerType, propertyName);
                if (metadata.DisplayName == null)
                {
                    metadata.DisplayName = RetrieveValue(resourceType, key, propertyName);
                }

                if (metadata.ShortDisplayName == null)
                {
                    metadata.ShortDisplayName = RetrieveValue(resourceType, key + ShortDisplayNameSuffix, propertyName + ShortDisplayNameSuffix);
                }

                if (metadata.Watermark == null)
                {
                    metadata.Watermark = RetrieveValue(resourceType, key + PromptSuffix, propertyName + PromptSuffix);
                }

                if (metadata.Description == null)
                {
                    metadata.Description = RetrieveValue(resourceType, key + DescriptionSuffix, propertyName + DescriptionSuffix);
                }
            }
        }

        private static string RetrieveValue([NotNull] Type resourceType, string key, string propertyName)
        {
            return resourceType.GetResourceValueByPropertyLookup(key) ?? resourceType.GetResourceValueByPropertyLookup(propertyName);
        }
    }
}
