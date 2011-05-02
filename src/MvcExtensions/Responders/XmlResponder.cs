#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Defines a responder which returns xml.
    /// </summary>
    public class XmlResponder : SerializableResponder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResponder"/> class.
        /// </summary>
        public XmlResponder()
        {
            SupportedFormat = "xml";
            ContentType = KnownMimeTypes.XmlTypes()[0];

            foreach (string mimeType in KnownMimeTypes.XmlTypes())
            {
                SupportedMimeTypes.Add(mimeType);
            }
        }

        /// <summary>
        /// Writes to response output.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="output">The output.</param>
        protected override void WriteTo(object model, TextWriter output)
        {
            Invariant.IsNotNull(output, "output");

            if (model == null)
            {
                return;
            }

            new XmlSerializer(model.GetType()).Serialize(output, model);
        }
    }
}