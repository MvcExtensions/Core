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
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Xml.Linq;

    /// <summary>
    /// Defines a static class to render flash messages for <see cref="HtmlHelper"/>.
    /// </summary>
    public static class FlashHtmlExtensions
    {
        /// <summary>
        /// Flashes the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="encoded">if set to <c>true</c> [encoded].</param>
        /// <returns></returns>
        public static IHtmlString Flash(this HtmlHelper instance, string tagName = "p", bool encoded = true)
        {
            Invariant.IsNotNull(instance, "instance");

            Func<string, XNode> content = message => encoded ? new XText(message) : XElement.Parse(message) as XNode;

            IList<KeyValuePair<string, string>> messages = new FlashStorage(instance.ViewContext.TempData).Messages.ToList();

            IEnumerable<XElement> elements = messages.Select(pair => new XElement(tagName ?? "p", new XAttribute("class", "flash" + " " + pair.Key), content(pair.Value)));
            string html = string.Join(Environment.NewLine, elements.Select(e => e.ToString()));

            return instance.Raw(html);
        }
    }
}