#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    /// <summary>
    /// Defines a class to fluently configure metadata of a model.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class ObjectMetadataItemBuilder<TModel> : ModelMetadataItemBuilder<ObjectMetadataItem, ObjectMetadataItemBuilder<TModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectMetadataItemBuilder&lt;TModel&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public ObjectMetadataItemBuilder(ObjectMetadataItem item) : base(item)
        {
        }
    }
}