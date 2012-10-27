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
        protected const string PromptSuffix = "_Prompt";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="containerType"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultResourceType"></param>
        public void Transform(DisplayAttribute attr, Type containerType, string propertyName, Type defaultResourceType)
        {
            Invariant.IsNotNull(attr, "displayAttribute");

            var resourceType = attr.ResourceType ?? defaultResourceType;
            if (resourceType == null)
            {
                return;
            }

            // reset resource and manually set values
            attr.ResourceType = null;
            var resourceKey = GetResourceKey(containerType, propertyName);

            // retrieve values
            attr.Name = GetValue(resourceType, attr.Name, resourceKey, propertyName);
            attr.Description = GetValue(resourceType, attr.Description, resourceKey + DescriptionSuffix, propertyName + DescriptionSuffix);
            attr.ShortName = GetValue(resourceType, attr.ShortName, resourceKey + ShortDisplayNameSuffix, propertyName + ShortDisplayNameSuffix);
            attr.Prompt = GetValue(resourceType, attr.Prompt, resourceKey + PromptSuffix, propertyName + PromptSuffix);
            attr.GroupName = GetValue(resourceType, attr.GroupName, null, null); // leave original behaviour
        }

        private static string GetValue(Type resourceType, string propertyValue, string key, string propertyName)
        {
            if (propertyValue != null)
            {
                // try to mathc value in resources. If it's not found, return user-defined name
                return resourceType.GetResourceValueByPropertyLookup(propertyValue) ?? propertyValue;
            }

            // match by className_propertyName, and then by propertyName only
            return resourceType.GetResourceValueByPropertyLookup(key) ?? resourceType.GetResourceValueByPropertyLookup(propertyName);
        }
    }
}
