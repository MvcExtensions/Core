#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    /// <summary>
    /// Responsible for <see cref="IModelMetadataConfiguration"/> registration
    /// </summary>
    public interface IRegistrar
    {
        /// <summary>
        /// Registers metadata provider and model metadata configuration classes
        /// </summary>
        void Register();
    }
}
