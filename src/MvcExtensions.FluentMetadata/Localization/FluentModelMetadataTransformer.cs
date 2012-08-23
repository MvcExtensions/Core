#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Resources;
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
            if (resourceType != null)
            {
                var rm = new ResourceManager(resourceType);
                var propertyName = metadata.PropertyName;

                var key = GetResourceKey(containerType, propertyName);
                if (metadata.DisplayName == null)
                {
                    metadata.DisplayName = RetriveResourceValue(rm, key, propertyName);
                }

                if (metadata.ShortDisplayName == null)
                {
                    metadata.ShortDisplayName = RetriveResourceValue(rm, key + ShortDisplayNameSuffix, propertyName + ShortDisplayNameSuffix);
                }

                if (metadata.Watermark == null)
                {
                    metadata.Watermark = RetriveResourceValue(rm, key + PromptSuffix, propertyName + PromptSuffix);
                }

                if (metadata.Description == null)
                {
                    metadata.Description = RetriveResourceValue(rm, key + DescriptionSuffix, propertyName + DescriptionSuffix);
                }
            }
        }

        private static string RetriveResourceValue(ResourceManager rm, string key, string propertyName)
        {
            return rm.GetString(key) ?? rm.GetString(propertyName);
        }
    }
}
