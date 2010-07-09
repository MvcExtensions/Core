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

    using Hash = global::IronRuby.Builtins.Hash;

    public partial class IronRubyHtmlHelper
    {
        public MvcForm BeginRouteForm(string routeName)
        {
            return FormExtensions.BeginRouteForm(this, routeName);
        }

        public MvcForm BeginRouteForm(Hash routeValues)
        {
            return this.BeginRouteForm(routeValues.ToRouteValueDictionary());
        }

        public MvcForm BeginRouteForm(string routeName, Hash routeValues)
        {
            return this.BeginRouteForm(routeName, routeValues.ToRouteValueDictionary());
        }

        public MvcForm BeginRouteForm(string routeName, FormMethod method)
        {
            return FormExtensions.BeginRouteForm(this, routeName, method);
        }

        public MvcForm BeginRouteForm(string routeName, Hash routeValues, FormMethod method)
        {
            return this.BeginRouteForm(routeName, routeValues.ToRouteValueDictionary(), method);
        }

        public MvcForm BeginRouteForm(string routeName, FormMethod method, Hash htmlAttributes)
        {
            return this.BeginRouteForm(routeName, method, htmlAttributes.ToRouteValueDictionary());
        }

        public MvcForm BeginRouteForm(string routeName, Hash routeValues, FormMethod method, Hash htmlAttributes)
        {
            return this.BeginRouteForm(routeName, routeValues.ToRouteValueDictionary(), method, htmlAttributes.ToRouteValueDictionary());
        }

        public void EndForm()
        {
            FormExtensions.EndForm(this);
        }
    }
}