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
    public interface IPropertyModelMetadataConvention
    {
        /// <summary>
        /// Verifies that conventions can be applied to the given property
        /// </summary>
        /// <param name="propertyInfo">Target property information</param>
        /// <returns>true - if metadata can be accepted; otherwise, false</returns>
        bool IsApplicable(PropertyInfo propertyInfo);

        /// <summary>
        /// Creates a set of model metadata rules
        /// </summary>
        /// <param name="property">Target property information</param>
        /// <param name="item"></param>
        /// <returns>A instance of <see cref="ModelMetadataItem"/></returns>
        ModelMetadataItem Apply(PropertyInfo property, ModelMetadataItem item);
    }
}
