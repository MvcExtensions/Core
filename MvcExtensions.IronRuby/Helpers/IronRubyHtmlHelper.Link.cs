#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.IronRuby
{
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;

    using Hash = global::IronRuby.Builtins.Hash;

    public partial class IronRubyHtmlHelper
    {
        public MvcHtmlString ActionLink(string linkText, Hash values)
        {
            return RouteLink(linkText, values);
        }

        public MvcHtmlString ActionLink(string linkText, string actionName)
        {
            return LinkExtensions.ActionLink(this, linkText, actionName);
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, Hash routeValues)
        {
            return this.ActionLink(linkText, actionName, routeValues.ToRouteValueDictionary());
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, Hash routeValues, Hash htmlAttributes)
        {
            return this.ActionLink(linkText, actionName, routeValues.ToRouteValueDictionary(), htmlAttributes.ToRouteValueDictionary());
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, string controllerName)
        {
            return LinkExtensions.ActionLink(this, linkText, actionName, controllerName);
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, string controllerName, Hash routeValues)
        {
            return this.ActionLink(linkText, actionName, controllerName, routeValues.ToRouteValueDictionary(), new RouteValueDictionary());
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, string controllerName, Hash routeValues, Hash htmlAttributes)
        { 
            return this.ActionLink(linkText, actionName, controllerName, routeValues.ToRouteValueDictionary(), htmlAttributes.ToRouteValueDictionary());
        }

        public MvcHtmlString RouteLink(string linkText, Hash routeValues)
        {
            return this.RouteLink(linkText, routeValues.ToRouteValueDictionary());
        }

        public MvcHtmlString RouteLink(string linkText, string routeName)
        {
            return LinkExtensions.RouteLink(this, linkText, routeName);
        }

        public MvcHtmlString RouteLink(string linkText, string routeName, Hash routeValues)
        {
            return this.RouteLink(linkText, routeName, routeValues.ToRouteValueDictionary());
        }

        public MvcHtmlString RouteLink(string linkText, Hash routeValues, Hash htmlAttributes)
        {
            return this.RouteLink(linkText, routeValues.ToRouteValueDictionary(), htmlAttributes.ToRouteValueDictionary());
        }

        public MvcHtmlString RouteLink(string linkText, string routeName, Hash routeValues, Hash htmlAttributes)
        {
            return this.RouteLink(linkText, routeName, routeValues.ToRouteValueDictionary(), htmlAttributes.ToRouteValueDictionary());
        }
    }
}