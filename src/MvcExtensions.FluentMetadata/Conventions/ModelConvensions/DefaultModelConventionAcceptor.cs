namespace MvcExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultModelConventionAcceptor : IModelConventionAcceptor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool CanAcceptConventions(AcceptorContext context)
        {
            return context.HasMetadataConfiguration;        
        }
    }
}