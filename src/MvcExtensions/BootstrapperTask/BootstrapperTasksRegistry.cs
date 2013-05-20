#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines a Registry class which holds the list of the task that would be executed when bootstrapping the application.
    /// </summary>
    public class BootstrapperTasksRegistry : IBootstrapperTasksRegistry
    {
        private static readonly IBootstrapperTasksRegistry instance = new BootstrapperTasksRegistry();
        
        private readonly object lockObject = new object();
        private readonly IDictionary<Type, Action<object>> tasks = new Dictionary<Type, Action<object>>();

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>The instance.</value>
        public static IBootstrapperTasksRegistry Current
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        /// <value>The tasks.</value>
        [NotNull] public virtual IEnumerable<KeyValuePair<Type, Action<object>>> TaskConfigurations
        {
            get
            {
                return tasks;
            }
        }

        /// <summary>
        /// Includes the specified task.
        /// </summary>
        /// <typeparam name="TTask">The type of the task.</typeparam>
        /// <returns></returns>
        [NotNull]
        public IBootstrapperTasksRegistry Include<TTask>() where TTask : BootstrapperTask
        {
            return Include<TTask>(null);
        }

        /// <summary>
        /// Includes the specified task and also allows to configure.
        /// </summary>
        /// <typeparam name="TTask">The type of the task.</typeparam>
        /// <param name="configure">The configure.</param>
        /// <returns></returns>
        [NotNull]
        public virtual IBootstrapperTasksRegistry Include<TTask>(Action<TTask> configure) where TTask : BootstrapperTask
        {
            Action<object> modified = null;

            if (configure != null)
            {
                modified = t => configure((TTask)t);
            }

            Include(typeof(TTask), modified);

            return this;
        }

        /// <summary>
        /// Excludes this instance.
        /// </summary>
        /// <typeparam name="TTask">The type of the task.</typeparam>
        /// <returns></returns>
        [NotNull]
        public virtual IBootstrapperTasksRegistry Exclude<TTask>() where TTask : BootstrapperTask
        {
            tasks.Remove(typeof(TTask));
            return this;
        }

        private void Include([NotNull] Type type, Action<object> configure)
        {
            var requires = type.GetCustomAttributes(typeof(DependsOnAttribute), true)
                .Cast<DependsOnAttribute>()
                .Select(a => a.TaskType)
                .Distinct()
                .ToList();

            foreach (var require in requires)
            {
                Include(require, null);
            }

            if (!tasks.ContainsKey(type))
            {
                lock (lockObject)
                {
                    if (!tasks.ContainsKey(type))
                    {
                        tasks.Add(type, configure);
                    }
                }
            }
        }
    }
}
