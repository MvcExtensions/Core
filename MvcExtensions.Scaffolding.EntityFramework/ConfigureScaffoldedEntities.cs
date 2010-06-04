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
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines a class which is used to configure the scaffolded entities.
    /// </summary>
    public class ConfigureScaffoldedEntities : BootstrapperTask
    {
        private const int LargeTextLength = 1073741823;

        private static readonly Regex NameExpression = new Regex("([A-Z]+(?=$|[A-Z][a-z])|[A-Z]?[a-z]+)", RegexOptions.Compiled);

        private static readonly Type[] booleanTypes = new[] {typeof(bool), typeof (bool?)};
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
            IModelMetadataRegistry metadataRegistry = container.GetInstance<IModelMetadataRegistry>();
            IEntityFrameworkMetadataProvider metadataProvider = container.GetInstance<IEntityFrameworkMetadataProvider>();

            foreach (EntityMetadata entityMetadata in metadataProvider)
            {
                IDictionary<string, ModelMetadataItem> propertiesMetadata = new Dictionary<string, ModelMetadataItem>();

                foreach (PropertyMetadata propertyMetadata in entityMetadata.Properties)
                {
                    string propertyNameInUpperCase = propertyMetadata.Name.ToUpperInvariant();
                    ModelMetadataItem metadataItem;

                    if (propertyMetadata.PropertyType == stringType)
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

                        if (propertyMetadata.Length == LargeTextLength)
                        {
                            builder.AsMultilineText();
                        }

                        builder.MaximumLength(propertyMetadata.Length);

                        metadataItem = stringMetadataItem;
                    }
                    else if (dateTypes.Contains(propertyMetadata.PropertyType))
                    {
                        ValueTypeMetadataItem valueTypeMetadataItem = new ValueTypeMetadataItem();
                        ValueTypeMetadataItemBuilder<DateTime> builder = new ValueTypeMetadataItemBuilder<DateTime>(valueTypeMetadataItem);

                        if (propertyNameInUpperCase.EndsWith("DATE"))
                        {
                            builder.FormatAsDateOnly();
                        }
                        else if (propertyNameInUpperCase.EndsWith("TIME"))
                        {
                            builder.FormatAsTimeOnly();
                        }

                        metadataItem = valueTypeMetadataItem;
                    }
                    else if (decimalTypes.Contains(propertyMetadata.PropertyType))
                    {
                        ValueTypeMetadataItem valueTypeMetadataItem = new ValueTypeMetadataItem();
                        new ValueTypeMetadataItemBuilder<decimal>(valueTypeMetadataItem).FormatAsCurrency();

                        metadataItem = valueTypeMetadataItem;
                    }
                    else if (booleanTypes.Contains(propertyMetadata.PropertyType))
                    {
                        metadataItem = new BooleanMetadataItem();
                    }
                    else if (otherValueTypes.Contains(propertyMetadata.PropertyType))
                    {
                        metadataItem = new ValueTypeMetadataItem();
                    }
                    else
                    {
                        metadataItem = new ObjectMetadataItem();
                    }

                    metadataItem.ShowForEdit = !propertyMetadata.IsGenerated;

                    if (propertyMetadata.IsNullable)
                    {
                        metadataItem.IsRequired = true;
                    }

                    metadataItem.DisplayName = GetDisplayName(entityMetadata, propertyMetadata);

                    propertiesMetadata.Add(propertyMetadata.Name, metadataItem);
                }

                metadataRegistry.RegisterModelProperties(entityMetadata.EntityType, propertiesMetadata);
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