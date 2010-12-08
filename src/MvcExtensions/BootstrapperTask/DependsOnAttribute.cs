#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;

    /// <summary>
    /// Defined an attribute which is used to mark the depended tasks
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DependsOnAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependsOnAttribute"/> class.
        /// </summary>
        /// <param name="taskType">Type of the task.</param>
        public DependsOnAttribute(Type taskType)
        {
            Invariant.IsNotNull(taskType, "taskType");

            if (!KnownTypes.BootstrapperTaskType.IsAssignableFrom(taskType))
            {
                throw new ArgumentException(string.Format(Culture.CurrentUI, ExceptionMessages.IncorrectTypeMustBeDescended, KnownTypes.BootstrapperTaskType.FullName), "taskType");
            }

            TaskType = taskType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependsOnAttribute"/> class.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        public DependsOnAttribute(string typeName) : this(Type.GetType(typeName, true, true))
        {
        }

        /// <summary>
        /// Gets or sets the type of the task.
        /// </summary>
        /// <value>The type of the task.</value>
        public Type TaskType { get; private set; }
    }
}