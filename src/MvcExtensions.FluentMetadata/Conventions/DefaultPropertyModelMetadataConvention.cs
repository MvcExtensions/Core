#region Copyright
// Copyright (c) 2009 - 2011, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    /// Default inplementation of <see cref="IPropertyModelMetadataConvention"/> class. 
    /// Contains common logic to create convention for metadata.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DefaultPropertyModelMetadataConvention<T> : IPropertyModelMetadataConvention
    {
        /// <summary>
        /// Verifies that conventions can be applied to the given property
        /// </summary>
        /// <param name="propertyInfo">Target property information</param>
        /// <returns>true - if metadata can be accepted; otherwise, false</returns>
        public virtual bool IsApplicable([NotNull] PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType == typeof(T);
        }

        /// <summary>
        /// Creates a set of model metadata rules
        /// </summary>
        /// <param name="property">Target property information</param>
        /// <param name="item"></param>
        /// <returns>A instance of <see cref="ModelMetadataItem"/></returns>
        public virtual ModelMetadataItem Apply(PropertyInfo property, ModelMetadataItem item)
        {
            var builder = new ModelMetadataItemBuilder<T>(item);
            Apply(property, builder);
            return builder.Item;
        }

        /// <summary>
        /// Creates a set of model metadata rules
        /// </summary>
        /// <param name="property">The property</param>
        /// <param name="builder">The model metadata item builder</param>
        protected virtual void Apply(PropertyInfo property, ModelMetadataItemBuilder<T> builder)
        {
        }
    }
}
