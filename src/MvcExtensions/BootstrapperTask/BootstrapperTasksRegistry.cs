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
    using System.Linq;

    /// <summary>
    /// Defines a Registry class which holds the list of the task that would be executed when bootstrapping the application.
    /// </summary>
    public class BootstrapperTasksRegistry : IBootstrapperTasksRegistry
    {
        private static readonly IBootstrapperTasksRegistry instance = new BootstrapperTasksRegistry();

        private readonly IList<TaskConfiguration> tasks = new List<TaskConfiguration>();

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>The instance.</value>
        public static IBootstrapperTasksRegistry Current
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        /// <value>The tasks.</value>
        public virtual IEnumerable<KeyValuePair<Type, Action<object>>> TaskConfigurations
        {
            get
            {
                return tasks.Select(configuration => new KeyValuePair<Type, Action<object>>(configuration.TaskType, configuration.Configure));
            }
        }

        /// <summary>
        /// Includes the specified task.
        /// </summary>
        /// <typeparam name="TTask">The type of the task.</typeparam>
        /// <returns></returns>
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
        public virtual IBootstrapperTasksRegistry Exclude<TTask>() where TTask : BootstrapperTask
        {
            Type taskType = typeof(TTask);

            IEnumerable<TaskConfiguration> taskConfigurations = tasks.Where(c => c.TaskType == taskType).ToList();

            foreach (TaskConfiguration taskConfiguration in taskConfigurations)
            {
                tasks.Remove(taskConfiguration);
            }

            return this;
        }

        private void Include(Type type, Action<object> configure)
        {
            IEnumerable<Type> requires = type.GetCustomAttributes(typeof(DependsOnAttribute), true)
                                             .Cast<DependsOnAttribute>()
                                             .Select(a => a.TaskType)
                                             .Distinct()
                                             .ToList();

            foreach (Type require in requires)
            {
                Include(require, null);
            }

            if (!tasks.Any(c => c.TaskType == type))
            {
                tasks.Add(new TaskConfiguration { TaskType = type, Configure = configure });
            }
        }

        private sealed class TaskConfiguration
        {
            public Type TaskType { get; set; }

            public Action<object> Configure { get; set; }
        }
    }
}