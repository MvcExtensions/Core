#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>, AlexBar <abarbashin@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    /// <summary>
    /// Holds ModelMetadataItemBuilder&lt;TValue&gt; class
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface IRemoteValidationConfigurator<TValue>
    {
        /// <summary>
        /// ModelMetadataItemBuilder reference 
        /// </summary>
        ModelMetadataItemBuilder<TValue> ModelMetadataItemBuilder
        {
            get;
            set;
        }
    }
}