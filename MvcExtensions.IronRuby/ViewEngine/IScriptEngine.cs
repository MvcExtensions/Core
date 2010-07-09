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
    /// Defines an interface 
    /// </summary>
    public interface IScriptEngine
    {
        /// <summary>
        /// Executes the in scope.
        /// </summary>
        /// <param name="block">The block.</param>
        void ExecuteInScope(Action<ScriptScope> block);

        /// <summary>
        /// Executes the script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="scope">The scope.</param>
        /// <returns></returns>
        object ExecuteScript(string script, ScriptScope scope);
    }
}