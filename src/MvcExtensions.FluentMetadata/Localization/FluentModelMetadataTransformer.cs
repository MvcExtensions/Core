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

    /// <summary>
    /// Transforms <see cref="ModelMetadata"/> to apply convensions
    /// </summary>
    public class FluentModelMetadataTransformer : TransformerCore
    {
        private const string DescriptionSuffix = "_Description";
        private const string ShortDisplayNameSuffix = "_ShortName";
        private const string PromptSuffix = "_Prompt";
        
        /// <summary>
        /// Tranform <see cref="ModelMetadata"/>
        /// </summary>
        /// <param name="metadata"></param>
        public void Transform(ModelMetadata metadata)
        {
            Invariant.IsNotNull(metadata, "metadata");

            var containerType = metadata.ContainerType;
            if (!ConventionSettings.ConventionsActive || containerType == null || string.IsNullOrEmpty(metadata.PropertyName))
            {
                return;
            }

            // flent configuration does not have ResourceType, so get it from type
            var resourceType = ConventionSettings.GetDefaultResourceType(containerType);
            var propertyName = metadata.PropertyName;
            if (resourceType != null && !string.IsNullOrEmpty(propertyName))
            {
                var key = GetResourceKey(containerType, propertyName);
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

        private static string RetrieveValue(Type resourceType, string key, string propertyName)
        {
            return resourceType.GetResourceValueByPropertyLookup(key) ?? resourceType.GetResourceValueByPropertyLookup(propertyName);
        }


       
    }
}
