#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Resources;

    /// <summary>
    /// 
    /// </summary>
    public class DisplayAttributeTransformer : TransformerCore
    {
        /// <summary>
        /// 
        /// </summary>
        protected const string DescriptionSuffix = "_Description";

        /// <summary>
        /// 
        /// </summary>
        protected const string ShortDisplayNameSuffix = "_ShortName";

        /// <summary>
        /// 
        /// </summary>
        protected const string GroupNameSuffix = "_GroupName";

        /// <summary>
        /// 
        /// </summary>
        protected const string PromptSuffix = "_Prompt";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayAttribute"></param>
        /// <param name="containerType"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultResourceType"></param>
        public void Transform(DisplayAttribute displayAttribute, Type containerType, string propertyName, Type defaultResourceType)
        {
            Invariant.IsNotNull(displayAttribute, "displayAttribute");

            // ensure resource type
            displayAttribute.ResourceType = displayAttribute.ResourceType ?? defaultResourceType;

            if (displayAttribute.ResourceType == null)
            {
                return;
            }

            // ensure resource name
            var displayAttributeName = GetDisplayAttributeName(containerType, propertyName, displayAttribute);
            if (displayAttributeName != null)
            {
                displayAttribute.Name = displayAttributeName;
            }

            // store resource
            var resourceType = displayAttribute.ResourceType;
            if (!displayAttribute.ResourceType.HasProperty(displayAttribute.Name))
            {
                displayAttribute.ResourceType = null;
            }

            LocalizeDisplayAttributeValues(displayAttribute, resourceType, containerType, propertyName);
        }

        private static string GetDisplayAttributeName(Type containerType, string propertyName, DisplayAttribute displayAttribute)
        {
            if (containerType != null && string.IsNullOrEmpty(displayAttribute.Name))
            {
                // check to see that resource key exists.
                var resourceKey = GetResourceKey(containerType, propertyName);
                var hasResource = HasResourceValue(displayAttribute.ResourceType, resourceKey);
                return hasResource ? resourceKey : propertyName;
            }

            return null;
        }
        
        private static void LocalizeDisplayAttributeValues(DisplayAttribute displayAttribute, Type resourceType, Type containerType, string propertyName)
        {
            var resourceKey = GetResourceKey(containerType, propertyName);
            if (displayAttribute.ResourceType == null)
            {
                var rm = new ResourceManager(resourceType);

                if (displayAttribute.Description == null)
                {
                    displayAttribute.Description = rm.GetString(resourceKey + DescriptionSuffix) ?? rm.GetString(propertyName + DescriptionSuffix);
                }

                if (displayAttribute.ShortName == null)
                {
                    displayAttribute.ShortName = rm.GetString(resourceKey + ShortDisplayNameSuffix) ?? rm.GetString(propertyName + ShortDisplayNameSuffix);
                }

                if (displayAttribute.Prompt == null)
                {
                    displayAttribute.Prompt = rm.GetString(resourceKey + PromptSuffix) ?? rm.GetString(propertyName + PromptSuffix);
                }

                if (displayAttribute.GroupName == null)
                {
                    displayAttribute.GroupName = rm.GetString(resourceKey + GroupNameSuffix) ?? rm.GetString(propertyName + GroupNameSuffix);
                }
            }
            else
            {
                if (displayAttribute.Description == null)
                {
                    var descriptionKey = resourceKey + DescriptionSuffix;
                    if (displayAttribute.ResourceType.HasProperty(descriptionKey))
                    {
                        displayAttribute.Description = descriptionKey;
                    }
                    else if (displayAttribute.ResourceType.HasProperty(propertyName + DescriptionSuffix))
                    {
                        displayAttribute.Description = propertyName + DescriptionSuffix;
                    }
                }

                if (displayAttribute.ShortName == null)
                {
                    var shortNameKey = resourceKey + ShortDisplayNameSuffix;
                    if (displayAttribute.ResourceType.HasProperty(shortNameKey))
                    {
                        displayAttribute.ShortName = shortNameKey;
                    }
                    else if (displayAttribute.ResourceType.HasProperty(propertyName + ShortDisplayNameSuffix))
                    {
                        displayAttribute.ShortName = propertyName + ShortDisplayNameSuffix;
                    }
                }

                if (displayAttribute.Prompt == null)
                {
                    var promptKey = resourceKey + PromptSuffix;
                    if (displayAttribute.ResourceType.HasProperty(promptKey))
                    {
                        displayAttribute.Prompt = promptKey;
                    }
                    else if (displayAttribute.ResourceType.HasProperty(propertyName + PromptSuffix))
                    {
                        displayAttribute.Prompt = propertyName + PromptSuffix;
                    }
                }

                if (displayAttribute.GroupName == null)
                {
                    var groupNameKey = resourceKey + GroupNameSuffix;
                    if (displayAttribute.ResourceType.HasProperty(groupNameKey))
                    {
                        displayAttribute.GroupName = groupNameKey;
                    }
                    else if (displayAttribute.ResourceType.HasProperty(propertyName + GroupNameSuffix))
                    {
                        displayAttribute.GroupName = propertyName + GroupNameSuffix;
                    }
                }
            }
        }
    }
}
