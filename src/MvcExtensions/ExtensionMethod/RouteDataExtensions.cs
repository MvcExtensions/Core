#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web.Routing;

    /// <summary>
    /// Defines a class which contains extension method for <see cref="RouteData"/>.
    /// </summary>
    public static class RouteDataExtensions
    {
        /// <summary>
        /// Controllers the name.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static string ControllerName(this RouteData instance)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.GetRequiredString("controller");
        }

        /// <summary>
        /// Actions the name.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static string ActionName(this RouteData instance)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.GetRequiredString("action");
        }
    }
}