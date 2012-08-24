#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Demo.Web
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;

    public abstract class ControllerBase : Controller
    {
        private const string CultureKey = "culture";

        [DebuggerStepThrough]
        protected override void ExecuteCore()
        {
            SetCulture(string.Empty);
            base.ExecuteCore();
        }

        private void SetCulture(string culture)
        {
            if (string.IsNullOrEmpty(culture))
            {
                var hasRouteKey = RouteData.Values.ContainsKey(CultureKey) && !string.IsNullOrWhiteSpace(RouteData.Values[CultureKey].ToString());
                if (hasRouteKey)
                {
                    culture = RouteData.Values[CultureKey].ToString();
                }
                else
                {
                    var cookie = HttpContext.Request.Cookies[CultureKey];
                    if (cookie != null)
                    {
                        culture = cookie.Value;
                    }
                }
            }

            culture = culture ?? "en";
            RouteData.Values[CultureKey] = culture;

            ViewBag.Culture = culture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);

            var httpCookie = new HttpCookie(CultureKey, Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName)
                {
                    Expires = DateTime.UtcNow.AddYears(1)
                };
            HttpContext.Response.SetCookie(httpCookie);
        }
    }
}
