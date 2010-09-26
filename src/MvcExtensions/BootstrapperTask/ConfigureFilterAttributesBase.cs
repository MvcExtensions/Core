#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class to configure <seealso cref="FilterAttribute"/> for <see cref="Controller"/> or action methods.
    /// </summary>
    public abstract class ConfigureFilterAttributesBase : BootstrapperTask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureFilterAttributesBase"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        protected ConfigureFilterAttributesBase(IFilterRegistry registry)
        {
            Invariant.IsNotNull(registry, "registry");

            Registry = registry;
        }

        /// <summary>
        /// Gets the filter registry.
        /// </summary>
        /// <value>The filter registry.</value>
        protected IFilterRegistry Registry
        {
            get;
            private set;
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            Configure();

            return TaskContinuation.Continue;
        }

        /// <summary>
        /// Configures the filters.
        /// </summary>
        protected abstract void Configure();
    }
}