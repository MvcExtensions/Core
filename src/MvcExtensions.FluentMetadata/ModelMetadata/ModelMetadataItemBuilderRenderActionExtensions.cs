#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;

    /// <summary>
    /// Extensions for <see cref="ModelMetadataItemBuilder{TValue}"/> which add RenderAction methods.
    /// </summary>
    public static class ModelMetadataItemBuilderRenderActionExtensions
    {
        /// <summary>
        /// Marks the value to render partial action.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="action">The child action which should be rendered.</param>
        /// <typeparam name="TModel">Type of the model.</typeparam>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<TModel> RenderAction<TModel>(this ModelMetadataItemBuilder<TModel> self, Func<HtmlHelper, IHtmlString> action)
        {
            self.Template("RenderAction");

            var settings = self.Item.GetAdditionalSettingOrCreateNew<RenderActionSetting>();
            settings.Action = action;

            return self;
        }

        /// <summary>
        /// Marks the value to render partial action.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="actionName">The name of the action which should be rendered.</param>
        /// <typeparam name="TModel">Type of the model.</typeparam>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<TModel> RenderAction<TModel>(this ModelMetadataItemBuilder<TModel> self, string actionName)
        {
            return RenderAction(self, html => html.Action(actionName));
        }

        /// <summary>
        /// Marks the value to render partial action.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="actionName">The name of the action which should be rendered.</param>
        /// <param name="controllerName">The name of the controller that contains the action.</param>
        /// <typeparam name="TModel">Type of the model.</typeparam>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<TModel> RenderAction<TModel>(this ModelMetadataItemBuilder<TModel> self, string actionName, string controllerName)
        {
            return RenderAction(self, html => html.Action(actionName, controllerName));
        }

        /// <summary>
        /// Marks the value to render partial action.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="actionName">The name of the action which should be rendered.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. You can use <paramref name="routeValues"/> to provide the parameters that are bound to the action method parameters. The <paramref name="routeValues"/> parameter is merged with the original route values and overrides them.</param>
        /// <typeparam name="TModel">Type of the model.</typeparam>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<TModel> RenderAction<TModel>(this ModelMetadataItemBuilder<TModel> self, string actionName, object routeValues)
        {
            return RenderAction(self, html => html.Action(actionName, routeValues));
        }

        /// <summary>
        /// Marks the value to render partial action.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="actionName">The name of the action which should be rendered.</param>
        /// <param name="controllerName">The name of the controller that contains the action.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. You can use <paramref name="routeValues"/> to provide the parameters that are bound to the action method parameters. The <paramref name="routeValues"/> parameter is merged with the original route values and overrides them.</param>
        /// <typeparam name="TModel">Type of the model.</typeparam>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<TModel> RenderAction<TModel>(this ModelMetadataItemBuilder<TModel> self, string actionName, string controllerName, object routeValues)
        {
            return RenderAction(self, html => html.Action(actionName, controllerName, routeValues));
        }

        /// <summary>
        /// Marks the value to render partial action.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="actionName">The name of the action which should be rendered.</param>
        /// <param name="routeValues">A dictionary that contains the parameters for a route. You can use <paramref name="routeValues"/> to provide the parameters that are bound to the action method parameters. The <paramref name="routeValues"/> parameter is merged with the original route values and overrides them.</param>
        /// <typeparam name="TModel">Type of the model.</typeparam>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<TModel> RenderAction<TModel>(this ModelMetadataItemBuilder<TModel> self, string actionName, RouteValueDictionary routeValues)
        {
            return RenderAction(self, html => html.Action(actionName, routeValues));
        }

        /// <summary>
        /// Marks the value to render partial action.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="actionName">The name of the action which should be rendered.</param>
        /// <param name="controllerName">The name of the controller that contains the action.</param>
        /// <param name="routeValues">A dictionary that contains the parameters for a route. You can use <paramref name="routeValues"/> to provide the parameters that are bound to the action method parameters. The <paramref name="routeValues"/> parameter is merged with the original route values and overrides them.</param>
        /// <typeparam name="TModel">Type of the model.</typeparam>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<TModel> RenderAction<TModel>(this ModelMetadataItemBuilder<TModel> self, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            return RenderAction(self, html => html.Action(actionName, controllerName, routeValues));
        }
    }
}