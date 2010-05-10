#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    /// <summary>
    /// Define a class that is used to store the selectable element setting.
    /// </summary>
    public class ModelMetadataItemSelectableElementSetting : IModelMetadataAdditionalSetting
    {
        /// <summary>
        /// Gets or sets the view data key.
        /// </summary>
        /// <value>The view data key.</value>
        public string ViewDataKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the option label.
        /// </summary>
        /// <value>The option label.</value>
        public string OptionLabel
        {
            get;
            set;
        }
    }
}