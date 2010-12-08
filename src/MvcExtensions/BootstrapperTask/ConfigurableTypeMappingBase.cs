#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    /// <summary>
    /// Defines a class to configure type mapping.
    /// </summary>
    /// <typeparam name="TType1">The type of the type1.</typeparam>
    /// <typeparam name="TType2">The type of the type2.</typeparam>
    public abstract class ConfigurableTypeMappingBase<TType1, TType2> : BootstrapperTask where TType1 : class where TType2 : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurableTypeMappingBase{TType1,TType2}"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        protected ConfigurableTypeMappingBase(TypeMappingRegistry<TType1, TType2> registry)
        {
            Invariant.IsNotNull(registry, "registry");

            Registry = registry;
        }

        /// <summary>
        /// Gets or sets the registry.
        /// </summary>
        /// <value>The registry.</value>
        protected TypeMappingRegistry<TType1, TType2> Registry
        {
            get;
            private set;
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            Configure();

            return TaskContinuation.Continue;
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        protected abstract void Configure();
    }
}