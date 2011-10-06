namespace MvcExtensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// The default filter provider which extract filters from <see cref="IFilterRegistry"/>
    /// </summary>
    public class FilterProvider : IFilterProvider
    {
        private readonly IFilterRegistry filterRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegistry"/> class.
        /// </summary>
        /// <param name="filterRegistry">The filter registry</param>
        public FilterProvider(IFilterRegistry filterRegistry)
        {
            this.filterRegistry = filterRegistry;
        }

        /// <summary>
        /// Returns an enumerator that contains all the <see cref="T:System.Web.Mvc.IFilterProvider"/> instances in the service locator.
        /// </summary>
        /// <returns>
        /// The enumerator that contains all the <see cref="T:System.Web.Mvc.IFilterProvider"/> instances in the service locator.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="actionDescriptor">The action descriptor.</param>
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return filterRegistry.Items.Where(item => item.IsMatching(controllerContext, actionDescriptor))
                .SelectMany(item => item.BuildFilters())
                .OrderBy(x => x.Order)
                .ToArray();
        }
    }
}