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
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web.Compilation;

    /// <summary>
    /// Defines a wrapper class which provides access to the internal <seealso cref="BuildManager"/>.
    /// </summary>
    public class BuildManagerWrapper : IBuildManager
    {
        private static readonly IBuildManager instance = new BuildManagerWrapper();
        private IEnumerable<Assembly> referencedAssemblies;
        private IEnumerable<Type> publicTypes;
        private IEnumerable<Type> concreteTypes;

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>The current.</value>
        public static IBuildManager Current
        {
            [DebuggerStepThrough]
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        /// <value>The assemblies.</value>
        public virtual IEnumerable<Assembly> Assemblies
        {
            [DebuggerStepThrough]
            get
            {
                return referencedAssemblies ?? (referencedAssemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().Where(assembly => !assembly.GlobalAssemblyCache).ToList());
            }
        }

        /// <summary>
        /// Gets the available public types of <see cref="Assemblies"/>.
        /// </summary>
        /// <value>The concrete types.</value>
        public IEnumerable<Type> PublicTypes
        {
            get
            {
                return publicTypes ?? (publicTypes = Assemblies.PublicTypes().ToList());
            }
        }

        /// <summary>
        /// Gets the available concrete types of <see cref="Assemblies"/>.
        /// </summary>
        /// <value>The concrete types.</value>
        public IEnumerable<Type> ConcreteTypes
        {
            [DebuggerStepThrough]
            get
            {
                return concreteTypes ?? (concreteTypes = Assemblies.ConcreteTypes().ToList());
            }
        }
    }
}