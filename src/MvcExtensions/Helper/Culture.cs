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
    using JetBrains.Annotations;

    internal static class Culture
    {
        [NotNull] public static CultureInfo Current
        {
            [DebuggerStepThrough]
            get
            {
                return CultureInfo.CurrentCulture;
            }
        }

        [NotNull] public static CultureInfo CurrentUI
        {
            [DebuggerStepThrough]
            get
            {
                return CultureInfo.CurrentUICulture;
            }
        }
    }
}