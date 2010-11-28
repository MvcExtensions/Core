#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Globalization;
    using System.Diagnostics;

    internal static class Culture
    {
        public static CultureInfo Current
        {
            [DebuggerStepThrough]
            get
            {
                return CultureInfo.CurrentCulture;
            }
        }

        public static CultureInfo CurrentUI
        {
            [DebuggerStepThrough]
            get
            {
                return CultureInfo.CurrentUICulture;
            }
        }
    }
}