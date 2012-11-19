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
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines class to fluently configure metadata.
    /// </summary>
    [TypeForwardedFrom(KnownAssembly.MvcExtensions)]
    public class ModelMetadataItemBuilder<TValue> : IModelMetadataItemBuilder<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMetadataItemBuilder{TValue}"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public ModelMetadataItemBuilder(ModelMetadataItem item)
        {
            Invariant.IsNotNull(item, "item");

            Item = item;
        }

        /// <summary>
        /// Gets the internal item.
        /// </summary>
        /// <value>The item.</value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ModelMetadataItem Item
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            get;
            private set;
        }

        private ModelMetadataItemBuilder<TValue> This
        {
            [DebuggerStepThrough]
            get
            {
                return this;
            }
        }

        ModelMetadataItem IModelMetadataItemBuilder<TValue>.Item
        {
            get
            {
                return Item;
            }
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.DisplayName(Func<string> value)
        {
            return DisplayName(value);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.ShortDisplayName(string value)
        {
            return ShortDisplayName(value);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.ShortDisplayName(Func<string> value)
        {
            return ShortDisplayName(value);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Template(string value)
        {
            return Template(value);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Description(string value)
        {
            return Description(value);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Description(Func<string> value)
        {
            return Description(value);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.ReadOnly()
        {
            return ReadOnly();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Writable()
        {
            return Writable();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Compare(string otherProperty)
        {
            return Compare(otherProperty);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Compare(string otherProperty, string errorMessage)
        {
            return Compare(otherProperty, errorMessage);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Compare(string otherProperty, Func<string> errorMessage)
        {
            return Compare(otherProperty, errorMessage);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Compare(string otherProperty, Type errorMessageResourceType, string errorMessageResourceName)
        {
            return Compare(otherProperty, errorMessageResourceType, errorMessageResourceName);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Required()
        {
            return Required();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Required(string errorMessage)
        {
            return Required(errorMessage);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Required(Func<string> errorMessage)
        {
            return Required(errorMessage);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Required(Type errorMessageResourceType, string errorMessageResourceName)
        {
            return Required(errorMessageResourceType, errorMessageResourceName);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Optional()
        {
            return Optional();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.AsHidden()
        {
            return AsHidden();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.AsHidden(bool hideSurroundingHtml)
        {
            return AsHidden(hideSurroundingHtml);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.HideSurroundingHtml()
        {
            return HideSurroundingHtml();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.ShowSurroundingHtml()
        {
            return ShowSurroundingHtml();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.AllowHtml()
        {
            return AllowHtml();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.DisallowHtml()
        {
            return DisallowHtml();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.ShowForDisplay()
        {
            return ShowForDisplay();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.HideForDisplay()
        {
            return HideForDisplay();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.ShowForEdit()
        {
            return ShowForEdit();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.HideForEdit()
        {
            return HideForEdit();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Show()
        {
            return Show();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Hide()
        {
            return Hide();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.NullDisplayText(string value)
        {
            return NullDisplayText(value);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.NullDisplayText(Func<string> value)
        {
            return NullDisplayText(value);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Watermark(string value)
        {
            return Watermark(value);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Watermark(Func<string> value)
        {
            return Watermark(value);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Order(int value)
        {
            return Order(value);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.DisplayFormat(string format)
        {
            return DisplayFormat(format);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.DisplayFormat(Func<string> format)
        {
            return DisplayFormat(format);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.EditFormat(string format)
        {
            return EditFormat(format);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.EditFormat(Func<string> format)
        {
            return EditFormat(format);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Format(string value)
        {
            return Format(value);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Format(Func<string> value)
        {
            return Format(value);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.ApplyFormatInEditMode()
        {
            return ApplyFormatInEditMode();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Validate(Func<TValue, bool> validator, string errorMessage)
        {
            return Validate(validator, errorMessage);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Validate(Func<TValue, bool> validator, Func<string> errorMessage)
        {
            return Validate(validator, errorMessage);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Validate<TModel>(Func<TModel, bool> validator, string errorMessage)
        {
            return Validate(validator, errorMessage);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Validate<TModel>(Func<TModel, bool> validator, Func<string> errorMessage)
        {
            return Validate(validator, errorMessage);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Validate<TModel>(Func<TModel, TValue, bool> validator, string errorMessage)
        {
            return Validate(validator, errorMessage);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.Validate<TModel>(Func<TModel, TValue, bool> validator, Func<string> errorMessage)
        {
            return Validate(validator, errorMessage);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.ValidateBy<TValidator>()
        {
            return ValidateBy<TValidator>();
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.ValidateBy<TValidator>(Action<TValidator> configure)
        {
            return ValidateBy(configure);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.ValidateBy<TValidator>(TValidator validator)
        {
            return ValidateBy(validator);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.ValidateBy<TValidator>(Func<TValidator> factory)
        {
            return ValidateBy(factory);
        }

        IModelMetadataItemBuilder<TValue> IModelMetadataItemBuilder<TValue>.DisplayName(string value)
        {
            return DisplayName(value);
        }

        /// <summary>
        /// Sets the Display name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> DisplayName(string value)
        {
            return DisplayName(() => value);
        }

        /// <summary>
        /// Sets the Display name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> DisplayName(Func<string> value)
        {
            Item.DisplayName = value;

            return This;
        }

        /// <summary>
        /// Sets the short display name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> ShortDisplayName(string value)
        {
            return ShortDisplayName(() => value);
        }

        /// <summary>
        /// Sets the short display name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> ShortDisplayName(Func<string> value)
        {
            Item.ShortDisplayName = value;

            return This;
        }

        /// <summary>
        /// Sets the Templates name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Template([AspMvcEditorTemplate, AspMvcDisplayTemplate] string value)
        {
            Item.TemplateName = value;

            return This;
        }

        /// <summary>
        /// Sets the Description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Description(string value)
        {
            return Description(() => value);
        }

        /// <summary>
        /// Sets the Description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Description(Func<string> value)
        {
            Item.Description = value;

            return This;
        }

        /// <summary>
        /// Marks the value as read only.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> ReadOnly()
        {
            Item.IsReadOnly = true;

            return This;
        }

        /// <summary>
        /// Marks the value as writable, this is handy when the framework initializes the
        /// value as read only and you want to negate it.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Writable()
        {
            Item.IsReadOnly = false;

            return This;
        }

        /// <summary>
        /// Sets the other property that the value must match.
        /// </summary>
        /// <param name="otherProperty">The other property</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Compare(string otherProperty)
        {
            return Compare(otherProperty, null, null, null);
        }

        /// <summary>
        /// Sets the other property that the value must match.
        /// </summary>
        /// <param name="otherProperty">The other property</param>
        /// <param name="errorMessage">The error message when the value is not specified.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Compare(string otherProperty, string errorMessage)
        {
            return Compare(otherProperty, () => errorMessage);
        }

        /// <summary>
        /// Sets the other property that the value must match.
        /// </summary>
        /// <param name="otherProperty">The other property</param>
        /// <param name="errorMessage">The error message when the value is not specified.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Compare(string otherProperty, Func<string> errorMessage)
        {
            return Compare(otherProperty, errorMessage, null, null);
        }

        /// <summary>
        /// Sets the other property that the value must match.
        /// </summary>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <param name="otherProperty">The other property</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Compare(string otherProperty, Type errorMessageResourceType, string errorMessageResourceName)
        {
            return Compare(otherProperty, null, errorMessageResourceType, errorMessageResourceName);
        }

        /// <summary>
        /// Marks the value as required.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Required()
        {
            return Required(null, null, null);
        }

        /// <summary>
        /// Marks the value as required.
        /// </summary>
        /// <param name="errorMessage">The error message when the value is not specified.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Required(string errorMessage)
        {
            return Required(() => errorMessage);
        }

        /// <summary>
        /// Marks the value as required.
        /// </summary>
        /// <param name="errorMessage">The error message when the value is not specified.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Required(Func<string> errorMessage)
        {
            return Required(errorMessage, null, null);
        }

        /// <summary>
        /// Marks the value as required.
        /// </summary>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Required(Type errorMessageResourceType, string errorMessageResourceName)
        {
            return Required(null, errorMessageResourceType, errorMessageResourceName);
        }

        /// <summary>
        /// Marks the value as optional, this is handy when the framework initializes the 
        /// value as required and you want to negate it.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Optional()
        {
            Item.IsRequired = false;

            var requiredValidation = Item.GetValidation<RequiredValidationMetadata>();

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
        public ModelMetadataItemBuilder<TValue> AsHidden()
        {
            Item.TemplateName = "HiddenInput";

            return This;
        }

        /// <summary>
        /// Marks the value to render as hidden input element in edit mode.
        /// </summary>
        /// <param name="hideSurroundingHtml">Indicates whether the value will appear in display mode</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> AsHidden(bool hideSurroundingHtml)
        {
            Item.TemplateName = "HiddenInput";
            Item.HideSurroundingHtml = hideSurroundingHtml;

            return This;
        }

        /// <summary>
        /// Hides surrounding HTML.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> HideSurroundingHtml()
        {
            Item.HideSurroundingHtml = true;

            return This;
        }

        /// <summary>
        /// Shows surrounding HTML.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> ShowSurroundingHtml()
        {
            Item.HideSurroundingHtml = false;

            return This;
        }

        /// <summary>
        /// Disables request validation for property.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> AllowHtml()
        {
            Item.RequestValidationEnabled = false;

            return This;
        }

        /// <summary>
        /// Enebles request validation for property.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> DisallowHtml()
        {
            Item.RequestValidationEnabled = true;

            return This;
        }

        /// <summary>
        /// Shows the value in display mode.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> ShowForDisplay()
        {
            Item.ShowForDisplay = true;

            return This;
        }

        /// <summary>
        /// Hides the value in display mode.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> HideForDisplay()
        {
            Item.ShowForDisplay = false;

            return This;
        }

        /// <summary>
        /// Shows the value in edit mode.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> ShowForEdit()
        {
            Item.ShowForEdit = true;

            return This;
        }

        /// <summary>
        /// Hides the value in edit mode.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> HideForEdit()
        {
            Item.ShowForEdit = false;

            return This;
        }

        /// <summary>
        /// Shows the value in both display and edit mode.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Show()
        {
            Item.ShowForDisplay = true;
            Item.ShowForEdit = true;

            return This;
        }

        /// <summary>
        /// Hides the value in both display and edit mode.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Hide()
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
        public ModelMetadataItemBuilder<TValue> NullDisplayText(string value)
        {
            return NullDisplayText(() => value);
        }

        /// <summary>
        /// Sets the display text when the value is null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> NullDisplayText(Func<string> value)
        {
            Item.NullDisplayText = value;

            return This;
        }

        /// <summary>
        /// Sets the Watermark.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Watermark(string value)
        {
            return Watermark(() => value);
        }

        /// <summary>
        /// Sets the Watermark.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Watermark(Func<string> value)
        {
            Item.Watermark = value;

            return This;
        }

        /// <summary>
        /// Sets the order
        /// </summary>
        /// <param name="value">The order</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Order(int value)
        {
            Item.Order = value;
            return This;
        }

        /// <summary>
        /// Sets the format in display mode.
        /// </summary>
        /// <param name="format">The value.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> DisplayFormat(string format)
        {
            return DisplayFormat(() => format);
        }

        /// <summary>
        /// Sets the format in display mode.
        /// </summary>
        /// <param name="format">The value.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> DisplayFormat(Func<string> format)
        {
            Item.DisplayFormat = format;

            return This;
        }

        /// <summary>
        /// Sets the format in edit mode.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> EditFormat(string format)
        {
            return EditFormat(() => format);
        }

        /// <summary>
        /// Sets the format in edit mode.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> EditFormat(Func<string> format)
        {
            Item.EditFormat = format;

            return This;
        }

        /// <summary>
        /// Sets format for both display and edit mode.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Format(string value)
        {
            return Format(() => value);
        }

        /// <summary>
        /// Sets format for both display and edit mode.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Format(Func<string> value)
        {
            Item.DisplayFormat = Item.EditFormat = value;

            return This;
        }

        /// <summary>
        /// Indicates to apply format in edit mode.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> ApplyFormatInEditMode()
        {
            Item.ApplyFormatInEditMode = true;

            return This;
        }

        /// <summary>
        /// Sets the delegate based custom validation for value. 
        /// </summary>
        /// <param name="validator">Should return "true" if valid; otherwise, "false'.</param>
        /// <param name="errorMessage">Error message.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Validate(Func<TValue, bool> validator, string errorMessage = null)
        {
            var message = string.IsNullOrEmpty(errorMessage)
                              ? ExceptionMessages.TheValueIsInvalid // "The {0} value is invalid."
                              : errorMessage;

            return Validate<object>((container, value) => validator(value), () => message);
        }

        /// <summary>
        /// Sets the delegate based custom validation for value.
        /// </summary>
        /// <param name="validator">Should return "true" if valid; otherwise, "false'.</param>
        /// <param name="errorMessage">Error message.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Validate(Func<TValue, bool> validator, Func<string> errorMessage)
        {
            return Validate<object>((container, value) => validator(value), errorMessage);
        }

        /// <summary>
        /// Sets the delegate based custom validation for value.
        /// </summary>
        /// <param name="validator">Should return "true" if valid; otherwise, "false'.</param>
        /// <param name="errorMessage">Error message.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Validate<TModel>(Func<TModel, bool> validator, string errorMessage = null)
        {
            var message = string.IsNullOrEmpty(errorMessage)
                              ? ExceptionMessages.TheValueIsInvalid // "The {0} value is invalid."
                              : errorMessage;

            return Validate<TModel>((container, value) => validator(container), () => message);
        }

        /// <summary>
        /// Sets the delegate based custom validation for value.
        /// </summary>
        /// <param name="validator">Should return "true" if valid; otherwise, "false'.</param>
        /// <param name="errorMessage">Error message.</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Validate<TModel>(Func<TModel, bool> validator, Func<string> errorMessage)
        {
            return Validate<TModel>((container, value) => validator(container), errorMessage);
        }

        /// <summary>
        /// Sets the delegate based custom validation for model and value.
        /// </summary>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> Validate<TModel>(Func<TModel, TValue, bool> validator, string errorMessage = null)
        {
            var message = string.IsNullOrEmpty(errorMessage)
                              ? ExceptionMessages.TheValueIsInvalid // "The {0} value is invalid."
                              : errorMessage;

            return Validate(validator, () => message);
        }

        /// <summary>
        /// Sets the delegate based custom validation for model and value.
        /// </summary>
        /// <returns></returns>
        public virtual ModelMetadataItemBuilder<TValue> Validate<TModel>(Func<TModel, TValue, bool> validator, Func<string> errorMessage)
        {
            Invariant.IsNotNull(errorMessage, "errorMessage");

            var validation = Item.GetValidationOrCreateNew<DelegateBasedModelMetadata>();
            var localValidator = validator;
            validation.AddValidator(((container, value) => localValidator((TModel)container, (TValue)value)), errorMessage());

            return this;
        }

        /// <summary>
        /// Sets the <typeparamref name="TValidator"/> to validate value.
        /// </summary>
        /// <typeparam name="TValidator">The type of validator</typeparam>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> ValidateBy<TValidator>()
            where TValidator : CustomValidatorAttribute
        {
            return ValidateBy<TValidator>(v => { });
        }

        /// <summary>
        /// Sets the <typeparamref name="TValidator"/> to validate value.
        /// </summary>
        /// <typeparam name="TValidator">The type of validator</typeparam>
        /// <param name="configure">The configuration</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> ValidateBy<TValidator>(Action<TValidator> configure)
            where TValidator : CustomValidatorAttribute
        {
            return ValidateBy(CreateFactory<TValidator>(), configure);
        }

        /// <summary>
        /// Sets the <typeparamref name="TValidator"/> to validate value.
        /// </summary>
        /// <typeparam name="TValidator">The type of validator</typeparam>
        /// <param name="validator">The instance of validator</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> ValidateBy<TValidator>(TValidator validator)
            where TValidator : CustomValidatorAttribute
        {
            return ValidateBy(() => validator);
        }

        /// <summary>
        /// Sets the <typeparamref name="TValidator"/> to validate value.
        /// </summary>
        /// <typeparam name="TValidator">The type of validator</typeparam>
        /// <param name="factory">The factory used to build validator</param>
        /// <returns></returns>
        public ModelMetadataItemBuilder<TValue> ValidateBy<TValidator>(Func<TValidator> factory)
            where TValidator : CustomValidatorAttribute
        {
            return ValidateBy(factory, v => { });
        }

        /// <summary>
        /// Sets the <typeparamref name="TValidator"/> to validate value.
        /// </summary>
        /// <typeparam name="TValidator">The type of validator</typeparam>
        /// <param name="factory">The factory used to build validator</param>
        /// <param name="configure">The configuration</param>
        /// <returns></returns>
        protected virtual ModelMetadataItemBuilder<TValue> ValidateBy<TValidator>(Func<TValidator> factory, Action<TValidator> configure) 
            where TValidator : CustomValidatorAttribute
        {
            var validation = Item.GetValidationOrCreateNew<CustomValidationMetadata<TValidator>>();
            
            validation.Factory = factory;
            validation.ConfigureValidator = configure;

            return this;
        }

        /// <summary>
        /// Marks the value as required.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        protected virtual ModelMetadataItemBuilder<TValue> Required(Func<string> errorMessage, Type errorMessageResourceType, string errorMessageResourceName)
        {
            Item.IsRequired = true;

            var validation = Item.GetValidationOrCreateNew<RequiredValidationMetadata>();

            validation.ErrorMessage = errorMessage;
            validation.ErrorMessageResourceType = errorMessageResourceType;
            validation.ErrorMessageResourceName = errorMessageResourceName;

            return This;
        }

        /// <summary>
        /// Sets the other property that the value must match.
        /// </summary>
        /// <param name="otherProperty">The other property.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        protected virtual ModelMetadataItemBuilder<TValue> Compare(string otherProperty, Func<string> errorMessage, Type errorMessageResourceType, string errorMessageResourceName)
        {
            var compareValidation = Item.GetValidationOrCreateNew<CompareValidationMetadata>();

            compareValidation.OtherProperty = otherProperty;
            compareValidation.ErrorMessage = errorMessage;
            compareValidation.ErrorMessageResourceType = errorMessageResourceType;
            compareValidation.ErrorMessageResourceName = errorMessageResourceName;

            return This;
        }

        private static Func<TValidator> CreateFactory<TValidator>() where TValidator : CustomValidatorAttribute
        {
            var validatorType = typeof(TValidator);
            ConstructorInfo constructor = validatorType.GetConstructor(new Type[]{});

            if (constructor == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The constructor of {0} with no parameters is not available.", validatorType));
            }

            return Expression.Lambda<Func<TValidator>>(Expression.New(constructor)).Compile();
        }
    }
}