#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Defines an static class which contains extension methods of <see cref="ViewDataDictionary"/>.
    /// </summary>
    public static class ViewDataDictionaryExtensions
    {
        /// <summary>
        /// Gets the value against the specified type name.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static TValue Get<TValue>(this ViewDataDictionary instance)
        {
            return Get<TValue>(instance, MakeKey<TValue>());
        }

        /// <summary>
        /// Gets the value that is stored against the specified key.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static TValue Get<TValue>(this ViewDataDictionary instance, string key)
        {
            return Get(instance, key, default(TValue));
        }

        /// <summary>
        /// Gets the value that is stored against the specified key, if the key does not exists it will return the provided default value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static TValue Get<TValue>(this ViewDataDictionary instance, string key, TValue defaultValue)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.ContainsKey(key) ? (TValue)instance[key] : defaultValue;
        }

        /// <summary>
        /// Sets the value against the specified type.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        public static void Set<TValue>(this ViewDataDictionary instance, TValue value)
        {
            Set(instance, MakeKey<TValue>(), value);
        }

        /// <summary>
        /// Sets the value against the specified key.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void Set<TValue>(this ViewDataDictionary instance, string key, TValue value)
        {
            Invariant.IsNotNull(instance, "instance");

            instance[key] = value;
        }

        /// <summary>
        /// Determines whether  the specified type exists.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// <c>true</c> if [contains] [the specified instance]; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains<TValue>(this ViewDataDictionary instance)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.ContainsKey(MakeKey<TValue>());
        }

        /// <summary>
        /// Removes the specified type.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="instance">The instance.</param>
        public static bool Remove<TValue>(this ViewDataDictionary instance)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.Remove(MakeKey<TValue>());
        }

        /// <summary>
        /// Convert the view data into json string.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static string ToJson(this ViewDataDictionary instance)
        {
            return ToJson(instance, null);
        }

        /// <summary>
        /// Convert the view data into json string.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="jsonConverters">The json converters.</param>
        /// <returns></returns>
        public static string ToJson(this ViewDataDictionary instance, IEnumerable<JavaScriptConverter> jsonConverters)
        {
            return AsSerializable(instance).ToJson(jsonConverters);
        }

        /// <summary>
        /// Convert the view data into a serializable object.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static object AsSerializable(this ViewDataDictionary instance)
        {
            Invariant.IsNotNull(instance, "instance");

            var viewData = instance.Select(pair => new { key = pair.Key, value = pair.Value })
                                   .ToList();

            var modelStates = instance.ModelState.IsValid ?
                              null :
                              instance.ModelState.Select(ms => new { key = ms.Key, errors = ms.Value.Errors.Select(error => (error.Exception == null) ? error.ErrorMessage : error.Exception.Message).Where(error => !string.IsNullOrEmpty(error)) })
                                                 .Where(ms => ms.errors.Any()) // No need to include model state that does not have any errors
                                                 .ToList();

            var result = new { viewData = viewData.Any() ? viewData : null, model = instance.Model, modelStates = ((modelStates != null) && modelStates.Any()) ? modelStates : null };

            return result;
        }

        private static string MakeKey<TValue>()
        {
            return typeof(TValue).FullName;
        }
    }
}