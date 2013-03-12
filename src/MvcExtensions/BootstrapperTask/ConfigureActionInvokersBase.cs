#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web.Mvc;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines a class to configure mapping between controller activator and controller.
    /// </summary>
    [DependsOn(typeof(RegisterActionInvokers))]
    public abstract class ConfigureActionInvokersBase : ConfigurableTypeMappingBase<Controller, IActionInvoker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureActionInvokersBase"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        protected ConfigureActionInvokersBase([NotNull] TypeMappingRegistry<Controller, IActionInvoker> registry) : base(registry)
        {
        }
    }
}