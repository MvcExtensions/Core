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

    internal static class ControllerActionInvokerHelper
    {
        public static void Inject(IServiceInjector container, FilterInfo filters)
        {
            ICollection<object> injectedFilters = new HashSet<object>();

            Inject(container, injectedFilters, filters.AuthorizationFilters);
            Inject(container, injectedFilters, filters.ActionFilters);
            Inject(container, injectedFilters, filters.ResultFilters);
            Inject(container, injectedFilters, filters.ExceptionFilters);
        }

        private static bool IsFilterAttribute<TFilter>(TFilter filter)
        {
            return KnownTypes.FilterType.IsAssignableFrom(filter.GetType());
        }

        private static void Inject<TFilter>(IServiceInjector container, ICollection<object> injectedFilters, IEnumerable<TFilter> filters) 
            where TFilter : class
        {
            foreach (TFilter filter in filters.Where(filter => !injectedFilters.Contains(filter) && IsFilterAttribute(filter)))
            {
                container.Inject(filter);
                injectedFilters.Add(filter);
            }
        }
    }
}