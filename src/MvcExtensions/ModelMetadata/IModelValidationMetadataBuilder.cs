#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    /// <summary>
    /// Represents an interface to build an <see cref="IModelValidationMetadata" />
    /// </summary>
    public interface IModelValidationMetadataBuilder
    {
        /// <summary>
        /// Builds an <see cref="IModelValidationMetadata" />
        /// </summary>
        /// <returns> </returns>
        IModelValidationMetadata Build();
    }
}