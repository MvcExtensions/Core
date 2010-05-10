#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    /// <summary>
    /// Defines a class that is used to store <seealso cref="string"/> metadata.
    /// </summary>
    public class StringMetadataItem : ModelMetadataItem, IModelMetadataFormattableItem
    {
        /// <summary>
        /// Gets or sets the display format.
        /// </summary>
        /// <value>The display format.</value>
        public string DisplayFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the edit format.
        /// </summary>
        /// <value>The edit format.</value>
        public string EditFormat
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
        public bool ApplyFormatInEditMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the value would be converted to null when the value is empty string.
        /// </summary>
        /// <value>
        /// <c>true</c> if [convert empty string to null]; otherwise, <c>false</c>.
        /// </value>
        public bool ConvertEmptyStringToNull
        {
            get;
            set;
        }
    }
}