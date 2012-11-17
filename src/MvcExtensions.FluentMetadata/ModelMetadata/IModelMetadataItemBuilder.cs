namespace MvcExtensions
{
    using System;
    using System.ComponentModel;
    using System.Web.Mvc;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines a contract to fluently configure metadata.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface IModelMetadataItemBuilder<out TValue>
    {
        /// <summary>
        /// Gets the internal item.
        /// </summary>
        /// <value>The item.</value>
        [EditorBrowsable(EditorBrowsableState.Never)] ModelMetadataItem Item { [EditorBrowsable(EditorBrowsableState.Never)] get; }

        /// <summary>
        /// Sets the Display name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> DisplayName(string value);

        /// <summary>
        /// Sets the Display name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> DisplayName(Func<string> value);

        /// <summary>
        /// Sets the short display name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> ShortDisplayName(string value);

        /// <summary>
        /// Sets the short display name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> ShortDisplayName(Func<string> value);

        /// <summary>
        /// Sets the Templates name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Template([AspMvcEditorTemplate, AspMvcDisplayTemplate] string value);

        /// <summary>
        /// Sets the Description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Description(string value);

        /// <summary>
        /// Sets the Description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Description(Func<string> value);

        /// <summary>
        /// Marks the value as read only.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> ReadOnly();

        /// <summary>
        /// Marks the value as writable, this is handy when the framework initializes the
        /// value as read only and you want to negate it.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Writable();

        /// <summary>
        /// Sets the other property that the value must match.
        /// </summary>
        /// <param name="otherProperty">The other property</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Compare(string otherProperty);

        /// <summary>
        /// Sets the other property that the value must match.
        /// </summary>
        /// <param name="otherProperty">The other property</param>
        /// <param name="errorMessage">The error message when the value is not specified.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Compare(string otherProperty, string errorMessage);

        /// <summary>
        /// Sets the other property that the value must match.
        /// </summary>
        /// <param name="otherProperty">The other property</param>
        /// <param name="errorMessage">The error message when the value is not specified.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Compare(string otherProperty, Func<string> errorMessage);

        /// <summary>
        /// Sets the other property that the value must match.
        /// </summary>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <param name="otherProperty">The other property</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Compare(string otherProperty, Type errorMessageResourceType, string errorMessageResourceName);

        /// <summary>
        /// Marks the value as required.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Required();

        /// <summary>
        /// Marks the value as required.
        /// </summary>
        /// <param name="errorMessage">The error message when the value is not specified.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Required(string errorMessage);

        /// <summary>
        /// Marks the value as required.
        /// </summary>
        /// <param name="errorMessage">The error message when the value is not specified.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Required(Func<string> errorMessage);

        /// <summary>
        /// Marks the value as required.
        /// </summary>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Required(Type errorMessageResourceType, string errorMessageResourceName);

        /// <summary>
        /// Marks the value as optional, this is handy when the framework initializes the 
        /// value as required and you want to negate it.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Optional();

        /// <summary>
        /// Marks the value to render as hidden input element in edit mode.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> AsHidden();

        /// <summary>
        /// Marks the value to render as hidden input element in edit mode.
        /// </summary>
        /// <param name="hideSurroundingHtml">Indicates whether the value will appear in display mode</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> AsHidden(bool hideSurroundingHtml);

        /// <summary>
        /// Hides surrounding HTML.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> HideSurroundingHtml();

        /// <summary>
        /// Shows surrounding HTML.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> ShowSurroundingHtml();

        /// <summary>
        /// Disables request validation for property.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> AllowHtml();

        /// <summary>
        /// Enebles request validation for property.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> DisallowHtml();

        /// <summary>
        /// Shows the value in display mode.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> ShowForDisplay();

        /// <summary>
        /// Hides the value in display mode.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> HideForDisplay();

        /// <summary>
        /// Shows the value in edit mode.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> ShowForEdit();

        /// <summary>
        /// Hides the value in edit mode.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> HideForEdit();

        /// <summary>
        /// Shows the value in both display and edit mode.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Show();

        /// <summary>
        /// Hides the value in both display and edit mode.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Hide();

        /// <summary>
        /// Sets the display text when the value is null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> NullDisplayText(string value);

        /// <summary>
        /// Sets the display text when the value is null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> NullDisplayText(Func<string> value);

        /// <summary>
        /// Sets the Watermark.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Watermark(string value);

        /// <summary>
        /// Sets the Watermark.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Watermark(Func<string> value);

        /// <summary>
        /// Sets the order
        /// </summary>
        /// <param name="value">The order</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Order(int value);

        /// <summary>
        /// Sets the format in display mode.
        /// </summary>
        /// <param name="format">The value.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> DisplayFormat(string format);

        /// <summary>
        /// Sets the format in display mode.
        /// </summary>
        /// <param name="format">The value.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> DisplayFormat(Func<string> format);

        /// <summary>
        /// Sets the format in edit mode.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> EditFormat(string format);

        /// <summary>
        /// Sets the format in edit mode.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> EditFormat(Func<string> format);

        /// <summary>
        /// Sets format for both display and edit mode.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Format(string value);

        /// <summary>
        /// Sets format for both display and edit mode.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Format(Func<string> value);

        /// <summary>
        /// Indicates to apply format in edit mode.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> ApplyFormatInEditMode();

        /// <summary>
        /// Sets the delegate based custom validation for value.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Validate(Func<TValue, bool> validator, string errorMessage = null);

        /// <summary>
        /// Sets the delegate based custom validation for value.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Validate(Func<TValue, bool> validator, Func<string> errorMessage);

        /// <summary>
        /// Sets the delegate based custom validation for value.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Validate<TModel>(Func<TModel, bool> validator, string errorMessage = null);

        /// <summary>
        /// Sets the delegate based custom validation for value.
        /// </summary>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> Validate<TModel>(Func<TModel, bool> validator, Func<string> errorMessage);

        /// <summary>
        /// Sets the <typeparamref name="TValidator"/> to validate value.
        /// </summary>
        /// <typeparam name="TValidator">The type of validator</typeparam>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> ValidateBy<TValidator>()
            where TValidator : CustomValidatorAttribute;

        /// <summary>
        /// Sets the <typeparamref name="TValidator"/> to validate value.
        /// </summary>
        /// <typeparam name="TValidator">The type of validator</typeparam>
        /// <param name="configure">The configuration</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> ValidateBy<TValidator>(Action<TValidator> configure)
            where TValidator : CustomValidatorAttribute;

        /// <summary>
        /// Sets the <typeparamref name="TValidator"/> to validate value.
        /// </summary>
        /// <typeparam name="TValidator">The type of validator</typeparam>
        /// <param name="validator">The instance of validator</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> ValidateBy<TValidator>(TValidator validator)
            where TValidator : CustomValidatorAttribute;

        /// <summary>
        /// Sets the <typeparamref name="TValidator"/> to validate value.
        /// </summary>
        /// <typeparam name="TValidator">The type of validator</typeparam>
        /// <param name="factory">The factory used to build validator</param>
        /// <returns></returns>
        IModelMetadataItemBuilder<TValue> ValidateBy<TValidator>(Func<TValidator> factory)
            where TValidator : CustomValidatorAttribute;
    }
}