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
    /// Result of performing a scan.
    /// </summary>
    public class ConfigurationsScanResult
    {
        /// <summary>
        /// Creates an instance of an ConfigurationsScanResult.
        /// </summary>
        public ConfigurationsScanResult(Type configurationType)
        {
            InterfaceType = typeof(IModelMetadataConfiguration);
            MetadataConfigurationType = configurationType;
        }

        /// <summary>
        /// <see cref="IModelMetadataConfiguration"/> type
        /// </summary>
        public Type InterfaceType { get; private set; }

        /// <summary>
        /// Implementation of <see cref="IModelMetadataConfiguration"/>.
        /// </summary>
        public Type MetadataConfigurationType { get; private set; }
    }
}
