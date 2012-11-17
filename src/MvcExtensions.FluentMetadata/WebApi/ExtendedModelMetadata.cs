namespace MvcExtensions.WebApi
{
    using System;
    using System.Web.Http.Metadata;

    /// <summary>
    /// Defines a metadata class which supports fluent metadata registration.
    /// </summary>
    public class ExtendedModelMetadata : ModelMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedModelMetadata"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="containerType">Type of the container.</param>
        /// <param name="modelAccessor">The model accessor.</param>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="metadata">Metadata</param>
        public ExtendedModelMetadata(ModelMetadataProvider provider, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName, ModelMetadataItem metadata)
            : base(provider, containerType, modelAccessor, modelType, propertyName)
        {
            Metadata = metadata;
        }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        public ModelMetadataItem Metadata { get; private set; }
    }
}