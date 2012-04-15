namespace MvcExtensions
{
    using System.Web.Mvc;

    /// <summary>
    /// Represents an interface to register the default <seealso cref="ModelMetadataProvider"/>.
    /// </summary>
    public interface IModelMetadataRegistrar
    {
        /// <summary>
        /// Registers metadata provider
        /// </summary>
        /// <returns></returns>
        void RegisterMetadataProviders();
    }
}