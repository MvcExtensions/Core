namespace MvcExtensions
{
    /// <summary>
    /// Configures the <see cref="ModelMetadataItem"/>
    /// </summary>
    public interface IModelMetadataItemConfigurator
    {
        /// <summary>
        /// Configures the <see cref="ModelMetadataItem"/>
        /// </summary>
        /// <param name="item"></param>
        void Configure(ModelMetadataItem item);
    }
}