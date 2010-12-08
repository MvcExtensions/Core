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
    /// Defines a base class which is used to hold the general type mapping.
    /// </summary>
    /// <typeparam name="TType1">The type of the type1.</typeparam>
    /// <typeparam name="TType2">The type of the type2.</typeparam>
    public class TypeMappingRegistry<TType1, TType2> where TType2 : class
    {
        private readonly IDictionary<Type, Type> mappings = new Dictionary<Type, Type>();

        /// <summary>
        /// Gets the mappings.
        /// </summary>
        /// <value>The mappings.</value>
        protected virtual IDictionary<Type, Type> Mappings
        {
            get { return mappings; }
        }

        /// <summary>
        /// Registers the specified type1.
        /// </summary>
        /// <param name="type1">The type1.</param>
        /// <param name="type2">The type2.</param>
        public virtual void Register(Type type1, Type type2)
        {
            Invariant.IsNotNull(type1, "type1");
            Invariant.IsNotNull(type2, "type2");

            EnsureType(typeof(TType1), type1, "type1");
            EnsureType(typeof(TType2), type2, "type2");

            Mappings.Add(type1, type2);
        }

        /// <summary>
        /// Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// <c>true</c> if the specified type is registered; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsRegistered(Type type)
        {
            return Mappings.ContainsKey(type);
        }

        /// <summary>
        /// Returns the matched type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public virtual Type Matching(Type type)
        {
            return IsRegistered(type) ? Mappings[type] : null;
        }

        private static void EnsureType(Type parent, Type child, string parameterName)
        {
            if (!parent.IsAssignableFrom(child))
            {
                throw new ArgumentException(string.Format(Culture.CurrentUI, ExceptionMessages.IncorrectTypeMustBeDescended, parent.FullName), parameterName);
            }
        }
    }
}