#region Copyright
// Copyright (c) 2009 - 2011, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Reflection;

    /// <summary>
    /// Interface for all convenstions
    /// </summary>
    public interface IPropertyMetadataConvention
    {
        /// <summary>
        /// Verifies that conventions can be applied to the given property
        /// </summary>
        bool CanBeAccepted(PropertyInfo propertyInfo);

        /// <summary>
        /// Creates a set of model metadata rules
        /// </summary>
        /// <returns>A instance of <see cref="ModelMetadataItem"/></returns>
        ModelMetadataItem CreateMetadataRules();
    }
}
