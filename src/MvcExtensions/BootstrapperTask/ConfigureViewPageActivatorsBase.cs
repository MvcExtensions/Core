#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class to configure mapping between view page activator and view.
    /// </summary>
    [DependsOn(typeof(RegisterViewPageActivator))]
    public abstract class ConfigureViewPageActivatorsBase : ConfigurableTypeMappingBase<IView, IViewPageActivator>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureViewPageActivatorsBase"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        protected ConfigureViewPageActivatorsBase(TypeMappingRegistry<IView, IViewPageActivator> registry) : base(registry)
        {
        }
    }
}