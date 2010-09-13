#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Reflection;

    internal static class KnownAssembly
    {
        public const string AspNetMvcFutureAssemblyName = "Microsoft.Web.Mvc";

        public static readonly Assembly AspNetMvcAssembly = KnownTypes.ControllerType.Assembly;

        public static readonly Assembly AspNetMvcExtensionsAssembly = typeof(KnownAssembly).Assembly;
    }
}