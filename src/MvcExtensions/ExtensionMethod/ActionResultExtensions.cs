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

    /// <summary>
    /// Defines a static class which contains extension methods for flash messages.
    /// </summary>
    public static class ActionResultExtensions
    {
        /// <summary>
        /// Withes the flash.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static WrappedActionResultWithFlash<RedirectResult> WithFlash(this RedirectResult instance, object arguments)
        {
            return Flash(instance, ToDictionary(arguments));
        }

        /// <summary>
        /// Withes the flash.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static WrappedActionResultWithFlash<RedirectResult> WithFlash(this RedirectResult instance, IDictionary<string, string> arguments)
        {
            return Flash(instance, arguments);
        }

        /// <summary>
        /// Withes the flash.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static WrappedActionResultWithFlash<RedirectToRouteResult> WithFlash(this RedirectToRouteResult instance, object arguments)
        {
            return Flash(instance, ToDictionary(arguments));
        }

        /// <summary>
        /// Withes the flash.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static WrappedActionResultWithFlash<RedirectToRouteResult> WithFlash(this RedirectToRouteResult instance, IDictionary<string, string> arguments)
        {
            return Flash(instance, arguments);
        }

        /// <summary>
        /// Withes the flash.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static WrappedActionResultWithFlash<ViewResult> WithFlash(this ViewResult instance, object arguments)
        {
            return Flash(instance, ToDictionary(arguments));
        }

        /// <summary>
        /// Withes the flash.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static WrappedActionResultWithFlash<ViewResult> WithFlash(this ViewResult instance, IDictionary<string, string> arguments)
        {
            return Flash(instance, arguments);
        }

        private static WrappedActionResultWithFlash<TActionResult> Flash<TActionResult>(TActionResult instance, IDictionary<string, string> arguments) where TActionResult : ActionResult
        {
            return new WrappedActionResultWithFlash<TActionResult>(instance, arguments);
        }

        private static IDictionary<string, string> ToDictionary(object arguments)
        {
            if (arguments == null)
            {
                return new Dictionary<string, string>();
            }

            return arguments.GetType()
                            .GetProperties()
                            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                            .ToDictionary(p => p.Name, p => p.GetValue(arguments, null).ToString());
        }
    }
}