#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

using System;

namespace MvcExtensions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines a class to fluently configure metadata of a model.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class ObjectMetadataItemBuilder<TModel> : ModelMetadataItemBuilder<ObjectMetadataItem, ObjectMetadataItemBuilder<TModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectMetadataItemBuilder&lt;TModel&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public ObjectMetadataItemBuilder(ObjectMetadataItem item) : base(item)
        {
        }

        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="viewDataKey">The view data key, the value of the view data key must be a <seealso cref="IEnumerable{SelectListItem}"/>.</param>
        /// <returns></returns>
        public ObjectMetadataItemBuilder<TModel> AsDropDownList(string viewDataKey)
        {
            return AsDropDownList(viewDataKey, (Func<string>)null);
        }

        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="viewDataKey">The view data key, the value of the view data key must be a <seealso cref="IEnumerable{SelectListItem}"/>.</param>
        /// <param name="optionLabel">The option label.</param>
        /// <returns></returns>
        public ObjectMetadataItemBuilder<TModel> AsDropDownList(string viewDataKey, string optionLabel)
        {
            return AsDropDownList(viewDataKey, () => optionLabel);
        }

        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="viewDataKey">The view data key, the value of the view data key must be a <seealso cref="IEnumerable{SelectListItem}"/>.</param>
        /// <param name="optionLabel">The option label.</param>
        /// <returns></returns>
        public ObjectMetadataItemBuilder<TModel> AsDropDownList(string viewDataKey, Func<string> optionLabel)
        {
            return AsDropDownList(viewDataKey, optionLabel, "DropDownList");
        }

        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="viewDataKey">The view data key.</param>
        /// <param name="optionLabel">The option label.</param>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        public ObjectMetadataItemBuilder<TModel> AsDropDownList(string viewDataKey, string optionLabel, string template)
        {
            return AsDropDownList(viewDataKey, () => optionLabel, template);
        }

        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="viewDataKey">The view data key.</param>
        /// <param name="optionLabel">The option label.</param>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        public ObjectMetadataItemBuilder<TModel> AsDropDownList(string viewDataKey, Func<string> optionLabel, string template)
        {
            return Select(template, viewDataKey, optionLabel);
        }

        /// <summary>
        /// Marks the value to render as ListBox in edit mode.
        /// </summary>
        /// <param name="viewDataKey">The view data key, the value of the view data key must be a <seealso cref="IEnumerable{SelectListItem}"/>.</param>
        /// <returns></returns>
        public ObjectMetadataItemBuilder<TModel> AsListBox(string viewDataKey)
        {
            return AsListBox(viewDataKey, "ListBox");
        }

        /// <summary>
        /// Marks the value to render as ListBox in edit mode.
        /// </summary>
        /// <param name="viewDataKey">The view data key.</param>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        public ObjectMetadataItemBuilder<TModel> AsListBox(string viewDataKey, string template)
        {
            return Select(template, viewDataKey, null);
        }

        /// <summary>
        /// Render the value using the specified template.
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="viewDataKey">The view data key, the value of the view data key must be a <seealso cref="IEnumerable{SelectListItem}"/>.</param>
        /// <param name="optionLabel">The option label.</param>
        /// <returns></returns>
        protected virtual ObjectMetadataItemBuilder<TModel> Select(string templateName, string viewDataKey, Func<string> optionLabel)
        {
            ModelMetadataItemSelectableElementSetting selectableElementSetting = Item.AdditionalSettings
                                                                                     .OfType<ModelMetadataItemSelectableElementSetting>()
                                                                                     .FirstOrDefault();

            if (selectableElementSetting == null)
            {
                selectableElementSetting = new ModelMetadataItemSelectableElementSetting();
                Item.AdditionalSettings.Add(selectableElementSetting);
            }

            selectableElementSetting.ViewDataKey = viewDataKey;

            if (optionLabel != null)
            {
                selectableElementSetting.OptionLabel = optionLabel;
            }

            Item.TemplateName = templateName;

            return this;
        }
    }
}