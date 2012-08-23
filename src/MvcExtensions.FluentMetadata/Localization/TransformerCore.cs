namespace MvcExtensions
{
    using System;

    /// <summary>
    /// Base class for transformers
    /// </summary>
    public abstract class TransformerCore
    {
        /// <summary>
        /// Format Resource key for given <paramref name="containerType"/> and <paramref name="propertyName"/>
        /// </summary>
        /// <param name="containerType">Container type</param>
        /// <param name="propertyName">Property name</param>
        /// <returns></returns>
        protected static string GetResourceKey(Type containerType, string propertyName)
        {
            return String.Format("{0}_{1}", containerType.Name, propertyName);
        }

        /// <summary>
        /// Checks if Resource file <paramref name="resourceKey"/> contains a <paramref name="resourceKey"/>
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        protected static bool HasResourceValue(Type resourceType, string resourceKey)
        {
            return resourceType.HasProperty(resourceKey);
        }
    }
}