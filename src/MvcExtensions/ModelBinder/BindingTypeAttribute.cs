#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Diagnostics;
    using System.Web.Mvc;

    /// <summary>
    /// Defines an attribute  which is used to store the type information for which the <seealso cref="IModelBinder"/> is registered.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class BindingTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingTypeAttribute"/> class.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        [DebuggerStepThrough]
        public BindingTypeAttribute(Type modelType) : this(modelType, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingTypeAttribute"/> class.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="inherited">if set to <c>true</c> [inherited].</param>
        [DebuggerStepThrough]
        public BindingTypeAttribute(Type modelType, bool inherited)
        {
            Invariant.IsNotNull(modelType, "modelType");

            if (!inherited && (!(modelType.IsClass && !modelType.IsAbstract)))
            {
                throw new ArgumentException(string.Format(Culture.Current, ExceptionMessages.MustBeAValidClass, modelType.FullName), "modelType");
            }

            ModelType = modelType;
            Inherited = inherited;
        }

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <value>The type of the model.</value>
        public Type ModelType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether to include the inherited types of the <see cref="ModelType"/>.
        /// </summary>
        /// <value><c>true</c> if inherited; otherwise, <c>false</c>.</value>
        public bool Inherited
        {
            get;
            private set;
        }
    }
}