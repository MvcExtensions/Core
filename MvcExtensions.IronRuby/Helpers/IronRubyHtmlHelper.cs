#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.IronRuby
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public partial class IronRubyHtmlHelper : HtmlHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Mvc.HtmlHelper"/> class by using the specified view context and view data container.
        /// </summary>
        /// <param name="viewContext">The view context.</param><param name="viewDataContainer">The view data container.</param><exception cref="T:System.ArgumentNullException">The <paramref name="viewContext"/> or the <paramref name="viewDataContainer"/> parameter is null.</exception>
        public IronRubyHtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer) : base(viewContext, viewDataContainer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Mvc.HtmlHelper"/> class by using the specified view context, view data container, and route collection.
        /// </summary>
        /// <param name="viewContext">The view context.</param><param name="viewDataContainer">The view data container.</param><param name="routeCollection">The route collection.</param><exception cref="T:System.ArgumentNullException">One or more parameters is null.</exception>
        public IronRubyHtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer, RouteCollection routeCollection) : base(viewContext, viewDataContainer, routeCollection)
        {
        }
    }
}