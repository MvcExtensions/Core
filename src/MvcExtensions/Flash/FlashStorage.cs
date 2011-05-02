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
    using System.Web.Mvc;

    /// <summary>
    /// Defines class which abstract the <see cref="TempDataDictionary"/> for storing flash messages.
    /// </summary>
    public class FlashStorage
    {
        /// <summary>
        /// The key in the tempdata upon which the messages are stored.
        /// </summary>
        public static readonly string Key = typeof(FlashStorage).FullName;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlashStorage"/> class.
        /// </summary>
        /// <param name="backingStore">The backing store.</param>
        public FlashStorage(TempDataDictionary backingStore)
        {
            if (backingStore == null)
            {
                throw new ArgumentNullException("backingStore");
            }

            BackingStore = backingStore;
        }

        /// <summary>
        /// Gets or sets the backing store.
        /// </summary>
        /// <value>The backing store.</value>
        public TempDataDictionary BackingStore { get; private set; }

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <value>The messages.</value>
        public IEnumerable<KeyValuePair<string, string>> Messages
        {
            get
            {
                try
                {
                    object value;

                    if (!BackingStore.TryGetValue(Key, out value))
                    {
                        return new List<KeyValuePair<string, string>>();
                    }

                    return (IEnumerable<KeyValuePair<string, string>>)value;
                }
                finally
                {
                    BackingStore.Remove(Key);
                }
            }
        }

        /// <summary>
        /// Adds the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        public void Add(string type, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            IList<KeyValuePair<string, string>> messages;
            object temp;

            if (!BackingStore.TryGetValue(Key, out temp))
            {
                messages = new List<KeyValuePair<string, string>>();
                BackingStore.Add(Key, messages);
            }
            else
            {
                messages = (IList<KeyValuePair<string, string>>)temp;
            }

            var item = messages.SingleOrDefault(p => p.Key.Equals(type, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(item.Value))
            {
                messages.Remove(item);
            }

            messages.Add(new KeyValuePair<string, string>(type, message));

            BackingStore.Keep(Key);
        }
    }
}