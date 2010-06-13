#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a base class that is used to store metadata.
    /// </summary>
    public abstract class ModelMetadataItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMetadataItem"/> class.
        /// </summary>
        protected ModelMetadataItem()
        {
            ShowForDisplay = true;
            Validations = new List<IModelValidationMetadata>();
            AdditionalSettings = new List<IModelMetadataAdditionalSetting>();
        }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public Func<string> DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the short name of the display.
        /// </summary>
        /// <value>The short name of the display.</value>
        public Func<string> ShortDisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the template.
        /// </summary>
        /// <value>The name of the template.</value>
        public string TemplateName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public Func<string> Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the whether associate model is read only.
        /// </summary>
        /// <value>The is read only.</value>
        public bool? IsReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the whether associate model is required.
        /// </summary>
        /// <value>The is required.</value>
        public bool? IsRequired
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hide surrounding HTML.
        /// </summary>
        /// <value>The hide surrounding HTML.</value>
        public bool? HideSurroundingHtml
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show for display.
        /// </summary>
        /// <value><c>true</c> if [show for display]; otherwise, <c>false</c>.</value>
        public bool ShowForDisplay
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show for edit.
        /// </summary>
        /// <value>The show for edit.</value>
        public bool? ShowForEdit
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the null display text.
        /// </summary>
        /// <value>The null display text.</value>
        public Func<string> NullDisplayText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the watermark.
        /// </summary>
        /// <value>The watermark.</value>
        public Func<string> Watermark
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or the validations metadata.
        /// </summary>
        /// <value>The validations.</value>
        public IList<IModelValidationMetadata> Validations
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the additional settings.
        /// </summary>
        /// <value>The additional settings.</value>
        public IList<IModelMetadataAdditionalSetting> AdditionalSettings
        {
            get;
            private set;
        }
    }
}