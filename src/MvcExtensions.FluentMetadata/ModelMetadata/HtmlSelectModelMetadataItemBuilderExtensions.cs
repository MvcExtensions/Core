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
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    /// <summary>
    /// Extensions for <see cref="ModelMetadataItemBuilder{TValue}"/> which add AsDropDownList and AsListBox methods 
    /// </summary>
    [TypeForwardedFrom(KnownAssembly.MvcExtensions)]
    public static class HtmlSelectModelMetadataItemBuilderExtensions
    {
        /// <summary>
        /// Marks the value to render as DropDownList element in edit mode.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="viewDataKey">The view data key, the value of the view data key must be a <seealso cref="IEnumerable{T}"/>.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<TValue> AsDropDownList<TValue>([NotNull] this ModelMetadataItemBuilder<TValue> self, string viewDataKey)
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
        [NotNull]
        public static ModelMetadataItemBuilder<TValue> AsDropDownList<TValue>([NotNull] this ModelMetadataItemBuilder<TValue> self, string viewDataKey, string optionLabel)
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
        [NotNull]
        public static ModelMetadataItemBuilder<TValue> AsDropDownList<TValue>([NotNull] this ModelMetadataItemBuilder<TValue> self, string viewDataKey, Func<string> optionLabel)
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
        [NotNull]
        public static ModelMetadataItemBuilder<TValue> AsDropDownList<TValue>([NotNull] this ModelMetadataItemBuilder<TValue> self, string viewDataKey, string optionLabel, [AspMvcEditorTemplate, AspMvcDisplayTemplate]string template)
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
        [NotNull]
        public static ModelMetadataItemBuilder<TValue> AsDropDownList<TValue>([NotNull] this ModelMetadataItemBuilder<TValue> self, string viewDataKey, Func<string> optionLabel, [AspMvcEditorTemplate, AspMvcDisplayTemplate]string template)
        {
            return HtmlSelect(self, template, viewDataKey, optionLabel);
        }

        /// <summary>
        /// Marks the value to render as ListBox in edit mode.
        /// </summary>
        /// <param name="self">The instance.</param>
        /// <param name="viewDataKey">The view data key, the value of the view data key must be a <seealso cref="IEnumerable{SelectListItem}"/>.</param>
        /// <returns></returns>
        [NotNull]
        public static ModelMetadataItemBuilder<TValue> AsListBox<TValue>([NotNull] this ModelMetadataItemBuilder<TValue> self, string viewDataKey)
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
        [NotNull]
        public static ModelMetadataItemBuilder<TValue> AsListBox<TValue>([NotNull] this ModelMetadataItemBuilder<TValue> self, string viewDataKey, [AspMvcEditorTemplate, AspMvcDisplayTemplate]string template)
        {
            return HtmlSelect(self, template, viewDataKey, null);
        }

        [NotNull]
        private static ModelMetadataItemBuilder<TValue> HtmlSelect<TValue>([NotNull] ModelMetadataItemBuilder<TValue> self, string templateName, string viewDataKey, Func<string> optionLabel)
        {
            self.AddAction(
                m =>
                {
                    var selectableElementSetting = m.GetAdditionalSettingOrCreateNew<ModelMetadataItemSelectableElementSetting>();

                    selectableElementSetting.ViewDataKey = viewDataKey;

                    if (optionLabel != null)
                    {
                        selectableElementSetting.OptionLabel = optionLabel;
                    }

                    m.TemplateName = templateName;
                });

            return self;
        }
    }
}