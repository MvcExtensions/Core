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

    /// <summary>
    /// Defines an interface which is used to  indicates the implemented task can ignore types.
    /// </summary>
    public abstract class IgnorableTypesBootstrapperTask<TBootstrapperTask, TIgnoreType> : BootstrapperTask where TBootstrapperTask : IgnorableTypesBootstrapperTask<TBootstrapperTask, TIgnoreType> where TIgnoreType : class
    {
        private readonly ICollection<Type> ignoredTypes = new List<Type>();

        /// <summary>
        /// Gets the ignored types.
        /// </summary>
        /// <value>The ignored types.</value>
        protected virtual ICollection<Type> IgnoredTypes
        {
            get { return ignoredTypes; }
        }

        /// <summary>
        /// Ignores this instance.
        /// </summary>
        /// <typeparam name="TType">The type of the ignore type.</typeparam>
        /// <returns></returns>
        public TBootstrapperTask Ignore<TType>() where TType : TIgnoreType
        {
            IgnoredTypes.Add(typeof(TType));

            return this as TBootstrapperTask;
        }
    }
}