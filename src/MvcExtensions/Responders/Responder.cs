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
    /// Defines an abstract class which is used to respond to controller action.
    /// </summary>
    public abstract class Responder : IResponder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Responder"/> class.
        /// </summary>
        protected Responder()
        {
            IncludedActions = new List<string>();
            ExcludedActions = new List<string>();
            SupportedMimeTypes = new List<string>();
        }

        /// <summary>
        /// Gets or sets the supported format.
        /// </summary>
        /// <value>The supported format.</value>
        protected string SupportedFormat { get; set; }

        /// <summary>
        /// Gets or sets the supported MIME types.
        /// </summary>
        /// <value>The supported MIME types.</value>
        protected ICollection<string> SupportedMimeTypes { get; private set; }

        /// <summary>
        /// Gets or sets the excluded actions.
        /// </summary>
        /// <value>The excluded actions.</value>
        protected ICollection<string> ExcludedActions { get; private set; }

        /// <summary>
        /// Gets or sets the included actions.
        /// </summary>
        /// <value>The included actions.</value>
        protected ICollection<string> IncludedActions { get; private set; }

        /// <summary>
        /// Includes the specified actions.
        /// </summary>
        /// <param name="actions">The actions.</param>
        public virtual void Include(params string[] actions)
        {
            if (ExcludedActions.Any())
            {
                throw new InvalidOperationException(ExceptionMessages.ExcludeAlreadyExists);
            }

            Copy(actions, IncludedActions);
        }

        /// <summary>
        /// Excludes the specified actions.
        /// </summary>
        /// <param name="actions">The actions.</param>
        public virtual void Exclude(params string[] actions)
        {
            if (IncludedActions.Any())
            {
                throw new InvalidOperationException("You cannot exclude if you have already specified include.");
            }

            Copy(actions, ExcludedActions);
        }

        /// <summary>
        /// Determines whether this instance [can respond to action] the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>
        ///     <c>true</c> if this instance [can respond to action] the specified action; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanRespondToAction(string action)
        {
            // If Nothing is specified then include all.
            if (!IncludedActions.Any() && !ExcludedActions.Any())
            {
                return true;
            }

            // If includes exist but not excludes then check whether  the specified action exists in includes
            if (IncludedActions.Any() && !ExcludedActions.Any())
            {
                return IncludedActions.Contains(action, StringComparer.OrdinalIgnoreCase);
            }

            // If includes does not exist but excludes exist then check whether the specified action does not exists in excludes
            if (!IncludedActions.Any() && ExcludedActions.Any())
            {
                return !ExcludedActions.Contains(action, StringComparer.OrdinalIgnoreCase);
            }

            // Very unusual that we reached here, perhaps the Include/Exclude methods has been overridden.
            return false;
        }

        /// <summary>
        /// Determines whether this instance [can respond to format] the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        ///     <c>true</c> if this instance [can respond to format] the specified format; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanRespondToFormat(string format)
        {
            return SupportedFormat.Equals(format, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether this instance [can respond to MIME type] the specified MIME type.
        /// </summary>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <returns>
        ///     <c>true</c> if this instance [can respond to MIME type] the specified MIME type; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanRespondToMimeType(string mimeType)
        {
            return SupportedMimeTypes.Any(mt => mt.Equals(mimeType, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Responds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public abstract void Respond(ResponderContext context);

        private static void Copy(IEnumerable<string> source, ICollection<string> destination)
        {
            if ((source == null) || (!source.Any()))
            {
                return;
            }

            foreach (string action in source.Where(action => !destination.Contains(action, StringComparer.OrdinalIgnoreCase)))
            {
                destination.Add(action);
            }
        }
    }
}