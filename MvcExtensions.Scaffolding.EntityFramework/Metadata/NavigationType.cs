#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    /// <summary>
    /// Defined an enum to indicate the type of relation
    /// </summary>
    public enum NavigationType
    {
        /// <summary>
        /// One to  One
        /// </summary>
        One,

        /// <summary>
        /// One to Many
        /// </summary>
        Many
    }
}