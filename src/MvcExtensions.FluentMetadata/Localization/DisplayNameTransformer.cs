#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web.Mvc;

    /// <summary>
    /// Splits DisplayName attribute by cammel cases
    /// </summary>
    public class DisplayNameTransformer : TransformerCore
    {
        /// <summary>
        /// Process display attibute
        /// </summary>
        /// <param name="metadata"></param>
        public void Transform(ModelMetadata metadata)
        {
            Invariant.IsNotNull(metadata, "metadata");

            if (metadata.DisplayName == null || metadata.DisplayName == metadata.PropertyName)
            {
                metadata.DisplayName = metadata.PropertyName.SplitUpperCaseToString();
            }
        }

    }
}
