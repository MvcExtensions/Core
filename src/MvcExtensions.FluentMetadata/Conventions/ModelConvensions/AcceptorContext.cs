namespace MvcExtensions
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class AcceptorContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="hasMetadataConfiguration"></param>
        public AcceptorContext(Type modelType, bool hasMetadataConfiguration)
        {
            ModelType = modelType;
            HasMetadataConfiguration = hasMetadataConfiguration;
        }

        /// <summary>
        /// Indicates whether <see cref="ModelType"/> has related <seealso cref="IModelMetadataConfiguration"/> implementation
        /// </summary>
        public bool HasMetadataConfiguration { get; private set; }

        /// <summary>
        /// Type of view model
        /// </summary>
        public Type ModelType { get; private set; }
    }
}