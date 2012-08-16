#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web.Mvc;

    /// <summary>
    /// Extensions for <see cref="ModelMetadata"/> and <see cref="ViewDataDictionary"/> which add ability to retrive <see cref="RenderActionSetting"/> 
    /// </summary>
    public static class RenderActionSettingExtensions
    {
        /// <summary>
        /// Retrives the <see cref="RenderActionSetting"/> from the <see cref="ModelMetadata"/>
        /// </summary>
        /// <param name="modelMetadata"> The model metadata </param>
        /// <returns></returns>
        public static RenderActionSetting GetRenderActionSetting(this ModelMetadata modelMetadata)
        {
            var extendedModelMetadata = modelMetadata as ExtendedModelMetadata;
            if (extendedModelMetadata == null)
            {
                return new RenderActionSetting();
            }

            return extendedModelMetadata.Metadata.GetAdditionalSettingOrCreateNew<RenderActionSetting>();
        }

        /// <summary>
        /// Retrives the <see cref="RenderActionSetting"/> from the <see cref="ViewDataDictionary"/>
        /// </summary>
        /// <param name="viewData"> The view data dicionary </param>
        /// <returns></returns>
        public static RenderActionSetting GetRenderActionSetting(this ViewDataDictionary viewData)
        {
            Invariant.IsNotNull(viewData, "viewData");

            return GetRenderActionSetting(viewData.ModelMetadata);
        }
    }
}