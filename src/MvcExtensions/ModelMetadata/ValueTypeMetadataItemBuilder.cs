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
    using System.Linq;

    /// <summary>
    /// Defines a class to fluently configure metadata of a <seealso cref="ValueType"/> type.
    /// </summary>
    /// <typeparam name="TValueType">The type of the value type.</typeparam>
    public class ValueTypeMetadataItemBuilder<TValueType> : ModelMetadataItemBuilder<ValueTypeMetadataItem, ValueTypeMetadataItemBuilder<TValueType>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueTypeMetadataItemBuilder&lt;TValueType&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public ValueTypeMetadataItemBuilder(ValueTypeMetadataItem item) : base(item)
        {
        }

        /// <summary>
        /// Sets the format in display mode.
        /// </summary>
        /// <param name="format">The value.</param>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> DisplayFormat(string format)
        {
            return DisplayFormat(() => format);
        }

        /// <summary>
        /// Sets the format in display mode.
        /// </summary>
        /// <param name="format">The value.</param>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> DisplayFormat(Func<string> format)
        {
            Item.DisplayFormat = format;

            return this;
        }

        /// <summary>
        /// Sets the format in edit mode.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> EditFormat(string format)
        {
            return EditFormat(() => format);
        }

        /// <summary>
        /// Sets the format in edit mode.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> EditFormat(Func<string> format)
        {
            Item.EditFormat = format;

            return this;
        }

        /// <summary>
        /// Sets format for both display and edit mode.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> Format(string value)
        {
            return Format(() => value);
        }

        /// <summary>
        /// Sets format for both display and edit mode.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> Format(Func<string> value)
        {
            Item.DisplayFormat = Item.EditFormat = value;

            return this;
        }

        /// <summary>
        /// Indicates to apply format in edit mode.
        /// </summary>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> ApplyFormatInEditMode()
        {
            Item.ApplyFormatInEditMode = true;

            return this;
        }

        /// <summary>
        /// Sets the range of value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> Range(TValueType minimum, TValueType maximum)
        {
            return Range(minimum, maximum, null, null, null);
        }

        /// <summary>
        /// Sets the range of value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> Range(TValueType minimum, TValueType maximum, string errorMessage)
        {
            return Range(minimum, maximum, () => errorMessage);
        }

        /// <summary>
        /// Sets the range of value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> Range(TValueType minimum, TValueType maximum, Func<string> errorMessage)
        {
            return Range(minimum, maximum, errorMessage, null, null);
        }

        /// <summary>
        /// Sets the range of value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> Range(TValueType minimum, TValueType maximum, Type errorMessageResourceType, string errorMessageResourceName)
        {
            return Range(minimum, maximum, null, errorMessageResourceType, errorMessageResourceName);
        }

        /// <summary>
        /// Sets the range of value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        protected virtual ValueTypeMetadataItemBuilder<TValueType> Range(TValueType minimum, TValueType maximum, Func<string> errorMessage, Type errorMessageResourceType, string errorMessageResourceName)
        {
            RangeValidationMetadata<TValueType> rangeValidation = Item.GetValidationOrCreateNew<RangeValidationMetadata<TValueType>>();

            rangeValidation.Minimum = minimum;
            rangeValidation.Maximum = maximum;
            rangeValidation.ErrorMessage = errorMessage;
            rangeValidation.ErrorMessageResourceType = errorMessageResourceType;
            rangeValidation.ErrorMessageResourceName = errorMessageResourceName;

            return this;
        }
        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="viewDataKey">The view data key, the value of the view data key must be a <seealso cref="IEnumerable{SelectListItem}"/>.</param>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> AsDropDownList(string viewDataKey)
        {
            return AsDropDownList(viewDataKey, (Func<string>)null);
        }

        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="viewDataKey">The view data key, the value of the view data key must be a <seealso cref="IEnumerable{SelectListItem}"/>.</param>
        /// <param name="optionLabel">The option label.</param>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> AsDropDownList(string viewDataKey, string optionLabel)
        {
            return AsDropDownList(viewDataKey, () => optionLabel);
        }

        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="viewDataKey">The view data key, the value of the view data key must be a <seealso cref="IEnumerable{SelectListItem}"/>.</param>
        /// <param name="optionLabel">The option label.</param>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> AsDropDownList(string viewDataKey, Func<string> optionLabel)
        {
            return AsDropDownList(viewDataKey, optionLabel, "DropDownList");
        }

        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="viewDataKey">The view data key.</param>
        /// <param name="optionLabel">The option label.</param>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> AsDropDownList(string viewDataKey, string optionLabel, string template)
        {
            return AsDropDownList(viewDataKey, () => optionLabel, template);
        }

        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="viewDataKey">The view data key.</param>
        /// <param name="optionLabel">The option label.</param>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        public ValueTypeMetadataItemBuilder<TValueType> AsDropDownList(string viewDataKey, Func<string> optionLabel, string template)
        {
            return HtmlSelect(template, viewDataKey, optionLabel);
        }
        /// <summary>
        /// Render the value using the specified template.
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="viewDataKey">The view data key, the value of the view data key must be a <seealso cref="IEnumerable{T}"/>.</param>
        /// <param name="optionLabel">The option label.</param>
        /// <returns></returns>
        protected virtual ValueTypeMetadataItemBuilder<TValueType> HtmlSelect(string templateName, string viewDataKey, Func<string> optionLabel)
        {
            var selectableElementSetting = Item.GetAdditionalSettingOrCreateNew<ModelMetadataItemSelectableElementSetting>();

            selectableElementSetting.ViewDataKey = viewDataKey;

            if (optionLabel != null)
            {
                selectableElementSetting.OptionLabel = optionLabel;
            }

            Item.TemplateName = templateName;

            return this;
        }
    }
}