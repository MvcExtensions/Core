#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a class which is used to store model state errors for html responders.
    /// </summary>
    public class ModelStateError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelStateError"/> class.
        /// </summary>
        public ModelStateError()
        {
            Messages = new List<string>();
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>The messages.</value>
        public List<string> Messages { get; set; }
    }
}