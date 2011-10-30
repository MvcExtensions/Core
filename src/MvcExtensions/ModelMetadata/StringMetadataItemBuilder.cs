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

    /// <summary>
    /// Defines a class to fluently configure metadata of a <seealso cref="string"/> type.
    /// </summary>
    public static class StringMetadataItemBuilder
    {
        private static string emailExpression = @"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$";
        private static string emailErrorMessage = ExceptionMessages.InvalidEmailAddressFormat;

        private static string urlExpression = @"(ftp|http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
        private static string urlErrorMessage = ExceptionMessages.InvalidUrlFormat;

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
        /// Indicates that the value would appear as email address in display mode.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> AsEmail(this ModelMetadataItemBuilder<string> self)
        {
            self.Template("EmailAddress");

            if (self.Item.GetValidation<RegularExpressionValidationMetadata>() != null)
            {
                throw new InvalidOperationException(ExceptionMessages.CannotApplyEmailWhenThereIsAnActiveExpression);
            }

            return Expression(self, EmailExpression, ((EmailErrorMessageResourceType == null) && string.IsNullOrEmpty(EmailErrorMessageResourceName)) ? () => EmailErrorMessage : (Func<string>)null, EmailErrorMessageResourceType, EmailErrorMessageResourceName);
        }

        /// <summary>
        /// Indicates that the value would appear as raw html in display mode, so no encoding will be performed.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> AsHtml(this ModelMetadataItemBuilder<string> self)
        {
            return self.Template("Html");
        }

        /// <summary>
        /// Indicates that the value would appear as url in display mode.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> AsUrl(this ModelMetadataItemBuilder<string> self)
        {
            self.Template("Url");

            if (self.Item.GetValidation<RegularExpressionValidationMetadata>() != null)
            {
                throw new InvalidOperationException(ExceptionMessages.CannotApplyUrlWhenThereIsAnActiveExpression);
            }

            return Expression(self, UrlExpression, ((UrlErrorMessageResourceType == null) && string.IsNullOrEmpty(UrlErrorMessageResourceName)) ? () => UrlErrorMessage : (Func<string>)null, UrlErrorMessageResourceType, UrlErrorMessageResourceName);
        }

        /// <summary>
        /// Marks the value to render as text area element in edit mode.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> AsMultilineText(this ModelMetadataItemBuilder<string> self)
        {
            return self.Template("MultilineText");
        }

        /// <summary>
        /// Marks the value to render as password element in edit mode.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> AsPassword(this ModelMetadataItemBuilder<string> self)
        {
            return self.Template("Password");
        }

        /// <summary>
        /// Sets the regular expression that the value must match, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> Expression(this ModelMetadataItemBuilder<string> self, string pattern)
        {
            return Expression(self, pattern, null, null, null);
        }

        /// <summary>
        /// Sets the regular expression that the value must match, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> Expression(this ModelMetadataItemBuilder<string> self, string pattern, string errorMessage)
        {
            return Expression(self, pattern, () => errorMessage);
        }

        /// <summary>
        /// Sets the regular expression that the value must match, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> Expression(this ModelMetadataItemBuilder<string> self, string pattern, Func<string> errorMessage)
        {
            return Expression(self, pattern, errorMessage, null, null);
        }

        /// <summary>
        /// Sets the regular expression that the value must match, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> Expression(this ModelMetadataItemBuilder<string> self, string pattern, Type errorMessageResourceType, string errorMessageResourceName)
        {
            return Expression(self, pattern, null, errorMessageResourceType, errorMessageResourceName);
        }

        /// <summary>
        /// Sets the maximum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> MaximumLength(this ModelMetadataItemBuilder<string> self, int length)
        {
            return MaximumLength(self, length, null, null, null);
        }

        /// <summary>
        /// Sets the maximum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="length">The length.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> MaximumLength(this ModelMetadataItemBuilder<string> self, int length, string errorMessage)
        {
            return MaximumLength(self, length, () => errorMessage);
        }

        /// <summary>
        /// Sets the maximum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="length">The length.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> MaximumLength(this ModelMetadataItemBuilder<string> self, int length, Func<string> errorMessage)
        {
            return MaximumLength(self, length, errorMessage, null, null);
        }

        /// <summary>
        /// Sets the maximum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="length">The length.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> MaximumLength(this ModelMetadataItemBuilder<string> self, int length, Type errorMessageResourceType, string errorMessageResourceName)
        {
            return MaximumLength(self, length, null, errorMessageResourceType, errorMessageResourceName);
        }

        /// <summary>
        /// Sets the minimum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> MinimumLength(this ModelMetadataItemBuilder<string> self, int length)
        {
            return MinimumLength(self, length, null, null, null);
        }

        /// <summary>
        /// Sets the minimum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="length">The length.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> MinimumLength(this ModelMetadataItemBuilder<string> self, int length, string errorMessage)
        {
            return MinimumLength(self, length, () => errorMessage);
        }

        /// <summary>
        /// Sets the minimum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="length">The length.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> MinimumLength(this ModelMetadataItemBuilder<string> self, int length, Func<string> errorMessage)
        {
            return MinimumLength(self, length, errorMessage, null, null);
        }

        /// <summary>
        /// Sets the minimum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="length">The length.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<string> MinimumLength(this ModelMetadataItemBuilder<string> self, int length, Type errorMessageResourceType, string errorMessageResourceName)
        {
            return MinimumLength(self, length, null, errorMessageResourceType, errorMessageResourceName);
        }

        /// <summary>
        /// Sets the regular expression that the value must match, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        private static ModelMetadataItemBuilder<string> Expression(ModelMetadataItemBuilder<string> self, string pattern, Func<string> errorMessage, Type errorMessageResourceType, string errorMessageResourceName)
        {
            var regularExpressionValidation = self.Item.GetValidationOrCreateNew<RegularExpressionValidationMetadata>();
            
            regularExpressionValidation.Pattern = pattern;
            regularExpressionValidation.ErrorMessage = errorMessage;
            regularExpressionValidation.ErrorMessageResourceType = errorMessageResourceType;
            regularExpressionValidation.ErrorMessageResourceName = errorMessageResourceName;

            return self;
        }

        /// <summary>
        /// Sets the maximum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="length">The length.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        private static ModelMetadataItemBuilder<string> MaximumLength(ModelMetadataItemBuilder<string> self, int length, Func<string> errorMessage, Type errorMessageResourceType, string errorMessageResourceName)
        {
            var stringLengthValidation = self.Item.GetValidationOrCreateNew<StringLengthValidationMetadata>();

            stringLengthValidation.Maximum = length;
            stringLengthValidation.ErrorMessage = errorMessage;
            stringLengthValidation.ErrorMessageResourceType = errorMessageResourceType;
            stringLengthValidation.ErrorMessageResourceName = errorMessageResourceName;

            return self;
        }

        /// <summary>
        /// Sets the minimum length of the value, this comes into action when is <code>Required</code> is <code>true</code>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="length">The length.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorMessageResourceType">Type of the error message resource.</param>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns></returns>
        private static ModelMetadataItemBuilder<string> MinimumLength(ModelMetadataItemBuilder<string> self, int length, Func<string> errorMessage, Type errorMessageResourceType, string errorMessageResourceName)
        {
            var stringLengthValidation = self.Item.GetValidationOrCreateNew<StringLengthValidationMetadata>();

            stringLengthValidation.Minimum = length;
            stringLengthValidation.ErrorMessage = errorMessage;
            stringLengthValidation.ErrorMessageResourceType = errorMessageResourceType;
            stringLengthValidation.ErrorMessageResourceName = errorMessageResourceName;

            return self;
        }
    }
}