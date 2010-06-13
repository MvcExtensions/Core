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
    /// Represents an interface of a metadata that supports formatting.
    /// </summary>
    public interface IModelMetadataFormattableItem
    {
        /// <summary>
        /// Gets or sets the display format.
        /// </summary>
        /// <value>The display format.</value>
        Func<string> DisplayFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the edit format.
        /// </summary>
        /// <value>The edit format.</value>
        Func<string> EditFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to apply format in edit mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if [apply format in edit mode]; otherwise, <c>false</c>.
        /// </value>
        bool ApplyFormatInEditMode
        {
            get;
            set;
        }
    }
}