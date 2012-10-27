#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    /// <summary>
    /// Default interface that has to be implemented to accept conventions for metadata
    /// </summary>
    public interface IModelConventionAcceptor
    {
        /// <summary>
        /// Checks whether metadata for class can be accepted
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool CanAcceptConventions(AcceptorContext context);
    }
}
