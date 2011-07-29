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
    using System.IO;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Defines an interface which returns json.
    /// </summary>
    public class JsonResponder : SerializableResponder
    {
        private static readonly Func<JavaScriptSerializer> defaultSerializerFactory = () => new JavaScriptSerializer();

        private Func<JavaScriptSerializer> serializerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonResponder"/> class.
        /// </summary>
        public JsonResponder()
        {
            SupportedFormat = "json";

            ContentType = KnownMimeTypes.JsonTypes()[0];

            foreach (string mimeType in KnownMimeTypes.JsonTypes())
            {
                SupportedMimeTypes.Add(mimeType);
            }
        }

        /// <summary>
        /// Gets or sets the serializer factory.
        /// </summary>
        /// <value>The serializer factory.</value>
        public Func<JavaScriptSerializer> SerializerFactory
        {
            [DebuggerStepThrough]
            get { return serializerFactory ?? defaultSerializerFactory; }

            [DebuggerStepThrough]
            set { serializerFactory = value; }
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

            output.Write(SerializerFactory().Serialize(model));
        }
    }
}