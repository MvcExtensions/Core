#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>, AlexBar <abarbashin@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public static class RemoteValidationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<TValue> Remote<TValue>(this ModelMetadataItemBuilder<TValue> self,
                                                                      Func<RemoteValidationConfigurator<TValue>, AbstractRemoteValidationConfigurator<TValue>> configure)
        {
            return Remote(self, configure, null, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configure"></param>
        /// <param name="errorMessage"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<TValue> Remote<TValue>(this ModelMetadataItemBuilder<TValue> self,
                                                                      Func<RemoteValidationConfigurator<TValue>, AbstractRemoteValidationConfigurator<TValue>> configure, string errorMessage)
        {
            return Remote(self, configure, () => errorMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configure"></param>
        /// <param name="errorMessage"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<TValue> Remote<TValue>(this ModelMetadataItemBuilder<TValue> self,
                                                                      Func<RemoteValidationConfigurator<TValue>, AbstractRemoteValidationConfigurator<TValue>> configure, Func<string> errorMessage)
        {
            return Remote(self, configure, errorMessage, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configure"></param>
        /// <param name="errorMessageResourceName"></param>
        /// <param name="errorMessageResourceType"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static ModelMetadataItemBuilder<TValue> Remote<TValue>(this ModelMetadataItemBuilder<TValue> self,
                                                                      Func<RemoteValidationConfigurator<TValue>, AbstractRemoteValidationConfigurator<TValue>> configure, string errorMessageResourceName,
                                                                      Type errorMessageResourceType)
        {
            return Remote(self, configure, null, errorMessageResourceName, errorMessageResourceType);
        }

        private static ModelMetadataItemBuilder<TValue> Remote<TValue>(this ModelMetadataItemBuilder<TValue> self,
                                                                       Func<RemoteValidationConfigurator<TValue>, AbstractRemoteValidationConfigurator<TValue>> configure, Func<string> errorMessage, string errorMessageResourceName,
                                                                       Type errorMessageResourceType)
        {
            var settings = new RemoteValidationConfigurator<TValue>(self, errorMessage, errorMessageResourceName, errorMessageResourceType);
            var configurator = (IRemoteValidationConfigurator<TValue>)configure(settings);
            return configurator.ModelMetadataItemBuilder;
        }
    }
}