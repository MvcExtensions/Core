#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a base class that is used to configure metadata of a model fluently.
    /// </summary>
    public abstract class ModelMetadataConfiguration<TModel> : IModelMetadataConfiguration, IFluentSyntax where TModel : class
    {
        private readonly IDictionary<string, ModelMetadataItem> configurations = new Dictionary<string, ModelMetadataItem>(StringComparer.OrdinalIgnoreCase);
        private readonly Type modelType = typeof(TModel);

        #region IModelMetadataConfiguration Members
        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <value>The type of the model.</value>
        public Type ModelType
        {
            [DebuggerStepThrough, EditorBrowsable(EditorBrowsableState.Never)]
            get { return modelType; }
        }

        /// <summary>
        /// Gets the configurations.
        /// </summary>
        /// <value>The configurations.</value>
        public virtual IDictionary<string, ModelMetadataItem> Configurations
        {
            [DebuggerStepThrough, EditorBrowsable(EditorBrowsableState.Never)]
            get { return configurations; }
        }
        #endregion

        /// <summary>
        /// Configures the specified value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected ModelMetadataItemBuilder<TValue> Configure<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            return new ModelMetadataItemBuilder<TValue>(Append(expression));
        }

        /// <summary>
        /// Configures the specified value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="property">The expression.</param>
        /// <returns></returns>
        protected ModelMetadataItemBuilder<TValue> Configure<TValue>(string property)
        {
            return new ModelMetadataItemBuilder<TValue>(Append(property));
        }

        /// <summary>
        /// Configures the specified value.
        /// </summary>
        /// <param name="property">The expression.</param>
        /// <returns></returns>
        protected ModelMetadataItemBuilder<object> Configure(string property)
        {
            return new ModelMetadataItemBuilder<object>(Append(property));
        }

        /// <summary>
        /// Appends the specified configuration.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected virtual ModelMetadataItem Append<TType>(Expression<Func<TModel, TType>> expression)
        {
            Invariant.IsNotNull(expression, "expression");

            return Append(ExpressionHelper.GetExpressionText(expression));
        }

        private ModelMetadataItem Append(string property)
        {
            Invariant.IsNotNull(property, "property");

            var item = new ModelMetadataItem();

            configurations[property] = item;

            return item;
        }
    }
}