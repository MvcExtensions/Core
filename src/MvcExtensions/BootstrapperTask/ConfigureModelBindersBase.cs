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
    /// Defines a class to configure mapping between model binder and model.
    /// </summary>
    [DependsOn(typeof(RegisterModelBinders))]
    public abstract class ConfigureModelBindersBase : ConfigurableTypeMappingBase<object, IModelBinder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureModelBindersBase"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        protected ConfigureModelBindersBase(TypeMappingRegistry<object, IModelBinder> registry) : base(registry)
        {
        }
    }
}