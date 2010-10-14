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
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// A helper class to sort QValue.
    /// </summary>
    public static class QValueSorter
    {
        /// <summary>
        /// Sorts the specified types.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <param name="defective">if set to <c>true</c> [defective].</param>
        /// <returns></returns>
        public static IEnumerable<string> Sort(IEnumerable<string> types, bool defective)
        {
            Invariant.IsNotNull(types, "types");

            IEnumerable<TypeWithQValue> typesWithQValue = types.Select((t, i) => new TypeWithQValue(t, i))
                                                               .Where(tq => tq.Value > 0)
                                                               .OrderBy(tq => tq, new TypeWithQValueComparer(defective))
                                                               .ToList();

            return typesWithQValue.Select(t => t.Name).ToList();
        }

        private sealed class TypeWithQValue
        {
            private const float DefaultValue = 1.0f;

            public TypeWithQValue(string raw, int ordinal)
            {
                Name = string.Empty;
                Value = DefaultValue;

                Parse(raw);
                Ordinal = ordinal;
            }

            public string Name
            {
                get;
                private set;
            }

            public float Value
            {
                get;
                private set;
            }

            public int Ordinal
            {
                get;
                private set;
            }

            private void Parse(string raw)
            {
                string[] parts = raw.Trim().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                Name = parts[0];

                if (parts.Length <= 1)
                {
                    return;
                }

                foreach (string[] subParts in parts.Select(t => parts[1].Trim().Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries)).Where(subParts => (subParts.Length > 1) && subParts[0].Equals("q", StringComparison.OrdinalIgnoreCase)))
                {
                    float value;

                    if (float.TryParse(subParts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                    {
                        Value = value;
                    }

                    break;
                }
            }
        }

        private sealed class TypeWithQValueComparer : IComparer<TypeWithQValue>
        {
            private readonly bool defective;

            public TypeWithQValueComparer(bool defective)
            {
                this.defective = defective;
            }

            public int Compare(TypeWithQValue x, TypeWithQValue y)
            {
                if (x == null)
                {
                    throw new ArgumentNullException("x");
                }

                if (y == null)
                {
                    throw new ArgumentNullException("y");
                }

                if (defective)
                {
                    // When browser is defective we have to give precedence Html over XML
                    bool isXHtml = KnownMimeTypes.HtmlTypes().Contains(x.Name, StringComparer.OrdinalIgnoreCase);
                    bool isXXml = KnownMimeTypes.XmlTypes().Contains(x.Name, StringComparer.OrdinalIgnoreCase);

                    bool isYHtml = KnownMimeTypes.HtmlTypes().Contains(y.Name, StringComparer.OrdinalIgnoreCase);
                    bool isYXml = KnownMimeTypes.XmlTypes().Contains(y.Name, StringComparer.OrdinalIgnoreCase);

                    if (isXHtml && isYXml)
                    {
                        return -1;
                    }

                    if (isXXml && isYHtml)
                    {
                        return 1;
                    }
                }

                return x.Value == y.Value ? x.Ordinal.CompareTo(y.Ordinal) : y.Value.CompareTo(x.Value);
            }
        }
    }
}