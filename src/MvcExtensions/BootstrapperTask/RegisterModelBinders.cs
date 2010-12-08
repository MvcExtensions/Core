#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class which is used to register available <seealso cref="IModelBinder"/>.
    /// </summary>
    [DependsOn(typeof(RegisterModelBinderProvider))]
    public class RegisterModelBinders : IgnorableTypesBootstrapperTask<RegisterModelBinders, IModelBinder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterModelBinders"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RegisterModelBinders(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");

            Container = container;
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
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            Func<Type, bool> filter = type => KnownTypes.ModelBinderType.IsAssignableFrom(type) &&
                                              type.Assembly != KnownAssembly.AspNetMvcAssembly &&
                                              !IgnoredTypes.Any(ignoredType => ignoredType == type);

            Container.GetService<IBuildManager>()
                     .ConcreteTypes
                     .Where(filter)
                     .Each(type => Container.RegisterAsTransient(type));

            return TaskContinuation.Continue;
        }
    }
}