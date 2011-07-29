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
    using System.Web.Mvc;

    internal static class ControllerActionInvokerHelper
    {
        public static FilterInfo Merge(ContainerAdapter container, FilterInfo decoratedFilters, FilterInfo registeredFilters)
        {
            Inject(container, decoratedFilters);

            FilterInfo mergedFilters = new FilterInfo();

            MergeOrdered(mergedFilters, decoratedFilters, registeredFilters);
            MergeUnordered(mergedFilters, decoratedFilters, registeredFilters);

            return mergedFilters;
        }

        private static void MergeOrdered(FilterInfo mergedFilters, FilterInfo decoratedFilters, FilterInfo registeredFilters)
        {
            Merge(decoratedFilters.AuthorizationFilters, registeredFilters.AuthorizationFilters, IsFilterAttribute)
                .Cast<IMvcFilter>()
                .OrderBy(filter => filter.Order)
                .Cast<IAuthorizationFilter>().Each(filter => mergedFilters.AuthorizationFilters.Add(filter));

            Merge(decoratedFilters.ActionFilters, registeredFilters.ActionFilters, IsFilterAttribute)
                .Cast<IMvcFilter>()
                .OrderBy(filter => filter.Order)
                .Cast<IActionFilter>().Each(filter => mergedFilters.ActionFilters.Add(filter));

            Merge(decoratedFilters.ResultFilters, registeredFilters.ResultFilters, IsFilterAttribute)
                .Cast<IMvcFilter>()
                .OrderBy(filter => filter.Order)
                .Cast<IResultFilter>().Each(filter => mergedFilters.ResultFilters.Add(filter));

            Merge(decoratedFilters.ExceptionFilters, registeredFilters.ExceptionFilters, IsFilterAttribute)
                .Cast<IMvcFilter>()
                .OrderBy(filter => filter.Order)
                .Cast<IExceptionFilter>().Each(filter => mergedFilters.ExceptionFilters.Add(filter));
        }

        private static void MergeUnordered(FilterInfo mergedFilters, FilterInfo decoratedFilters, FilterInfo registeredFilters)
        {
            Merge(decoratedFilters.AuthorizationFilters, registeredFilters.AuthorizationFilters, IsNonFilterAttribute)
                .Reverse().Each(filter => mergedFilters.AuthorizationFilters.Insert(0, filter));

            Merge(decoratedFilters.ActionFilters, registeredFilters.ActionFilters, IsNonFilterAttribute)
                .Reverse().Each(filter => mergedFilters.ActionFilters.Insert(0, filter));

            Merge(decoratedFilters.ResultFilters, registeredFilters.ResultFilters, IsNonFilterAttribute)
                .Reverse().Each(filter => mergedFilters.ResultFilters.Insert(0, filter));

            Merge(decoratedFilters.ExceptionFilters, registeredFilters.ExceptionFilters, IsNonFilterAttribute)
                .Reverse().Each(filter => mergedFilters.ExceptionFilters.Insert(0, filter));
        }

        private static IEnumerable<T> Merge<T>(IEnumerable<T> source1, IEnumerable<T> source2, Func<T, bool> filter)
        {
            return source1.Where(filter).Concat(source2.Where(filter));
        }

        private static bool IsFilterAttribute<TFilter>(TFilter filter)
        {
            return KnownTypes.FilterType.IsAssignableFrom(filter.GetType());
        }

        private static bool IsNonFilterAttribute<TFilter>(TFilter filter)
        {
            return !KnownTypes.FilterType.IsAssignableFrom(filter.GetType());
        }

        private static void Inject(IServiceInjector container, FilterInfo decoratedFilters)
        {
            ICollection<object> injectedFilters = new List<object>();

            Inject(container, injectedFilters, decoratedFilters.AuthorizationFilters);
            Inject(container, injectedFilters, decoratedFilters.ActionFilters);
            Inject(container, injectedFilters, decoratedFilters.ResultFilters);
            Inject(container, injectedFilters, decoratedFilters.ExceptionFilters);
        }

        private static void Inject<TFilter>(IServiceInjector container, ICollection<object> injectedFilters, IEnumerable<TFilter> filters) where TFilter : class
        {
            foreach (TFilter filter in filters.Where(filter => IsFilterAttribute(filter) && !injectedFilters.Contains(filter)))
            {
                container.Inject(filter);
                injectedFilters.Add(filter);
            }
        }
    }
}