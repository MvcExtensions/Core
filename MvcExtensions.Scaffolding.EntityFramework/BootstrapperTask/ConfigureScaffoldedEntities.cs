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
    using System.ComponentModel;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines a class which is used to configure the scaffolded entities.
    /// </summary>
    public class ConfigureScaffoldedEntities : BootstrapperTask
    {
        private static readonly Type genericNavigationLookupType = typeof(NavigationLookup<,>);

        private static readonly Regex NameExpression = new Regex("([A-Z]+(?=$|[A-Z][a-z])|[A-Z]?[a-z]+)", RegexOptions.Compiled);

        private static readonly Type[] booleanTypes = new[] { typeof(bool), typeof(bool?) };
        private static readonly Type[] decimalTypes = new[] { typeof(decimal), typeof(decimal?) };
        private static readonly Type[] dateTypes = new[] { typeof(DateTime), typeof(DateTime?) };
        private static readonly Type[] otherValueTypes = new[] { typeof(byte), typeof(byte?), typeof(short), typeof(short?), typeof(int), typeof(int?), typeof(long), typeof(long?), typeof(float), typeof(float?), typeof(double), typeof(double?) };
        private static readonly Type stringType = typeof(string);

        private readonly ContainerAdapter container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureScaffoldedEntities"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ConfigureScaffoldedEntities(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");

            this.container = container;
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            IViewModelTypeRegistry viewModelRegistry = container.GetInstance<IViewModelTypeRegistry>();
            IEntityFrameworkMetadataProvider metadataProvider = container.GetInstance<IEntityFrameworkMetadataProvider>();
            IModelMetadataRegistry metadataRegistry = container.GetInstance<IModelMetadataRegistry>();

            foreach (KeyValuePair<Type, Type> pair in viewModelRegistry.GetMapping())
            {
                IDictionary<string, ModelMetadataItem> propertiesMetadata = new Dictionary<string, ModelMetadataItem>();

                Type entityType = pair.Key;
                Type viewModelType = pair.Key;

                EntityMetadata entityMetadata = metadataProvider.GetMetadata(entityType);

                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(viewModelType))
                {
                    PropertyMetadata propertyMetadata = entityMetadata.FindProperty(property.Name);
                    ModelMetadataItem metadataItem;

                    string propertyNameInUpperCase = property.Name.ToUpperInvariant();

                    if (property.PropertyType == stringType)
                    {
                        StringMetadataItem stringMetadataItem = new StringMetadataItem();
                        StringMetadataItemBuilder builder = new StringMetadataItemBuilder(stringMetadataItem);

                        if (propertyNameInUpperCase.Contains("EMAIL"))
                        {
                            builder.AsEmail();
                        }
                        else if (propertyNameInUpperCase.Contains("URL"))
                        {
                            builder.AsUrl();
                        }
                        else if (propertyNameInUpperCase.Contains("HTML"))
                        {
                            builder.AsHtml();
                        }
                        else if (propertyNameInUpperCase.Contains("PASSWORD"))
                        {
                            builder.AsPassword();
                        }

                        if (propertyMetadata.MaximumLength == int.MaxValue)
                        {
                            builder.AsMultilineText();
                        }

                        builder.MaximumLength(propertyMetadata.MaximumLength);

                        metadataItem = stringMetadataItem;
                    }
                    else if (dateTypes.Contains(property.PropertyType))
                    {
                        ValueTypeMetadataItem valueTypeMetadataItem = new ValueTypeMetadataItem();
                        ValueTypeMetadataItemBuilder<DateTime> builder = new ValueTypeMetadataItemBuilder<DateTime>(valueTypeMetadataItem);

                        if (propertyNameInUpperCase.EndsWith("DATE", StringComparison.Ordinal))
                        {
                            builder.FormatAsDateOnly();
                        }
                        else if (propertyNameInUpperCase.EndsWith("TIME", StringComparison.Ordinal))
                        {
                            builder.FormatAsTimeOnly();
                        }

                        metadataItem = valueTypeMetadataItem;
                    }
                    else if (decimalTypes.Contains(property.PropertyType))
                    {
                        ValueTypeMetadataItem valueTypeMetadataItem = new ValueTypeMetadataItem();
                        new ValueTypeMetadataItemBuilder<decimal>(valueTypeMetadataItem).FormatAsCurrency();

                        metadataItem = valueTypeMetadataItem;
                    }
                    else if (booleanTypes.Contains(property.PropertyType))
                    {
                        metadataItem = new BooleanMetadataItem();
                    }
                    else if (otherValueTypes.Contains(property.PropertyType))
                    {
                        metadataItem = new ValueTypeMetadataItem();
                    }
                    else if ((property.PropertyType.IsGenericType) && (genericNavigationLookupType.IsAssignableFrom(property.PropertyType.GetGenericTypeDefinition())))
                    {
                        metadataItem = new ObjectMetadataItem();
                    }
                    else
                    {
                        metadataItem = new ObjectMetadataItem();
                    }

                    metadataItem.ShowForEdit = !propertyMetadata.IsGenerated;

                    if (propertyMetadata.IsForeignKey)
                    {
                        metadataItem.ShowForDisplay = false;
                        metadataItem.ShowForEdit = false;
                    }

                    if (!propertyMetadata.IsNullable)
                    {
                        metadataItem.IsRequired = true;
                    }

                    metadataItem.DisplayName = GetDisplayName(entityMetadata, propertyMetadata);

                    propertiesMetadata.Add(propertyMetadata.Name, metadataItem);
                }

                metadataRegistry.RegisterModelProperties(viewModelType, propertiesMetadata);
            }

            return TaskContinuation.Continue;
        }

        private static string GetDisplayName(EntityMetadata entityMetadata, PropertyMetadata propertyMetadata)
        {
            string displayName = NameExpression.Replace(propertyMetadata.Name, " $1").Trim();

            if (displayName.StartsWith(entityMetadata.EntitySetName, StringComparison.OrdinalIgnoreCase))
            {
                displayName = displayName.Substring(entityMetadata.EntitySetName.Length);
            }

            if (displayName.StartsWith(entityMetadata.EntityType.Name, StringComparison.OrdinalIgnoreCase))
            {
                displayName = displayName.Substring(entityMetadata.EntityType.Name.Length);
            }

            displayName = displayName.Trim();

            if (displayName.Length == 0)
            {
                displayName = propertyMetadata.Name;
            }

            return displayName;
        }
    }
}