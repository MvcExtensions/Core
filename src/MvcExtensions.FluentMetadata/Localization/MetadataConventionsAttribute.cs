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
    /// Allows to overwrite global resource type for metadata. Can be applied to ViewModel class or to whole assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
    public class MetadataConventionsAttribute : Attribute
    {
        /// <summary>
        /// Creates <see cref="MetadataConventionsAttribute"/> attribute.
        /// </summary>
        public MetadataConventionsAttribute()
        {
        }

        /// <summary>
        /// Allows to ovewrite global resource type for metadata. Can be applied to ViewModel class or to whole assembly.
        /// </summary>
        /// <param name="resourceType"></param>
        public MetadataConventionsAttribute(Type resourceType)
        {
            ResourceType = resourceType;
        }

        /// <summary>
        /// Resource type to use for metadata class
        /// </summary>
        public Type ResourceType { get; set; }
    }
}
