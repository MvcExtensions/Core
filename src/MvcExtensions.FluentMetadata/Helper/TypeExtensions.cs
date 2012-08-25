#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Extensions for <see cref="Type"/>
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// Get specified attribute
        /// </summary>
        /// <param name="attributeProvider"></param>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public static TAttribute FirstOrDefault<TAttribute>(this ICustomAttributeProvider attributeProvider)
            where TAttribute : Attribute
        {
            var attributes = attributeProvider.GetCustomAttributes(typeof(TAttribute), true);
            return attributes.Length > 0 ? (TAttribute)attributes[0] : null;
        }

        /// <summary>
        /// Get attribute on type or assembly
        /// </summary>
        /// <param name="type"></param>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public static TAttribute GetAttributeOnTypeOrAssembly<TAttribute>(this Type type) where TAttribute : Attribute
        {
            if (type == null)
            {
                return null;
            }

            return type.FirstOrDefault<TAttribute>() ?? type.Assembly.FirstOrDefault<TAttribute>();
        }

        /// <summary>
        /// Check if property exists
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool HasProperty(this Type type, string propertyName)
        {
            if (type == null || string.IsNullOrEmpty(propertyName))
            {
                return false;
            }

            return type.GetProperty(propertyName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public) != null;
        }

        /// <summary>
        /// Get resource value by property lookup
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string GetResourceValueByPropertyLookup(this Type resourceType, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            PropertyInfo property = resourceType.GetProperty(key, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (property != null)
            {
                MethodInfo getMethod = property.GetGetMethod(true);
                if (getMethod == null || !getMethod.IsAssembly && !getMethod.IsPublic)
                {
                    property = null;
                }

            }

            if (property == null)
            {
                return null;
            }

            if (property.PropertyType != typeof(string))
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        ExceptionMessages.ResourcePropertyNotStringType,
                        new object[] { property.Name, resourceType.FullName }));
            }

            return (string)property.GetValue(null, null);
        }
    }
}
