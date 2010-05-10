#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    /// <summary>
    /// Represents an enum which defines the life time of the registered service.
    /// </summary>
    public enum LifetimeType
    {
        /// <summary>
        /// The same object will be returned for the same request.
        /// </summary>
        PerRequest,

        /// <summary>
        /// This object will be created only once and the same object will be returned each time it is requested.
        /// </summary>
        Singleton,

        /// <summary>
        /// The object will be created every time it is requested.
        /// </summary>
        Transient
    }
}