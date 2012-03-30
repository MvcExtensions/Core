#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Define a class that is used to store the render action element setting.
    /// </summary>
    public class RenderActionSetting : IModelMetadataAdditionalSetting
    {
        /// <summary>
        /// Get or sets the delegate which is used to invoke child action.
        /// </summary>
        public Func<HtmlHelper, IHtmlString> Action
        {
            get;
            set;
        }
    }
}