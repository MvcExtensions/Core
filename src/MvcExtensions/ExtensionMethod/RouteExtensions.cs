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
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Defines a static class to register RESTFul routes
    /// </summary>
    public static class RESTFullRouteExtensions
    {
        const string IdParameterExpression = "{" + RESTFulActionConstraint.IdParameterName + "}";

        private static readonly Type createType = typeof(IRESTFullCreate);
        private static readonly Type updateType = typeof(IRESTFulUpdate<>);
        private static readonly Type destroyType = typeof(IRESTFullDestroy<>);
        private static readonly Type detailsType = typeof(IRESTFullDetails<>);
        private static readonly Type listType = typeof(IRESTFullList);

        /// <summary>
        /// Register the RESTFull routes for the specified controller.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static RouteCollection Resources<TController>(this RouteCollection instance) where TController : Controller
        {
            Type controllerType = typeof(TController);
            string controllerTypeName = controllerType.Name;

            string controllerName = controllerTypeName.Substring(0, controllerTypeName.IndexOf("Controller", StringComparison.OrdinalIgnoreCase));

            bool supportsCreate = SupportsCreate(controllerType);

            if (supportsCreate)
            {
                instance.MapRoute(RouteName(controllerName, KnownActionNames.New), Url(controllerName, KnownActionNames.New), new { controller = controllerName, action = KnownActionNames.New }, new { httpMethod = new RESTFulActionConstraint(HttpVerbs.Get) });
            }

            if (SupportsUpdate(controllerType))
            {
                instance.MapRoute(RouteName(controllerName, KnownActionNames.Edit), Url(controllerName, IdParameterExpression, KnownActionNames.Edit), new { controller = controllerName, action = KnownActionNames.Edit }, new { httpMethod = new RESTFulActionConstraint(HttpVerbs.Get, true) });
                instance.MapRoute(RouteName(controllerName, KnownActionNames.Update), Url(controllerName, IdParameterExpression), new { controller = controllerName, action = KnownActionNames.Update }, new { httpMethod = new RESTFulActionConstraint(HttpVerbs.Put, true) });
            }

            if (SupportsDestroy(controllerType))
            {
                instance.MapRoute(RouteName(controllerName, KnownActionNames.Destroy), Url(controllerName, IdParameterExpression), new { controller = controllerName, action = KnownActionNames.Destroy }, new { httpMethod = new RESTFulActionConstraint(HttpVerbs.Delete, true) });
            }

            if (SupportsDetails(controllerType))
            {
                instance.MapRoute(RouteName(controllerName, KnownActionNames.Show), Url(controllerName, IdParameterExpression), new { controller = controllerName, action = KnownActionNames.Show }, new { httpMethod = new RESTFulActionConstraint(HttpVerbs.Get, true) });
            }

            if (supportsCreate)
            {
                instance.MapRoute(RouteName(controllerName, KnownActionNames.Create), Url(controllerName), new { controller = controllerName, action = KnownActionNames.Create }, new { httpMethod = new RESTFulActionConstraint(HttpVerbs.Post) });
            }

            if (SupportsList(controllerType))
            {
                instance.MapRoute(RouteName(controllerName, KnownActionNames.Index), Url(controllerName), new { controller = controllerName, action = KnownActionNames.Index }, new { httpMethod = new RESTFulActionConstraint(HttpVerbs.Get) });
            }

            return instance;
        }

        /// <summary>
        /// Register the RESTFull area routes for the specified controller.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static AreaRegistrationContext MapResources<TController>(this AreaRegistrationContext instance) where TController : Controller
        {
            Type controllerType = typeof(TController);
            string controllerTypeName = controllerType.Name;

            string controllerName = controllerTypeName.Substring(0, controllerTypeName.IndexOf("Controller", StringComparison.OrdinalIgnoreCase));

            bool supportsCreate = SupportsCreate(controllerType);

            if (supportsCreate)
            {
                instance.MapRoute(RouteName(instance.AreaName, controllerName, KnownActionNames.New), Url(instance.AreaName, controllerName, KnownActionNames.New), new { controller = controllerName, action = KnownActionNames.New }, new { httpMethod = new RESTFulActionConstraint(HttpVerbs.Get) });
            }

            if (SupportsUpdate(controllerType))
            {
                instance.MapRoute(RouteName(instance.AreaName, controllerName, KnownActionNames.Edit), Url(instance.AreaName, controllerName, IdParameterExpression, KnownActionNames.Edit), new { controller = controllerName, action = KnownActionNames.Edit }, new { httpMethod = new RESTFulActionConstraint(HttpVerbs.Get, true) });
                instance.MapRoute(RouteName(instance.AreaName, controllerName, KnownActionNames.Update), Url(instance.AreaName, controllerName, IdParameterExpression), new { controller = controllerName, action = KnownActionNames.Update }, new { httpMethod = new RESTFulActionConstraint(HttpVerbs.Put, true) });
            }

            if (SupportsDestroy(controllerType))
            {
                instance.MapRoute(RouteName(instance.AreaName, controllerName, KnownActionNames.Destroy), Url(instance.AreaName, controllerName, IdParameterExpression), new { controller = controllerName, action = KnownActionNames.Destroy }, new { httpMethod = new RESTFulActionConstraint(HttpVerbs.Delete, true) });
            }

            if (SupportsDetails(controllerType))
            {
                instance.MapRoute(RouteName(instance.AreaName, controllerName, KnownActionNames.Show), Url(instance.AreaName, controllerName, IdParameterExpression), new { controller = controllerName, action = KnownActionNames.Show }, new { httpMethod = new RESTFulActionConstraint(HttpVerbs.Get, true) });
            }

            if (supportsCreate)
            {
                instance.MapRoute(RouteName(instance.AreaName, controllerName, KnownActionNames.Create), Url(instance.AreaName, controllerName), new { controller = controllerName, action = KnownActionNames.Create }, new { httpMethod = new RESTFulActionConstraint(HttpVerbs.Post) });
            }

            if (SupportsList(controllerType))
            {
                instance.MapRoute(RouteName(instance.AreaName, controllerName, KnownActionNames.Index), Url(instance.AreaName, controllerName), new { controller = controllerName, action = KnownActionNames.Index }, new { httpMethod = new RESTFulActionConstraint(HttpVerbs.Get) });
            }

            return instance;
        }

        private static bool SupportsList(Type controllerType)
        {
            return listType.IsAssignableFrom(controllerType);
        }

        private static bool SupportsDetails(Type controllerType)
        {
            return ImplementsInterface(controllerType, detailsType);
        }

        private static bool SupportsDestroy(Type controllerType)
        {
            return ImplementsInterface(controllerType, destroyType);
        }

        private static bool SupportsUpdate(Type controllerType)
        {
            return ImplementsInterface(controllerType, updateType);
        }

        private static bool SupportsCreate(Type controllerType)
        {
            return createType.IsAssignableFrom(controllerType);
        }

        private static string Url(params string[] arguments)
        {
            return string.Join("/", arguments).ToLower(CultureInfo.CurrentCulture);
        }

        private static string RouteName(params string[] arguments)
        {
            return string.Join("-", arguments).ToLower(CultureInfo.CurrentUICulture);
        }

        private static bool ImplementsInterface(Type classType, Type interfaceType)
        {
            IEnumerable<Type> targetInterfaces = classType.GetInterfaces()
                                                          .Where(type => type.IsGenericType && interfaceType.IsAssignableFrom(type.GetGenericTypeDefinition()));

            return targetInterfaces.Any();
        }
    }
}