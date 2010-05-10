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
    /// Defines a helper class to sort types depending upon it value and order.
    /// </summary>
    public static class QValueSorter
    {
        /// <summary>
        /// Sorts the specified types based upon its QValue and order.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        public static IEnumerable<string> Sort(string types)
        {
            return string.IsNullOrEmpty(types) ?
                   new string[0] :
                   Sort(types.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// Sorts the specified types based upon its QValue and order.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        public static IEnumerable<string> Sort(IEnumerable<string> types)
        {
            Invariant.IsNotNull(types, "types");

            IEnumerable<TypeWithQValue> typesWithQValue = types.Select((t, i) => new TypeWithQValue(t, i))
                                                               .Where(tq => tq.Value > 0)
                                                               .OrderBy(tq => tq, new TypeWithQValueComparer())
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

                    if (float.TryParse(subParts[1], out value))
                    {
                        Value = value;
                    }

                    break;
                }
            }
        }

        private sealed class TypeWithQValueComparer : IComparer<TypeWithQValue>
        {
            public int Compare(TypeWithQValue x, TypeWithQValue y)
            {
                return x.Value == y.Value ? x.Ordinal.CompareTo(y.Ordinal) : y.Value.CompareTo(x.Value);
            }
        }
    }
}