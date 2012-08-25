#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Extends DisplayAttribute class
    /// </summary>
    public static class DisplayAttributeExtensions
    {
        /// <summary>
        /// Creates a copy of <see cref="DisplayAttribute"/>
        /// </summary>
        /// <param name="srcAttribute"></param>
        /// <returns></returns>
        public static DisplayAttribute Copy(this DisplayAttribute srcAttribute)
        {
            if (srcAttribute == null)
            {
                return null;
            }

            var copy = new DisplayAttribute
                {
                    Name = srcAttribute.Name,
                    GroupName = srcAttribute.GroupName,
                    Description = srcAttribute.Description,
                    ResourceType = srcAttribute.ResourceType,
                    ShortName = srcAttribute.ShortName,
                    Prompt = srcAttribute.Prompt
                };

            var order = srcAttribute.GetOrder();
            if (order != null)
            {
                copy.Order = order.Value;
            }
            
            var autoGenerateField = srcAttribute.GetAutoGenerateField();
            if (autoGenerateField != null)
            {
                copy.AutoGenerateField = autoGenerateField.Value;
            }

            var autoGenerateFilter = srcAttribute.GetAutoGenerateFilter();
            if (autoGenerateFilter != null)
            {
                copy.AutoGenerateFilter = autoGenerateFilter.Value;
            }

            return copy;
        }
    }
}
