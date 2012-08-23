#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Defines a utility class to validate method arguments.
    /// </summary>
    internal static class Invariant
    {
        /// <summary>
        /// Determines whether the given argument is not null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        [DebuggerStepThrough]
        public static void IsNotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, string.Format(CultureInfo.CurrentUICulture, ExceptionMessages.CannotBeNull, parameterName));
            }
        }
    }
}