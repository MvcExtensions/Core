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
    /// Defines a class to configure mapping between controller activator and controller.
    /// </summary>
    [DependsOn(typeof(RegisterControllerActivator))]
    public abstract class ConfigureControllerActivatorsBase : ConfigurableTypeMappingBase<Controller, IControllerActivator>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureControllerActivatorsBase"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        protected ConfigureControllerActivatorsBase(TypeMappingRegistry<Controller, IControllerActivator> registry) : base(registry)
        {
        }
    }
}