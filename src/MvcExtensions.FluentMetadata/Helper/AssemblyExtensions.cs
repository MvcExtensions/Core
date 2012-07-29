#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// <see cref="Assembly"/> class extensions
    /// </summary>
    internal static class AssemblyExtensions
    {
        /// <summary>
        /// Get all loadable types 
        /// from the given assembly
        /// </summary>
        /// <param name="assembly">assembly to scan</param>
        /// <returns>List of loadable types</returns>
        [DebuggerStepThrough]
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            IEnumerable<Type> types = null;
            if (assembly != null)
            {
                try
                {
                    types = assembly.GetTypes().Where(t => t != null);
                }
                catch (ReflectionTypeLoadException e)
                {
                    types = e.Types.Where(t => t != null);
                }
            }

            return types ?? Enumerable.Empty<Type>();
        }
    }
}