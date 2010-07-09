#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.IronRuby
{
    using System.Web.Routing;

    using Hash =  global::IronRuby.Builtins.Hash;

    internal static class HashExtensions
    {
        public static RouteValueDictionary ToRouteValueDictionary(this Hash dictionary)
        {
            RouteValueDictionary rvd = new RouteValueDictionary();

            if (dictionary != null)
            {
                foreach (var pair in dictionary)
                {
                    rvd.Add(pair.Key.ToString(), pair.Value);
                }
            }

            return rvd;
        }
    }
}