#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Scaffolding.EntityFramework
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Defines a class which is used to dynamically generate the DisplayModel and EditModel for a given <seealso cref="EntityMetadata"/>.
    /// </summary>
    public class ViewModelFactory : IViewModelFactory
    {
        private const string Name = "ViewModels";
        private const string AssemblyName = Name + "Assembly";
        private const string ModuleName = Name + "Module";

        private const BindingFlags PropertyBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty;
        private const MethodAttributes PropertyMethodsAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

        private static readonly Type typeType = typeof(Type);
        private static readonly Type objectType = typeof(object);

        private static readonly Type stringType = typeof(string);
        private static readonly Type dateTimeType = typeof(DateTime);
        private static readonly Type decimalType = typeof(decimal);
        private static readonly Type[] extraSimpleTypes = new[] { stringType, dateTimeType, decimalType };

        private static readonly Type typeConverterType = typeof(TypeConverterAttribute);
        private static readonly Type entityConverterType = typeof(EntityConverter);

        private static readonly AssemblyBuilder assemblyBuilder = CreateAssemblyBuilder();
        private static readonly ModuleBuilder moduleBuilder = CreateModuleBuilder();

        /// <summary>
        /// Creates the display model.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns></returns>
        public Type CreateDisplayModel(Type modelType)
        {
            TypeBuilder typeBuilder = CreateTypeBuilder(modelType, "DisplayModel");

            return typeBuilder.CreateType();
        }

        /// <summary>
        /// Creates the edit model.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns></returns>
        public Type CreateEditModel(Type modelType)
        {
            TypeBuilder typeBuilder = CreateTypeBuilder(modelType, "EditModel");

            return typeBuilder.CreateType();
        }

        private static TypeBuilder CreateTypeBuilder(Type modelType, string suffix)
        {
            string typeName = CreateTypeName(modelType, suffix);

            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout, objectType);

            ApplyEntityConverterAttribute(typeBuilder);

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            AddSimpleTypeProperties(typeBuilder, modelType);

            return typeBuilder;
        }

        private static void ApplyEntityConverterAttribute(TypeBuilder typeBuilder)
        {
            Type[] ctorParameters = new[] { typeType };
            ConstructorInfo ctor = typeConverterType.GetConstructor(ctorParameters);

            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(ctor, new object[] { entityConverterType });

            typeBuilder.SetCustomAttribute(attributeBuilder);
        }

        private static void AddSimpleTypeProperties(TypeBuilder typeBuilder, IReflect modelType)
        {
            foreach (PropertyInfo property in modelType.GetProperties(PropertyBindingFlags).Where(p => IsSimpleType(p.PropertyType)))
            {
                FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + property.Name, property.PropertyType, FieldAttributes.Private);

                string getName = "get_" + property.Name;

                MethodBuilder getMethodBuilder = typeBuilder.DefineMethod(getName, PropertyMethodsAttributes, property.PropertyType, Type.EmptyTypes);
                ILGenerator getMethodIL = getMethodBuilder.GetILGenerator();

                getMethodIL.Emit(OpCodes.Ldarg_0);
                getMethodIL.Emit(OpCodes.Ldfld, fieldBuilder);
                getMethodIL.Emit(OpCodes.Ret);

                string setName = "set_" + property.Name;

                MethodBuilder setMethodBuilder = typeBuilder.DefineMethod(setName, PropertyMethodsAttributes, null, new[] { property.PropertyType });
                ILGenerator setMethodIL = setMethodBuilder.GetILGenerator();

                setMethodIL.Emit(OpCodes.Ldarg_0);
                setMethodIL.Emit(OpCodes.Ldarg_1);
                setMethodIL.Emit(OpCodes.Stfld, fieldBuilder);
                setMethodIL.Emit(OpCodes.Ret);

                PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(property.Name, PropertyAttributes.HasDefault, property.PropertyType, new[] { property.PropertyType });

                propertyBuilder.SetGetMethod(getMethodBuilder);
                propertyBuilder.SetSetMethod(setMethodBuilder);
            }
        }

        private static bool IsSimpleType(Type type)
        {
            Type actualType = Nullable.GetUnderlyingType(type) ?? type;

            return actualType.IsPrimitive || extraSimpleTypes.Contains(actualType);
        }

        private static string CreateTypeName(Type modelType, string suffix)
        {
            return modelType.FullName.Replace(".", "$") + "$" + suffix;
        }

        private static ModuleBuilder CreateModuleBuilder()
        {
            ModuleBuilder builder = assemblyBuilder.DefineDynamicModule(ModuleName);

            return builder;
        }

        private static AssemblyBuilder CreateAssemblyBuilder()
        {
            AssemblyName assemblyName = new AssemblyName(AssemblyName)
                                            {
                                                Version = typeof(ViewModelFactory).Assembly.GetName().Version
                                            };

            AssemblyBuilder builder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            return builder;
        }
    }
}