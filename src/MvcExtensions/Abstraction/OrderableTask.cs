#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Diagnostics;

    /// <summary>
    /// Defines a class which supports execution order.
    /// </summary>
    public abstract class OrderableTask : Disposable, IOrderableTask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderableTask"/> class.
        /// </summary>
        [DebuggerStepThrough]
        protected OrderableTask()
        {
            Order = DefaultOrder;
        }

        /// <summary>
        /// Gets or sets the default order that the task would execute.
        /// </summary>
        /// <value>The order.</value>
        public static int DefaultOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the order that the task would execute.
        /// </summary>
        /// <value>The order.</value>
        public int Order
        {
            get;
            protected set;
        }
    }
}