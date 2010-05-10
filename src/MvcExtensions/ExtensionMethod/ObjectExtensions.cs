#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Defines an static class which contains extension methods of <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Convert the given object into json string.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static string ToJson(this object instance)
        {
            return ToJson(instance, null);
        }

        /// <summary>
        /// Convert the given object into json string.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="jsonConverters">The json converters.</param>
        /// <returns></returns>
        public static string ToJson(this object instance, IEnumerable<JavaScriptConverter> jsonConverters)
        {
            Invariant.IsNotNull(instance, "instance");

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            jsonSerializer.RegisterConverters(jsonConverters ?? new JavaScriptConverter[0]);

            return jsonSerializer.Serialize(instance);
        }
    }
}