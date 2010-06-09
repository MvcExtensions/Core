#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    /// <summary>
    /// Defines an interface which is used to map between entity to view model and vice versa.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    public interface IMapper<TEntity, TViewModel> where TEntity : class where TViewModel : IViewModel
    {
        /// <summary>
        /// Maps the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        TViewModel Map(TEntity entity);

        /// <summary>
        /// Creates from a new entity from the specified entity.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        TEntity CreateFrom(TViewModel viewModel);

        /// <summary>
        /// Copies the view model to the specified entity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        void Copy(TViewModel source, TEntity destination);
    }
}