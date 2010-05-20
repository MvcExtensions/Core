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
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class which is used to register available <seealso cref="ValueProviderFactory"/>.
    /// </summary>
    public class RegisterValueProviderFactories : BootstrapperTask
    {
        private static readonly IList<Type> ignoredTypes = new List<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterValueProviderFactories"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="valueProviderFactories">The value provider factories.</param>
        public RegisterValueProviderFactories(ContainerAdapter container, ValueProviderFactoryCollection valueProviderFactories)
        {
            Invariant.IsNotNull(container, "container");
            Invariant.IsNotNull(valueProviderFactories, "valueProviderFactories");

            Container = container;
            ValueProviderFactories = valueProviderFactories;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RegisterValueProviderFactories"/> should be excluded.
        /// </summary>
        /// <value><c>true</c> if excluded; otherwise, <c>false</c>.</value>
        public static bool Excluded
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the ignored value provider factory types.
        /// </summary>
        /// <value>The ignored types.</value>
        public static ICollection<Type> IgnoredTypes
        {
            [DebuggerStepThrough]
            get
            {
                return ignoredTypes;
            }
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        protected ContainerAdapter Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the value provider factories.
        /// </summary>
        /// <value>The value provider factories.</value>
        protected ValueProviderFactoryCollection ValueProviderFactories
        { 
            get;
            private set;
        }

        /// <summary>
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            if (!Excluded)
            {
                Func<Type, bool> filter = type => KnownTypes.ValueProviderFactoryType.IsAssignableFrom(type) &&
                                                  type.Assembly != KnownAssembly.AspNetMvcAssembly &&
                                                  !IgnoredTypes.Any(ignoredType => ignoredType == type) &&
                                                  !ValueProviderFactories.Any(factory => factory.GetType() == type);

                Container.GetInstance<IBuildManager>()
                         .ConcreteTypes
                         .Where(filter)
                         .Each(type => Container.RegisterAsSingleton(KnownTypes.ValueProviderFactoryType, type));

                Container.GetAllInstances<ValueProviderFactory>()
                         .Each(factory => ValueProviderFactories.Add(factory));
            }

            return TaskContinuation.Continue;
        }
    }
}