#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.IronRuby
{
    using System.IO;
    using System.Web.Mvc;

    using Microsoft.Scripting.Hosting;

    using Ruby = global::IronRuby.Ruby;

    /// <summary>
    /// Iron Ruby View Engine.
    /// </summary>
    public class IronRubyViewEngine : VirtualPathProviderViewEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IronRubyViewEngine"/> class.
        /// </summary>
        public IronRubyViewEngine() : this(CreateScriptEngine())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IronRubyViewEngine"/> class.
        /// </summary>
        /// <param name="scriptEngine">The script engine.</param>
        protected IronRubyViewEngine(IScriptEngine scriptEngine)
        {
            MasterLocationFormats = new[]
                                        {
                                            "~/Views/{1}/{0}.layout.erb",
                                            "~/Views/Shared/{0}.layout.erb"
                                        };

            AreaMasterLocationFormats = new[]
                                            {
                                                "~/Areas/{2}/Views/{1}/{0}.layout.erb",
                                                "~/Areas/{2}/Views/Shared/{0}.layout.erb",
                                            };

            ViewLocationFormats = new[]
                                      {
                                          "~/Views/{1}/{0}.html.erb",
                                          "~/Views/Shared/{0}.html.erb"
                                      };

            AreaViewLocationFormats = new[]
                                          {
                                              "~/Areas/{2}/Views/{1}/{0}.html.erb",
                                              "~/Areas/{2}/Views/Shared/{0}.html.erb"
                                          };

            PartialViewLocationFormats = ViewLocationFormats;
            AreaPartialViewLocationFormats = AreaViewLocationFormats;

            ScriptEngine = scriptEngine;
        }

        /// <summary>
        /// Gets or sets the script engine.
        /// </summary>
        /// <value>The script engine.</value>
        protected IScriptEngine ScriptEngine
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates the specified partial view by using the specified controller context.
        /// </summary>
        /// <returns>
        /// A reference to the partial view.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="partialPath">The partial path for the new partial view.</param>
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return CreateView(partialPath, null);
        }

        /// <summary>
        /// Creates the specified view by using the controller context, path of the view, and path of the master view.
        /// </summary>
        /// <returns>
        /// A reference to the view.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="viewPath">The path of the view.</param><param name="masterPath">The path of the master view.</param>
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return CreateView(viewPath, CreateView(masterPath, null));
        }

        private static IScriptEngine CreateScriptEngine()
        {
            ScriptRuntimeSetup runtimeSetup = new ScriptRuntimeSetup();
            runtimeSetup.LanguageSetups.Add(Ruby.CreateRubySetup());

            ScriptRuntime runtime = Ruby.CreateRuntime(runtimeSetup);

            return new IronRubyScriptEngine(Ruby.GetEngine(runtime));
        }

        private string ReadViewContent(string viewPath)
        {
            string content;

            using (Stream stream = VirtualPathProvider.GetFile(viewPath).Open())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    content = reader.ReadToEnd();
                }
            }

            return content;
        }

        private IronRubyView CreateView(string viewPath, IronRubyView masterView)
        {
            if (string.IsNullOrEmpty(viewPath))
            {
                return null;
            }

            string viewContent = ReadViewContent(viewPath);

            return new IronRubyView(viewContent, ScriptEngine, masterView);
        }
    }
}