#region Copyright
// Copyright (c) 2009 - 2011, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extensions for <see cref="ModelMetadataItemBuilder{TItem,TItemBuilder}"/> which add AsDropDownList and AsListBox methods 
    /// </summary>
    public static class HtmlSelectModelMetadataItemBuilderExtensions
    {
        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="viewDataKey">The view data key, the value of the view data key must be a <seealso cref="IEnumerable{T}"/>.</param>
        /// <returns></returns>
        public static TItemBuilder AsDropDownList<TItemBuilder, TItem>(this ModelMetadataItemBuilder<TItem, TItemBuilder> self, string viewDataKey)
            where TItemBuilder : ModelMetadataItemBuilder<TItem, TItemBuilder>
            where TItem : ModelMetadataItem
        {
            return self.AsDropDownList(viewDataKey, (Func<string>)null);
        }

        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="viewDataKey">The view data key, the value of the view data key must be a <seealso cref="IEnumerable{SelectListItem}"/>.</param>
        /// <param name="optionLabel">The option label.</param>
        /// <returns></returns>
        public static TItemBuilder AsDropDownList<TItemBuilder, TItem>(this ModelMetadataItemBuilder<TItem, TItemBuilder> self, string viewDataKey, string optionLabel)
            where TItemBuilder : ModelMetadataItemBuilder<TItem, TItemBuilder>
            where TItem : ModelMetadataItem
        {
            return self.AsDropDownList(viewDataKey, () => optionLabel);
        }

        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="viewDataKey">The view data key, the value of the view data key must be a <seealso cref="IEnumerable{SelectListItem}"/>.</param>
        /// <param name="optionLabel">The option label.</param>
        /// <returns></returns>
        public static TItemBuilder AsDropDownList<TItemBuilder, TItem>(this ModelMetadataItemBuilder<TItem, TItemBuilder> self, string viewDataKey, Func<string> optionLabel)
            where TItemBuilder : ModelMetadataItemBuilder<TItem, TItemBuilder>
            where TItem : ModelMetadataItem
        {
            return self.AsDropDownList(viewDataKey, optionLabel, "DropDownList");
        }

        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="viewDataKey">The view data key.</param>
        /// <param name="optionLabel">The option label.</param>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        public static TItemBuilder AsDropDownList<TItemBuilder, TItem>(this ModelMetadataItemBuilder<TItem, TItemBuilder> self, string viewDataKey, string optionLabel, string template)
            where TItemBuilder : ModelMetadataItemBuilder<TItem, TItemBuilder>
            where TItem : ModelMetadataItem
        {
            return self.AsDropDownList(viewDataKey, () => optionLabel, template);
        }

        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="viewDataKey">The view data key.</param>
        /// <param name="optionLabel">The option label.</param>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        public static TItemBuilder AsDropDownList<TItemBuilder, TItem>(this ModelMetadataItemBuilder<TItem, TItemBuilder> self, string viewDataKey, Func<string> optionLabel, string template)
            where TItemBuilder : ModelMetadataItemBuilder<TItem, TItemBuilder>
            where TItem : ModelMetadataItem
        {
            return HtmlSelect(self, template, viewDataKey, optionLabel);
        }

        /// <summary>
        /// Marks the value to render as ListBox in edit mode.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="viewDataKey">The view data key, the value of the view data key must be a <seealso cref="IEnumerable{SelectListItem}"/>.</param>
        /// <returns></returns>
        public static TItemBuilder AsListBox<TItemBuilder, TItem>(this ModelMetadataItemBuilder<TItem, TItemBuilder> self, string viewDataKey)
            where TItemBuilder : ModelMetadataItemBuilder<TItem, TItemBuilder>
            where TItem : ModelMetadataItem
        {
            return self.AsListBox(viewDataKey, "ListBox");
        }

        /// <summary>
        /// Marks the value to render as ListBox in edit mode.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="viewDataKey">The view data key.</param>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        public static TItemBuilder AsListBox<TItemBuilder, TItem>(this ModelMetadataItemBuilder<TItem, TItemBuilder> self, string viewDataKey, string template)
            where TItemBuilder : ModelMetadataItemBuilder<TItem, TItemBuilder>
            where TItem : ModelMetadataItem
        {
            return HtmlSelect(self, template, viewDataKey, null);
        }

        private static TItemBuilder HtmlSelect<TItemBuilder, TItem>(ModelMetadataItemBuilder<TItem, TItemBuilder> self, string templateName, string viewDataKey, Func<string> optionLabel)
            where TItemBuilder : ModelMetadataItemBuilder<TItem, TItemBuilder>
            where TItem : ModelMetadataItem
        {
            var selectableElementSetting = self.Item.GetAdditionalSettingOrCreateNew<ModelMetadataItemSelectableElementSetting>();

            selectableElementSetting.ViewDataKey = viewDataKey;

            if (optionLabel != null)
            {
                selectableElementSetting.OptionLabel = optionLabel;
            }

            self.Item.TemplateName = templateName;

            return (TItemBuilder)self;
        }
    }
}