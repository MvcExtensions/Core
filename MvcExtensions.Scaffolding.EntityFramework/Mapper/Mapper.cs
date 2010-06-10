#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Defines a class which is used to map between entity to view model and vice versa.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    public class Mapper<TEntity, TViewModel> : IMapper<TEntity, TViewModel> where TEntity : class where TViewModel : IViewModel, new()
    {
        private static readonly Dictionary<Type, IList<Action<TViewModel, TEntity>>> viewModelSourceMapping = new Dictionary<Type, IList<Action<TViewModel, TEntity>>>();
        private static readonly object viewModelSourceMappingSyncLock = new object();

        /// <summary>
        /// Maps the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public TViewModel Map(TEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates from a new entity from the specified entity.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public TEntity CreateFrom(TViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copies the view model to the specified entity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public void Copy(TViewModel source, TEntity destination)
        {
            foreach (Action<TViewModel, TEntity> invoker in GetOrCreateInvokers(source.GetType(), destination.GetType()))
            {
                invoker(source, destination);
            }
        }

        private static IEnumerable<Action<TViewModel, TEntity>> GetOrCreateInvokers(Type sourceType, Type destinationType)
        {
            IList<Action<TViewModel, TEntity>> invokers;

            if (viewModelSourceMapping.TryGetValue(sourceType, out invokers))
            {
                lock (viewModelSourceMappingSyncLock)
                {
                    if (!viewModelSourceMapping.TryGetValue(sourceType, out invokers))
                    {
                        invokers = new List<Action<TViewModel, TEntity>>();

                        IEnumerable<PropertyInfo> destinationProperties = destinationType.GetProperties();

                        foreach (PropertyInfo sourceProperty in sourceType.GetProperties())
                        {
                            string propertyName = sourceProperty.Name;

                            PropertyInfo destinationProperty = destinationProperties.SingleOrDefault(p => p.Name.Equals(propertyName));

                            if (destinationProperty != null)
                            {
                                if (sourceProperty.PropertyType.Equals(destinationProperty.PropertyType))
                                {
                                    ParameterExpression entity = Expression.Parameter(destinationType, "e");
                                    ParameterExpression model = Expression.Parameter(sourceType, "m");

                                    MemberExpression setter = Expression.Property(entity, propertyName);
                                    MemberExpression getter = Expression.Property(model, propertyName);

                                    MethodCallExpression call = Expression.Call(setter, destinationProperty.GetSetMethod(), Expression.Call(getter, sourceProperty.GetGetMethod()));

                                    Expression<Action<TViewModel, TEntity>> invoker = Expression.Lambda<Action<TViewModel, TEntity>>(call, entity, model);

                                    invokers.Add(invoker.Compile());
                                }
                            }
                        }

                        viewModelSourceMapping.Add(sourceType, invokers);
                    }
                }
            }

            return invokers;
        }
    }
}