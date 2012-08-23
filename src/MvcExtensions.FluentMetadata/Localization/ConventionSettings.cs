#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;

    /// <summary>
    /// Holds settings for applying convensions for metadata
    /// </summary>
    public static class ConventionSettings
    {
        private static Type defaultResourceType;
        
        /// <summary>
        /// Default resource type to use when appling convensions
        /// </summary>
        public static Type DefaultResourceType
        {
            get
            {
                return defaultResourceType;
            }

            set
            {
                defaultResourceType = value;

                // if user sets attribute, enable convensions by default
                if (value != null)
                {
                    ConventionsActive = true;
                }
            }
        }

        /// <summary>
        /// MetadataConventionsEnabled
        /// </summary>
        public static bool ConventionsActive { get; set; }

        /// <summary>
        /// If true, will require attribute per type or containing assembly. 
        /// </summary>
        public static bool RequireConventionAttribute { get; set; }
        
        /// <summary>
        /// Get default resource type
        /// </summary>
        internal static Type GetDefaultResourceType(Type containerType)
        {
            if (!ConventionsActive)
            {
                return null;
            }

            Type resourceType = null;
            var attribute = containerType.GetAttributeOnTypeOrAssembly<MetadataConventionsAttribute>();
            if (attribute == null && RequireConventionAttribute)
            {
                return null;
            }

            if (attribute != null && attribute.ResourceType != null)
            {
                resourceType = attribute.ResourceType;
            }

            return resourceType ?? DefaultResourceType;
        }
    }
}
