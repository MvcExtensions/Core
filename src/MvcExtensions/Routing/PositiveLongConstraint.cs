#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;

    /// <summary>
    /// Defines a class which is used to ensure the URL parameter value is  a positive <seealso cref="long"/>.
    /// </summary>
    [CLSCompliant(false)]
    public class PositiveLongConstraint : RangeConstraint<long>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositiveLongConstraint"/> class.
        /// </summary>
        public PositiveLongConstraint() : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PositiveIntegerConstraint"/> class.
        /// </summary>
        /// <param name="optional">if set to <c>true</c> [optional].</param>
        public PositiveLongConstraint(bool optional) : base(1, long.MaxValue, optional)
        {
        }
    }
}