#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Diagnostics;

    internal static class KnownActionNames
    {
        public const string Index = "index";
        public const string Show = "show";
        public const string New = "new";
        public const string Create = "create";
        public const string Edit = "edit";
        public const string Update = "update";
        public const string Destroy = "destroy";

        private static readonly string[] all = new[] { Index, Show, New, Create, Edit, Update, Destroy };
        private static readonly string[] createAndUpdate = new[] { Create, Update };

        [DebuggerStepThrough]
        public static string[] All()
        {
            return all;
        }

        [DebuggerStepThrough]
        public static string[] CreateAndUpdate()
        {
            return createAndUpdate;
        }
    }
}