#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    /// <summary>
    /// Defines an interface which is used to respond based upon the mime type.
    /// </summary>
    public interface IResponder
    {
        /// <summary>
        /// Includes the specified actions.
        /// </summary>
        /// <param name="actions">The actions.</param>
        void Include(params string[] actions);

        /// <summary>
        /// Excludes the specified actions.
        /// </summary>
        /// <param name="actions">The actions.</param>
        void Exclude(params string[] actions);

        /// <summary>
        /// Determines whether this instance [can respond to action] the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>
        ///     <c>true</c> if this instance [can respond to action] the specified action; otherwise, <c>false</c>.
        /// </returns>
        bool CanRespondToAction(string action);

        /// <summary>
        /// Determines whether this instance [can respond to format] the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        ///     <c>true</c> if this instance [can respond to format] the specified format; otherwise, <c>false</c>.
        /// </returns>
        bool CanRespondToFormat(string format);

        /// <summary>
        /// Determines whether this instance [can respond to MIME type] the specified MIME type.
        /// </summary>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <returns>
        ///     <c>true</c> if this instance [can respond to MIME type] the specified MIME type; otherwise, <c>false</c>.
        /// </returns>
        bool CanRespondToMimeType(string mimeType);

        /// <summary>
        /// Responds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        void Respond(ResponderContext context);
    }
}