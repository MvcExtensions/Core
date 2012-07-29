#region Copyright
// Copyright (c) 2009 - 2011, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    /// <summary>
    /// RemoteValidationConfigurator class implements methods to configure remote validation
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class RemoteValidationConfigurator<TValue> : AbstractRemoteValidationConfigurator<TValue>
    {
        private readonly Func<string> errorMessage;
        private readonly string errorMessageResourceName;
        private readonly Type errorMessageResourceType;
        private string httpMethod;

        internal RemoteValidationConfigurator(ModelMetadataItemBuilder<TValue> modelMetadataItemBuilder, Func<string> errorMessage, string errorMessageResourceName, Type errorMessageResourceType)
        {
            this.errorMessage = errorMessage;
            this.errorMessageResourceName = errorMessageResourceName;
            this.errorMessageResourceType = errorMessageResourceType;
            Core.ModelMetadataItemBuilder = modelMetadataItemBuilder;
        }

        private IRemoteValidationConfigurator<TValue> Core
        {
            get { return this; }
        }

        /// <summary>
        /// HttpMethod. The Default one of GET
        /// </summary>
        /// <param name="method">Http method, e.g., Get, Post</param>
        /// <returns></returns>
        public RemoteValidationConfigurator<TValue> HttpMethod(string method)
        {
            httpMethod = method;
            return this;
        }

        /// <summary>
        /// Register Remote validator for the controller and specified action; 
        /// additional fields will be added automatically based on method signature
        /// </summary>
        /// <param name="action">Action to call by validator 
        /// (additional fields will be added automatically based on method signature (the argument names will be capitilized))
        /// </param>
        /// <param name="areaName">The name of area</param>
        /// <typeparam name="TController">Target controller to find the action</typeparam>
        /// <returns></returns>
        public AbstractRemoteValidationConfigurator<TValue> For<TController>(Expression<Func<TController, ActionResult>> action, string areaName = null)
            where TController : IController
        {
            var methodCall = (MethodCallExpression)action.Body;
            var parameters = methodCall.Method.GetParameters();
            var additionalFields = parameters.Length > 1 ? parameters.Select(p => Capitalize(p.Name)) : Enumerable.Empty<string>();

            CreateRemoteValidation(action, areaName, null, additionalFields);
            return this;
        }

        /// <summary>
        /// Register Remote validator for the controller and specified action
        /// </summary>
        /// <param name="action">Action to call by validator
        /// (additional fields will be added automatically based on method signature (the argument names will be capitilized))</param>
        /// <param name="additionalFields"> </param>
        /// <param name="areaName"> </param>
        /// <typeparam name="TController">Target controller to find the action</typeparam>
        /// <returns></returns>
        public AbstractRemoteValidationConfigurator<TValue> For<TController>(Expression<Func<TController, ActionResult>> action, string areaName, IEnumerable<string> additionalFields)
            where TController : IController
        {
            CreateRemoteValidation(action, areaName, null, additionalFields);
            return this;
        }

        /// <summary>
        /// Register Remote validator for the controller and specified action
        /// </summary>
        /// <param name="action">Action to call by validator</param>
        /// <param name="additionalFields"> </param>
        /// <typeparam name="TController">Target controller to find the action</typeparam>
        /// <returns></returns>
        public AbstractRemoteValidationConfigurator<TValue> For<TController>(Expression<Func<TController, ActionResult>> action, IEnumerable<string> additionalFields)
            where TController : IController
        {
            CreateRemoteValidation(action, null, null, additionalFields);
            return this;
        }

        /// <summary>
        /// Register Remote validator for the controller and specified action
        /// </summary>
        /// <param name="action">Action to call by validator</param>
        /// <typeparam name="TController">Target controller to find the action</typeparam>
        /// <returns><see cref="AbstractRemoteValidationConfigurator{TValue}"/></returns>
        public AbstractRemoteValidationConfigurator<TValue> For<TController>(Expression<Func<TController, Func<TValue, ActionResult>>> action)
            where TController : IController
        {
            return For(action, null);
        }

        /// <summary>
        /// Register Remote validator for the controller and specified action
        /// </summary>
        /// <param name="action">Action to call by validator</param>
        /// <param name="areaName">The name of area</param>
        /// <typeparam name="TController">Target controller to find the action</typeparam>
        /// <returns><see cref="AbstractRemoteValidationConfigurator{TValue}"/></returns>
        public AbstractRemoteValidationConfigurator<TValue> For<TController>(Expression<Func<TController, Func<TValue, ActionResult>>> action, string areaName)
            where TController : IController
        {
            CreateRemoteValidation(action, areaName, null, Enumerable.Empty<string>());
            return this;
        }

        /// <summary>
        /// Register Remote validator by the controller name and action name
        /// </summary>
        /// <param name="controller">The name of controller</param>
        /// <param name="action">The name of action</param>
        /// <returns><see cref="AbstractRemoteValidationConfigurator{TValue}"/></returns>
        public AbstractRemoteValidationConfigurator<TValue> For(string controller, string action)
        {
            return For(controller, action, Enumerable.Empty<string>());
        }

        /// <summary>
        /// Register Remote validator by the controller name and action name
        /// </summary>
        /// <param name="controller">The name of controller</param>
        /// <param name="action">The name of action</param>
        /// <param name="areaName">The name of area</param>
        /// <returns><see cref="AbstractRemoteValidationConfigurator{TValue}"/></returns>
        public AbstractRemoteValidationConfigurator<TValue> For(string controller, string action, string areaName)
        {
            return For(controller, action, areaName, Enumerable.Empty<string>());
        }

        /// <summary>
        /// Register Remote validator by the controller name and action name
        /// </summary>
        /// <param name="controller">The name of controller</param>
        /// <param name="action">The name of action</param>
        /// <param name="additionalFields">The additional fields</param>
        /// <returns><see cref="AbstractRemoteValidationConfigurator{TValue}"/></returns>
        public AbstractRemoteValidationConfigurator<TValue> For(string controller, string action, IEnumerable<string> additionalFields)
        {
            return For(controller, action, null, additionalFields);
        }

        /// <summary>
        /// Register Remote validator by the controller name and action name
        /// </summary>
        /// <param name="controller">The name of controller</param>
        /// <param name="action">The name of action</param>
        /// <param name="areaName">The name of area</param>
        /// <param name="additionalFields">The additional fields</param>
        /// <returns><see cref="AbstractRemoteValidationConfigurator{TValue}"/></returns>
        public AbstractRemoteValidationConfigurator<TValue> For(string controller, string action, string areaName, IEnumerable<string> additionalFields)
        {
            CreateRemoteValidation(controller, action, areaName, null, additionalFields);
            return this;
        }

        /// <summary>
        /// Register Remote validator by the route name
        /// </summary>
        /// <param name="routeName">The name of the route</param>
        /// <returns><see cref="AbstractRemoteValidationConfigurator{TValue}"/></returns>
        public AbstractRemoteValidationConfigurator<TValue> For(string routeName)
        {
            return For(routeName, Enumerable.Empty<string>());
        }

        /// <summary>
        /// Register Remote validator by the route name
        /// </summary>
        /// <param name="routeName">The name of the route</param>
        /// <param name="additionalFields">The additional fields</param>
        /// <returns><see cref="AbstractRemoteValidationConfigurator{TValue}"/></returns>
        public AbstractRemoteValidationConfigurator<TValue> For(string routeName, IEnumerable<string> additionalFields)
        {
            CreateRemoteValidation(null, null, null, routeName, additionalFields);
            return this;
        }

        private static string Capitalize(string name)
        {
            return name.Length > 1 ? char.ToUpperInvariant(name[0]) + name.Substring(1) : name.ToUpper();
        }

        private void CreateRemoteValidation<TController, TParam>(Expression<Func<TController, Func<TParam, ActionResult>>> action, string areaName, string routeName, IEnumerable<string> additionalFields)
            where TController : IController
        {
            var controller = typeof(TController).Name.Replace("Controller", string.Empty);
            var name = new ExpressionVisitorHelper().GetMethod(action).Name;
            CreateRemoteValidation(controller, name, areaName, routeName, additionalFields);
        }

        private void CreateRemoteValidation<TController>(Expression<Func<TController, ActionResult>> action, string areaName, string routeName, IEnumerable<string> additionalFields)
            where TController : IController
        {
            var controller = typeof(TController).Name.Replace("Controller", string.Empty);
            var methodCall = (MethodCallExpression)action.Body;
            var actionName = methodCall.Method.Name;
            CreateRemoteValidation(controller, actionName, areaName, routeName, additionalFields);
        }

        private void CreateRemoteValidation(string controller, string action, string areaName, string routeName, IEnumerable<string> additionalFields)
        {
            var self = Core.ModelMetadataItemBuilder;
            var validation = self.Item.GetValidationOrCreateNew<RemoteValidationMetadata>();

            validation.ErrorMessage = errorMessage;
            validation.Action = action;
            validation.Controller = controller;
            validation.RouteName = routeName;
            validation.Area = areaName;
            validation.AdditionalFields = string.Join(",", additionalFields);
            validation.HttpMethod = httpMethod;
            validation.ErrorMessageResourceType = errorMessageResourceType;
            validation.ErrorMessageResourceName = errorMessageResourceName;
        }
    }
}
