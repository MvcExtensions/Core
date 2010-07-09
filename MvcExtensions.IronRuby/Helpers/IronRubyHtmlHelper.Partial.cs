#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.IronRuby
{
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public partial class IronRubyHtmlHelper
    {
        public void RenderPartial(string partialViewName)
        {
            RenderPartialExtensions.RenderPartial(this, partialViewName);
        }

        public void RenderPartial(string partialViewName, ViewDataDictionary viewData)
        {
            RenderPartialExtensions.RenderPartial(this, partialViewName, viewData);
        }

        public void RenderPartial(string partialViewName, object model)
        {
            RenderPartialExtensions.RenderPartial(this, partialViewName, model);
        }

        public void RenderPartial(string partialViewName, object model, ViewDataDictionary viewData)
        {
            RenderPartialExtensions.RenderPartial(this, partialViewName, model, viewData);
        }
    }
}