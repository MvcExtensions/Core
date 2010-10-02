#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Diagnostics;
    using System.Web.Mvc;

    /// <summary>
    /// Defines an abstract attribute which is used to transfer data between viewdata and tempdata.
    /// </summary>
    public abstract class ViewDataTempDataTransferAttribute : FilterAttribute, IActionFilter
    {
        private static string defaultKey = typeof(ViewDataTempDataTransferAttribute).AssemblyQualifiedName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewDataTempDataTransferAttribute"/> class.
        /// </summary>
        protected ViewDataTempDataTransferAttribute()
        {
            Key = DefaultKey;
        }

        /// <summary>
        /// Gets or sets the default key.
        /// </summary>
        /// <value>The default key.</value>
        public static string DefaultKey
        {
            [DebuggerStepThrough]
            get
            {
                return defaultKey;
            }

            [DebuggerStepThrough]
            set
            {
                defaultKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key
        {
            get;
            set;
        }

        /// <summary>
        /// Called before an action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Do nothing, just sleep.
        }

        /// <summary>
        /// Called after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public virtual void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Do nothing, just sleep.
        }
    }
}