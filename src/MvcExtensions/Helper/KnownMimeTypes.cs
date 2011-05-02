#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Diagnostics;

    internal static class KnownMimeTypes
    {
        private static readonly string[] html = new[] { "text/html", "application/xhtml+xml", "*/*" /* Default */ };
        private static readonly string[] xml = new[] { "application/xml", "text/xml" };
        private static readonly string[] json = new[] { "application/json", "text/json" };

        [DebuggerStepThrough]
        public static string[] HtmlTypes()
        {
            return html;
        }

        [DebuggerStepThrough]
        public static string[] XmlTypes()
        {
            return xml;
        }

        [DebuggerStepThrough]
        public static string[] JsonTypes()
        {
            return json;
        }
    }
}