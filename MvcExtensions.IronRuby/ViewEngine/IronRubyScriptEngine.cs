#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.IronRuby
{
    using System;

    using Microsoft.Scripting.Hosting;

    /// <summary>
    /// The Iron Ruby script engine. 
    /// </summary>
    public class IronRubyScriptEngine : IScriptEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IronRubyScriptEngine"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        public IronRubyScriptEngine(ScriptEngine engine)
        {
            Engine = engine;
        }

        /// <summary>
        /// Gets or sets the engine.
        /// </summary>
        /// <value>The engine.</value>
        protected ScriptEngine Engine
        {
            get;
            private set;
        }

        /// <summary>
        /// Executes the in scope.
        /// </summary>
        /// <param name="block">The block.</param>
        public void ExecuteInScope(Action<ScriptScope> block)
        {
            ScriptScope scope = Engine.CreateScope();

            block(scope);
        }

        /// <summary>
        /// Executes the script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="scope">The scope.</param>
        /// <returns></returns>
        public object ExecuteScript(string script, ScriptScope scope)
        {
            return Engine.Execute(script, scope);
        }
    }
}