#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Defines a class to fluently configure metadata of a <seealso cref="string"/> type.
    /// </summary>
    public class StringMetadataItemBuilder : ModelMetadataItemBuilder<StringMetadataItem, StringMetadataItemBuilder>
    {
        private static string emailExpression = @"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$";
        private static string emailErrorMessage = ExceptionMessages.InvalidEmailAddressFormat;

        private static string urlExpression = @"(ftp|http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
        private static string urlErrorMessage = ExceptionMessages.InvalidUrlFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringMetadataItemBuilder"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public StringMetadataItemBuilder(StringMetadataItem item) : base(item)
        {
        }

        /// <summary>
        /// Gets or sets the email expression.
        /// </summary>
        /// <value>The email expression.</value>
        public static string EmailExpression
        {
            [DebuggerStepThrough]
            get
            {
                return emailExpression;
            }

            [DebuggerStepThrough]
            set
            {
                emailExpression = value;
            }
        }

        /// <summary>
        /// Gets or sets the email error message.
        /// </summary>
        /// <value>The email error message.</value>
        public static string EmailErrorMessage
        {
            [DebuggerStepThrough]
            get
            {
                return emailErrorMessage;
            }

            [DebuggerStepThrough]
            set
            {
                emailErrorMessage = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the email error message resource.
        /// </summary>
        /// <value>The type of the email error message resource.</value>
        public static Type EmailErrorMessageResourceType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the email error message resource.
        /// </summary>
        /// <value>The name of the email error message resource.</value>
        public static string EmailErrorMessageResourceName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URL expression.
        /// </summary>
        /// <value>The URL expression.</value>
        public static string UrlExpression
        {
            [DebuggerStepThrough]
            get
            {
                return urlExpression;
            }

            [DebuggerStepThrough]
            set
            {
                urlExpression = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL error message.
        /// </summary>
        /// <value>The URL error message.</value>
        public static string UrlErrorMessage
        {
            [DebuggerStepThrough]
            get
            {
                return urlErrorMessage;
            }

            [DebuggerStepThrough]
            set
            {
                urlErrorMessage = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the URL error message resource.
        /// </summary>
        /// <value>The type of the URL error message resource.</value>
        public static Type UrlErrorMessageResourceType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the URL error message resource.
        /// </summary>
        /// <value>The name of the URL error message resource.</value>
        public static string UrlErrorMessageResourceName
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the format in display mode.
        /// </summary>
        /// <param name="format">The value.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder DisplayFormat(string format)
        {
            return DisplayFormat(() => format);
        }

        /// <summary>
        /// Sets the format in display mode.
        /// </summary>
        /// <param name="format">The value.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder DisplayFormat(Func<string> format)
        {
            Item.DisplayFormat = format;

            return this;
        }

        /// <summary>
        /// Sets the format in edit mode.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder EditFormat(string format)
        {
            return EditFormat(() => format);
        }

        /// <summary>
        /// Sets the format in edit mode.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder EditFormat(Func<string> format)
        {
            Item.EditFormat = format;

            return this;
        }

        /// <summary>
        /// Sets format for both display and edit mode.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder Format(string value)
        {
            return Format(() => value);
        }

        /// <summary>
        /// Sets format for both display and edit mode.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder Format(Func<string> value)
        {
            Item.DisplayFormat = Item.EditFormat = value;

            return this;
        }

        /// <summary>
        /// Indicates to apply format in edit mode.
        /// </summary>
        /// <returns></returns>
        public StringMetadataItemBuilder ApplyFormatInEditMode()
        {
            Item.ApplyFormatInEditMode = true;

            return this;
        }

        /// <summary>
        /// Indicates that the value would appear as email address in display mode.
        /// </summary>
        /// <returns></returns>
        public StringMetadataItemBuilder AsEmail()
        {
            Template("EmailAddress");

            if (GetExpressionValidation() != null)
            {
                throw new InvalidOperationException(ExceptionMessages.CannotApplyEmailWhenThereIsAnActiveExpression);
            }

            return Expression(EmailExpression, ((EmailErrorMessageResourceType == null) && string.IsNullOrEmpty(EmailErrorMessageResourceName)) ? () => EmailErrorMessage : (Func<string>)null, EmailErrorMessageResourceType, EmailErrorMessageResourceName);
        }

        /// <summary>
        /// Indicates that the value would appear as raw html in display mode, so no encoding will be performed.
        /// </summary>
        /// <returns></returns>
        public StringMetadataItemBuilder AsHtml()
        {
            return Template("Html");
        }

        /// <summary>
        /// Indicates that the value would appear as url in display mode.
        /// </summary>
        /// <returns></returns>
        public StringMetadataItemBuilder AsUrl()
        {
            Template("Url");

            if (GetExpressionValidation() != null)
            {
                throw new InvalidOperationException(ExceptionMessages.CannotApplyUrlWhenThereIsAnActiveExpression);
            }

            return Expression(UrlExpression, ((UrlErrorMessageResourceType == null) && string.IsNullOrEmpty(UrlErrorMessageResourceName)) ? () => UrlErrorMessage : (Func<string>)null, UrlErrorMessageResourceType, UrlErrorMessageResourceName);
        }

        /// <summary>
        /// Marks the value to render as text area element in edit mode.
        /// </summary>
        /// <returns></returns>
        public StringMetadataItemBuilder AsMultilineText()
        {
            return Template("MultilineText");
        }

        /// <summary>
        /// Marks the value to render as password element in edit mode.
        /// </summary>
        /// <returns></returns>
        public StringMetadataItemBuilder AsPassword()
        {
            return Template("Password");
        }

        /// <summary>
        /// Sets the regular expression that the value must match, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder Expression(string pattern)
        {
            return Expression(pattern, null, null, null);
        }

        /// <summary>
        /// Sets the regular expression that the value must match, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder Expression(string pattern, string errorMessage)
        {
            return Expression(pattern, () => errorMessage);
        }

        /// <summary>
        /// Sets the regular expression that the value must match, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder Expression(string pattern, Func<string> errorMessage)
        {
            return Expression(pattern, errorMessage, null, null);
        }

        /// <summary>
        /// Sets the regular expression that the value must match, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder Expression(string pattern, Type errorMessageResourceType, string errorMessageResourceName)
        {
            return Expression(pattern, null, errorMessageResourceType, errorMessageResourceName);
        }

        /// <summary>
        /// Sets the maximum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder MaximumLength(int length)
        {
            return MaximumLength(length, null, null, null);
        }

        /// <summary>
        /// Sets the maximum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder MaximumLength(int length, string errorMessage)
        {
            return MaximumLength(length, () => errorMessage);
        }

        /// <summary>
        /// Sets the maximum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder MaximumLength(int length, Func<string> errorMessage)
        {
            return MaximumLength(length, errorMessage, null, null);
        }

        /// <summary>
        /// Sets the maximum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder MaximumLength(int length, Type errorMessageResourceType, string errorMessageResourceName)
        {
            return MaximumLength(length, null, errorMessageResourceType, errorMessageResourceName);
        }

        /// <summary>
        /// Sets the minimum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder MinimumLength(int length)
        {
            return MinimumLength(length, null, null, null);
        }

        /// <summary>
        /// Sets the minimum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder MinimumLength(int length, string errorMessage)
        {
            return MinimumLength(length, () => errorMessage);
        }

        /// <summary>
        /// Sets the minimum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder MinimumLength(int length, Func<string> errorMessage)
        {
            return MinimumLength(length, errorMessage, null, null);
        }

        /// <summary>
        /// Sets the minimum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        public StringMetadataItemBuilder MinimumLength(int length, Type errorMessageResourceType, string errorMessageResourceName)
        {
            return MinimumLength(length, null, errorMessageResourceType, errorMessageResourceName);
        }

        /// <summary>
        /// Sets the regular expression that the value must match, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        protected virtual StringMetadataItemBuilder Expression(string pattern, Func<string> errorMessage, Type errorMessageResourceType, string errorMessageResourceName)
        {
            RegularExpressionValidationMetadata regularExpressionValidation = Item.GetValidationOrCreateNew<RegularExpressionValidationMetadata>();
            
            regularExpressionValidation.Pattern = pattern;
            regularExpressionValidation.ErrorMessage = errorMessage;
            regularExpressionValidation.ErrorMessageResourceType = errorMessageResourceType;
            regularExpressionValidation.ErrorMessageResourceName = errorMessageResourceName;

            return this;
        }

        /// <summary>
        /// Sets the maximum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        protected virtual StringMetadataItemBuilder MaximumLength(int length, Func<string> errorMessage, Type errorMessageResourceType, string errorMessageResourceName)
        {
            StringLengthValidationMetadata stringLengthValidation = Item.GetValidationOrCreateNew<StringLengthValidationMetadata>();

            stringLengthValidation.Maximum = length;
            stringLengthValidation.ErrorMessage = errorMessage;
            stringLengthValidation.ErrorMessageResourceType = errorMessageResourceType;
            stringLengthValidation.ErrorMessageResourceName = errorMessageResourceName;

            return this;
        }

        /// <summary>
        /// Sets the minimum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        protected virtual StringMetadataItemBuilder MinimumLength(int length, Func<string> errorMessage, Type errorMessageResourceType, string errorMessageResourceName)
        {
            StringLengthValidationMetadata stringLengthValidation = Item.GetValidationOrCreateNew<StringLengthValidationMetadata>();

            stringLengthValidation.Minimum = length;
            stringLengthValidation.ErrorMessage = errorMessage;
            stringLengthValidation.ErrorMessageResourceType = errorMessageResourceType;
            stringLengthValidation.ErrorMessageResourceName = errorMessageResourceName;

            return this;
        }

        private RegularExpressionValidationMetadata GetExpressionValidation()
        {
            return Item.GetValidation<RegularExpressionValidationMetadata>();
        }
    }
}