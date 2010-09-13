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
    using System.Linq.Expressions;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class to store the <see cref="FilterAttribute"/> factories of <seealso cref="Controller"/> action method.
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    public class FilterRegistryActionItem<TController> : FilterRegistryItem where TController : Controller
    {
        private readonly ReflectedActionDescriptor reflectedActionDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegistryActionItem&lt;TController&gt;"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="filters">The filters.</param>
        public FilterRegistryActionItem(Expression<Action<TController>> action, IEnumerable<Func<FilterAttribute>> filters) : base(filters)
        {
            Invariant.IsNotNull(action, "action");

            MethodCallExpression methodCall = action.Body as MethodCallExpression;

            if ((methodCall == null) || !KnownTypes.ActionResultType.IsAssignableFrom(methodCall.Method.ReturnType))
            {
                throw new ArgumentException(ExceptionMessages.TheExpressionMustBeAValidControllerAction, "action");
            }

            reflectedActionDescriptor = new ReflectedActionDescriptor(methodCall.Method, methodCall.Method.Name, new ReflectedControllerDescriptor(methodCall.Object.Type));
        }

        /// <summary>
        /// Determines whether the specified controller context is matching.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>
        /// <c>true</c> if the specified controller context is matching; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsMatching(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            if ((controllerContext != null) && (actionDescriptor != null))
            {
                ReflectedActionDescriptor matchingDescriptor = actionDescriptor as ReflectedActionDescriptor;

                return (matchingDescriptor != null) ?
                       reflectedActionDescriptor.MethodInfo == matchingDescriptor.MethodInfo :
                       IsSameAction(reflectedActionDescriptor, actionDescriptor);
            }

            return false;
        }

        private static bool IsSameAction(ActionDescriptor descriptor1, ActionDescriptor descriptor2)
        {
            ParameterDescriptor[] parameters1 = descriptor1.GetParameters();
            ParameterDescriptor[] parameters2 = descriptor2.GetParameters();

            bool same = descriptor1.ControllerDescriptor.ControllerName.Equals(descriptor2.ControllerDescriptor.ControllerName, StringComparison.OrdinalIgnoreCase) &&
                        descriptor1.ActionName.Equals(descriptor2.ActionName, StringComparison.OrdinalIgnoreCase) &&
                        (parameters1.Length == parameters2.Length);

            if (same)
            {
                for (int i = parameters1.Length - 1; i >= 0; i--)
                {
                    if (parameters1[i].ParameterType == parameters2[i].ParameterType)
                    {
                        continue;
                    }

                    same = false;
                    break;
                }
            }

            return same;
        }
    }
}