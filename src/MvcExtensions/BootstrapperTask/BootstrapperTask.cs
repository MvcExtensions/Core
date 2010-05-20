#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    /// <summary>
    /// Defines a base class which is executed when <see cref="ExtendedMvcApplication"/> starts and ends.
    /// </summary>
    public abstract class BootstrapperTask : OrderableTask
    {
    }
}