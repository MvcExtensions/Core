#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Defines a base class to fluently configure metadata.
    /// </summary>
    public abstract class ModelMetadataItemBuilder<TItem, TItemBuilder> : IFluentSyntax where TItem : ModelMetadataItem where TItemBuilder : ModelMetadataItemBuilder<TItem, TItemBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMetadataItemBuilder{TItem,TItemBuilder}"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        protected ModelMetadataItemBuilder(TItem item)
        {
            Invariant.IsNotNull(item, "item");

            Item = item;
        }

        /// <summary>
        /// Gets the internal item.
        /// </summary>
        /// <value>The item.</value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TItem Item
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            get;
            private set;
        }

        private TItemBuilder This
        {
            [DebuggerStepThrough]
            get
            {
                return this as TItemBuilder;
            }
        }

        /// <summary>
        /// Sets the Display name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public TItemBuilder DisplayName(string value)
        {
            return DisplayName(() => value);
        }

        /// <summary>
        /// Sets the Display name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public TItemBuilder DisplayName(Func<string> value)
        {
            Item.DisplayName = value;

            return This;
        }

        /// <summary>
        /// Sets the short display name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public TItemBuilder ShortDisplayName(string value)
        {
            return ShortDisplayName(() => value);
        }

        /// <summary>
        /// Sets the short display name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public TItemBuilder ShortDisplayName(Func<string> value)
        {
            Item.ShortDisplayName = value;

            return This;
        }

        /// <summary>
        /// Sets the Templates name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public TItemBuilder Template(string value)
        {
            Item.TemplateName = value;

            return This;
        }

        /// <summary>
        /// Sets the Description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public TItemBuilder Description(string value)
        {
            return Description(() => value);
        }

        /// <summary>
        /// Sets the Description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public TItemBuilder Description(Func<string> value)
        {
            Item.Description = value;

            return This;
        }

        /// <summary>
        /// Marks the value as read only.
        /// </summary>
        /// <returns></returns>
        public TItemBuilder ReadOnly()
        {
            Item.IsReadOnly = true;

            return This;
        }

        /// <summary>
        /// Marks the value as writable, this is handy when the framework initializes the
        /// value as read only and you want to negate it.
        /// </summary>
        /// <returns></returns>
        public TItemBuilder Writable()
        {
            Item.IsReadOnly = false;

            return This;
        }

        /// <summary>
        /// Marks the value as required.
        /// </summary>
        /// <returns></returns>
        public TItemBuilder Required()
        {
            return Required(null, null, null);
        }

        /// <summary>
        /// Marks the value as required.
        /// </summary>
        /// <param name="errorMessage">The error message when the value is not specified.</param>
        /// <returns></returns>
        public TItemBuilder Required(string errorMessage)
        {
            return Required(() => errorMessage);
        }

        /// <summary>
        /// Marks the value as required.
        /// </summary>
        /// <param name="errorMessage">The error message when the value is not specified.</param>
        /// <returns></returns>
        public TItemBuilder Required(Func<string> errorMessage)
        {
            return Required(errorMessage, null, null);
        }

        /// <summary>
        /// Marks the value as required.
        /// </summary>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        public TItemBuilder Required(Type errorMessageResourceType, string errorMessageResourceName)
        {
            return Required(null, errorMessageResourceType, errorMessageResourceName);
        }

        /// <summary>
        /// Marks the value as optional, this is handy when the framework initializes the 
        /// value as required and you want to negate it.
        /// </summary>
        /// <returns></returns>
        public TItemBuilder Optional()
        {
            Item.IsRequired = false;

            RequiredValidationMetadata requiredValidation = GetRequiredValidation();

            if (requiredValidation != null)
            {
                Item.Validations.Remove(requiredValidation);
            }

            return This;
        }

        /// <summary>
        /// Marks the value to render as hidden input element in edit mode.
        /// </summary>
        /// <returns></returns>
        public TItemBuilder AsHidden()
        {
            Item.TemplateName = "HiddenInput";

            return This;
        }

        /// <summary>
        /// Marks the value to render as hidden input element in edit mode.
        /// </summary>
        /// <param name="hideSurroundingHtml">Indicates whether the value will appear in display mode</param>
        /// <returns></returns>
        public TItemBuilder AsHidden(bool hideSurroundingHtml)
        {
            Item.TemplateName = "HiddenInput";
            Item.HideSurroundingHtml = hideSurroundingHtml;

            return This;
        }

        /// <summary>
        /// Shows the value in display mode.
        /// </summary>
        /// <returns></returns>
        public TItemBuilder ShowForDisplay()
        {
            Item.ShowForDisplay = true;

            return This;
        }

        /// <summary>
        /// Hides the value in display mode.
        /// </summary>
        /// <returns></returns>
        public TItemBuilder HideForDisplay()
        {
            Item.ShowForDisplay = false;

            return This;
        }

        /// <summary>
        /// Shows the value in edit mode.
        /// </summary>
        /// <returns></returns>
        public TItemBuilder ShowForEdit()
        {
            Item.ShowForEdit = true;

            return This;
        }

        /// <summary>
        /// Hides the value in edit mode.
        /// </summary>
        /// <returns></returns>
        public TItemBuilder HideForEdit()
        {
            Item.ShowForEdit = false;

            return This;
        }

        /// <summary>
        /// Shows the value in both display and edit mode.
        /// </summary>
        /// <returns></returns>
        public TItemBuilder Show()
        {
            Item.ShowForDisplay = true;
            Item.ShowForEdit = true;

            return This;
        }

        /// <summary>
        /// Hides the value in both display and edit mode.
        /// </summary>
        /// <returns></returns>
        public TItemBuilder Hide()
        {
            Item.ShowForDisplay = false;
            Item.ShowForEdit = false;

            return This;
        }

        /// <summary>
        /// Sets the display text when the value is null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public TItemBuilder NullDisplayText(string value)
        {
            return NullDisplayText(() => value);
        }

        /// <summary>
        /// Sets the display text when the value is null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public TItemBuilder NullDisplayText(Func<string> value)
        {
            Item.NullDisplayText = value;

            return This;
        }

        /// <summary>
        /// Sets the Watermark.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public TItemBuilder Watermark(string value)
        {
            return Watermark(() => value);
        }

        /// <summary>
        /// Sets the Watermark.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public TItemBuilder Watermark(Func<string> value)
        {
            Item.Watermark = value;

            return This;
        }

        /// <summary>
        /// Marks the value as required.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        protected virtual TItemBuilder Required(Func<string> errorMessage, Type errorMessageResourceType, string errorMessageResourceName)
        {
            Item.IsRequired = true;

            RequiredValidationMetadata requiredValidation = GetRequiredValidation();

            if (requiredValidation == null)
            {
                requiredValidation = new RequiredValidationMetadata();
                Item.Validations.Add(requiredValidation);
            }

            requiredValidation.ErrorMessage = errorMessage;
            requiredValidation.ErrorMessageResourceType = errorMessageResourceType;
            requiredValidation.ErrorMessageResourceName = errorMessageResourceName;

            return This;
        }

        private RequiredValidationMetadata GetRequiredValidation()
        {
            return Item.Validations.OfType<RequiredValidationMetadata>().SingleOrDefault();
        }
    }
}