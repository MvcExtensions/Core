#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web;

    /// <summary>
    /// Defines an static class which contains extension methods of <see cref="HttpResponseBase"/>.
    /// </summary>
    public static class HttpResponseBaseExtensions
    {
        /// <summary>
        /// Writes the json.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="json">The json.</param>
        public static void WriteJson(this HttpResponseBase instance, string json)
        {
            Invariant.IsNotNull(instance, "instance");
            Invariant.IsNotNull(json, "json");

            instance.Clear();
            instance.ContentType = "application/json";
            instance.Write(json);
        }
    }
}