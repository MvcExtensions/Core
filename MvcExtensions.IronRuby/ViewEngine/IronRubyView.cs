#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.IronRuby
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web.Mvc;

    using Microsoft.Scripting.Hosting;

    /// <summary>
    /// Defines an Iron Ruby view.
    /// </summary>
    public class IronRubyView : IView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IronRubyView"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="scriptEngine">The script engine.</param>
        public IronRubyView(string content, IScriptEngine scriptEngine) : this(content, scriptEngine, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IronRubyView"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="scriptEngine">The script engine.</param>
        /// <param name="master">The master.</param>
        public IronRubyView(string content, IScriptEngine scriptEngine, IronRubyView master)
        {
            Content = content;
            ScriptEngine = scriptEngine;
            Master = master;
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        protected string Content
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the script engine.
        /// </summary>
        /// <value>The script engine.</value>
        protected IScriptEngine ScriptEngine
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the master view.
        /// </summary>
        /// <value>The master.</value>
        protected IronRubyView Master
        {
            get;
            private set;
        }

        /// <summary>
        /// Renders the specified view context by using the specified the writer object.
        /// </summary>
        /// <param name="viewContext">The view context.</param><param name="writer">The writer object.</param>
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            ScriptEngine.ExecuteInScope(scope => RenderView(scope, viewContext, writer));
        }

        private void RenderView(ScriptScope scope, ViewContext viewContext, TextWriter writer)
        {
            const string MethodMissing = "def {0}.method_missing(methodname)\r\n\t get_Item(methodname.to_s)\r\nend";

            scope.SetVariable("html", new IronRubyHtmlHelper(viewContext, new Container(viewContext.ViewData)));
            scope.SetVariable("url", new UrlHelper(viewContext.RequestContext));
            scope.SetVariable("ajax", new AjaxHelper(viewContext, new Container(viewContext.ViewData)));
            scope.SetVariable("model", viewContext.ViewData.Model);
            scope.SetVariable("temp_data", viewContext.RouteData);
            scope.SetVariable("view_context", viewContext);
            scope.SetVariable("view_data", viewContext.ViewData);
            scope.SetVariable("context", viewContext.HttpContext);
            scope.SetVariable("request", viewContext.HttpContext.Request);
            scope.SetVariable("response", viewContext.HttpContext.Response);
            scope.SetVariable("user", viewContext.HttpContext.User);
            scope.SetVariable("route_data", viewContext.RouteData);
            scope.SetVariable("session", viewContext.HttpContext.Session);
            scope.SetVariable("server", viewContext.HttpContext.Server);
            scope.SetVariable("application", viewContext.HttpContext.Application);
            scope.SetVariable("cache", viewContext.HttpContext.Cache);

            StringBuilder script = new StringBuilder();
            string renderPageMethod = IronRubyViewMethodWrapper.Wrap("render_page", Content);

            script.Append(renderPageMethod);

            if (Master != null)
            {
                string renderLayoutMethod = IronRubyViewMethodWrapper.Wrap("render_layout", Master.Content);

                script.AppendLine(renderLayoutMethod);
            }
            else
            {
                script.AppendLine("def render_layout\r\n\tyield\r\nend");
            }

            script.AppendLine(string.Format(CultureInfo.InvariantCulture, MethodMissing, "view_data"));
            script.AppendLine(string.Format(CultureInfo.InvariantCulture, MethodMissing, "temp_data"));
            script.AppendLine(string.Format(CultureInfo.InvariantCulture, MethodMissing, "route_data"));
            script.AppendLine(string.Format(CultureInfo.InvariantCulture, MethodMissing, "session"));
            script.AppendLine(string.Format(CultureInfo.InvariantCulture, MethodMissing, "cache"));
            script.AppendLine(string.Format(CultureInfo.InvariantCulture, MethodMissing, "application"));

            script.AppendLine("render_layout { |content| render_page }");

            try
            {
                ScriptEngine.ExecuteScript(script.ToString(), scope);
            }
            catch (Exception e)
            {
                writer.Write(e.ToString());
            }
        }

        private sealed class Container : IViewDataContainer
        {
            internal Container(ViewDataDictionary viewData)
            {
                ViewData = viewData;
            }

            public ViewDataDictionary ViewData
            {
                get;
                set;
            }
        }
    }
}