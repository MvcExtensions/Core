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
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Defines a class which is used to dynamically generate the DisplayModel and EditModel for a given <seealso cref="EntityMetadata"/>.
    /// </summary>
    public class ViewModelTypeFactory : IViewModelTypeFactory
    {
        private const string AssemblyName = "ViewModelsAssembly";

        private const MethodAttributes PropertyMethodsAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

        private static readonly Type typeType = typeof(Type);
        private static readonly Type objectType = typeof(object);
        private static readonly Type stringType = typeof(string);

        private static readonly Type viewModelInterfaceType = typeof(IViewModel);
        private static readonly Type typeConverterType = typeof(TypeConverterAttribute);
        private static readonly Type entityConverterType = typeof(EntityConverter);

        private static readonly Type genericNavigationLookupType = typeof(NavigationLookup<,>);
        private static readonly Type genericEnumerableType = typeof(IEnumerable<>);

        private static readonly AssemblyBuilder assemblyBuilder = CreateAssemblyBuilder();
        private static readonly ModuleBuilder moduleBuilder = CreateModuleBuilder();

        private static readonly IEnumerable<MethodInfo> converterToStringMethods = typeof(Convert).GetMethods().Where(m => m.Name.Equals("ToString") && m.GetParameters().Length == 1);

        private readonly IEntityFrameworkMetadataProvider metadataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelTypeFactory"/> class.
        /// </summary>
        /// <param name="metadataProvider">The metadata provider.</param>
        public ViewModelTypeFactory(IEntityFrameworkMetadataProvider metadataProvider)
        {
            Invariant.IsNotNull(metadataProvider, "metadataProvider");

            this.metadataProvider = metadataProvider;
        }

        /// <summary>
        /// Creates the specified model type.
        /// </summary>
        /// <param name="entityType">Type of the model.</param>
        /// <returns></returns>
        public Type Create(Type entityType)
        {
            Invariant.IsNotNull(entityType, "entityType");

            Type viewModelType = BuildType(entityType, "ViewModel");

            return viewModelType;
        }

        private static void ApplyEntityConverterAttribute(TypeBuilder typeBuilder)
        {
            Type[] ctorParameters = new[] { typeType };
            ConstructorInfo ctor = typeConverterType.GetConstructor(ctorParameters);

            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(ctor, new object[] { entityConverterType });

            typeBuilder.SetCustomAttribute(attributeBuilder);
        }

        private static void AddSimplePropertiesAndToStringOverride(TypeBuilder typeBuilder, IEnumerable<PropertyMetadata> properties)
        {
            bool hasKeyAdded = false;

            foreach (PropertyMetadata property in properties)
            {
                string propertyName = property.Name;
                Type propertyType = property.PropertyType;

                if (hasKeyAdded || !property.IsKey)
                {
                    AddProperty(typeBuilder, propertyName, propertyType);
                }
                else
                {
                    FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + property.Name, property.PropertyType, FieldAttributes.PrivateScope);

                    MethodInfo convertMethod = converterToStringMethods.First(m => m.GetParameters()[0].ParameterType.Equals(propertyType));

                    MethodBuilder methodBuilder = typeBuilder.DefineMethod("ToString", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, CallingConventions.Standard, stringType, Type.EmptyTypes);
                    ILGenerator methodIL = methodBuilder.GetILGenerator();

                    methodIL.Emit(OpCodes.Ldarg_0);
                    methodIL.Emit(OpCodes.Ldfld, fieldBuilder);
                    methodIL.Emit(OpCodes.Call, convertMethod);

                    methodIL.Emit(OpCodes.Ret);

                    hasKeyAdded = true;
                }
            }
        }

        private static void AddProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.PrivateScope);

            AddProperty(typeBuilder, fieldBuilder, propertyName, propertyType);
        }

        private static void AddProperty(TypeBuilder typeBuilder, FieldInfo fieldBuilder, string propertyName, Type propertyType)
        {
            string getName = "get_" + propertyName;

            MethodBuilder getMethodBuilder = typeBuilder.DefineMethod(getName, PropertyMethodsAttributes, propertyType, Type.EmptyTypes);
            ILGenerator getMethodIL = getMethodBuilder.GetILGenerator();

            getMethodIL.Emit(OpCodes.Ldarg_0);
            getMethodIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getMethodIL.Emit(OpCodes.Ret);

            string setName = "set_" + propertyName;

            MethodBuilder setMethodBuilder = typeBuilder.DefineMethod(setName, PropertyMethodsAttributes, null, new[] { propertyType });
            ILGenerator setMethodIL = setMethodBuilder.GetILGenerator();

            setMethodIL.Emit(OpCodes.Ldarg_0);
            setMethodIL.Emit(OpCodes.Ldarg_1);
            setMethodIL.Emit(OpCodes.Stfld, fieldBuilder);
            setMethodIL.Emit(OpCodes.Ret);

            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, new[] { propertyType });

            propertyBuilder.SetGetMethod(getMethodBuilder);
            propertyBuilder.SetSetMethod(setMethodBuilder);
        }

        private static string CreateTypeName(Type entityType, string suffix)
        {
            return entityType.FullName.Replace(".", "$") + suffix;
        }

        private static ModuleBuilder CreateModuleBuilder()
        {
            ModuleBuilder builder = assemblyBuilder.DefineDynamicModule(AssemblyName);

            return builder;
        }

        private static AssemblyBuilder CreateAssemblyBuilder()
        {
            AssemblyName assemblyName = new AssemblyName(AssemblyName)
                                            {
                                                Version = typeof(ViewModelTypeFactory).Assembly.GetName().Version
                                            };

            AssemblyBuilder builder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            return builder;
        }

        private Type BuildType(Type entityType, string suffix)
        {
            string typeName = CreateTypeName(entityType, suffix);

            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout, objectType, new[] { viewModelInterfaceType });

            ApplyEntityConverterAttribute(typeBuilder);

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
            typeBuilder.AddInterfaceImplementation(viewModelInterfaceType);

            EntityMetadata entityMetadata = metadataProvider.GetMetadata(entityType);

            IEnumerable<PropertyMetadata> simpleProperties = entityMetadata.Properties.Where(p => !p.IsForeignKey && (p.Navigation == null));
            IEnumerable<PropertyMetadata> navigationProperties = entityMetadata.Properties.Where(p => !p.IsForeignKey && (p.Navigation != null));

            AddSimplePropertiesAndToStringOverride(typeBuilder, simpleProperties);
            AddNavigationProperties(typeBuilder, navigationProperties);

            return typeBuilder.CreateType();
        }

        private void AddNavigationProperties(TypeBuilder typeBuilder, IEnumerable<PropertyMetadata> properties)
        {
            foreach (PropertyMetadata property in properties)
            {
                Type propertyType = GetNavigationPropertyType(property);

                AddProperty(typeBuilder, property.Name, propertyType);
            }
        }

        private Type GetNavigationPropertyType(PropertyMetadata property)
        {
            EntityMetadata navigateEntity = metadataProvider.GetMetadata(property.Navigation.EntityType);
            PropertyMetadata navigateProperty = navigateEntity.FindProperty(property.Navigation.PropertyName);
            Type textType = navigateProperty.PropertyType;
            Type valueType = navigateEntity.GetKeyTypes()[0];

            Type propertyType = genericNavigationLookupType.MakeGenericType(textType, valueType);

            if (property.Navigation.NavigationType == NavigationType.Many)
            {
                propertyType = genericEnumerableType.MakeGenericType(propertyType);
            }

            return propertyType;
        }
    }
}