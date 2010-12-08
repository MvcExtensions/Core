#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Web.Mvc;

    internal static class KnownTypes
    {
        public static readonly Type BindingAttributeType = typeof(BindingTypeAttribute);

        public static readonly Type BootstrapperTaskType = typeof(BootstrapperTask);

        public static readonly Type PerRequestTaskType = typeof(PerRequestTask);

        public static readonly Type ModelBinderType = typeof(IModelBinder);

        public static readonly Type ControllerActivatorType = typeof(IControllerActivator);

        public static readonly Type ControllerType = typeof(Controller);

        public static readonly Type ActionInvokerType = typeof(IActionInvoker);

        public static readonly Type DefaultActionInvokerType = typeof(ExtendedControllerActionInvoker);

        public static readonly Type AsyncActionInvokerType = typeof(ExtendedAsyncControllerActionInvoker);

        public static readonly Type FilterType = typeof(IMvcFilter);

        public static readonly Type FilterAttributeType = typeof(FilterAttribute);

        public static readonly Type FilterProviderType = typeof(IFilterProvider);

        public static readonly Type ViewPageActivatorType = typeof(IViewPageActivator);

        public static readonly Type ViewType = typeof(IView);

        public static readonly Type ViewEngineType = typeof(IViewEngine);

        public static readonly Type ActionResultType = typeof(ActionResult);

        public static readonly Type ValueProviderFactoryType = typeof(ValueProviderFactory);

        public static readonly Type ModelMetadataConfigurationType = typeof(IModelMetadataConfiguration);
    }
}