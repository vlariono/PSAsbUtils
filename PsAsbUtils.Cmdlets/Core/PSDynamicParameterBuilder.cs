using System.Management.Automation;
using System.Reflection;
using System.Reflection.Emit;

namespace PsAsbUtils.Cmdlets;

public class PSDynamicParameterBuilder<T>
{
    private readonly TypeBuilder _typeBuilder;

    private static Type? s_type;

    public PSDynamicParameterBuilder()
    {
        var assemblyName = new AssemblyName(nameof(PSDynamicParameterBuilder<T>));
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
        _typeBuilder = moduleBuilder.DefineType(assemblyName.FullName,
                                                TypeAttributes.Public |
                                                TypeAttributes.Class |
                                                TypeAttributes.AutoClass |
                                                TypeAttributes.AnsiClass |
                                                TypeAttributes.BeforeFieldInit |
                                                TypeAttributes.AutoLayout, null);

        _typeBuilder.DefineDefaultConstructor(
            MethodAttributes.Public |
            MethodAttributes.SpecialName |
            MethodAttributes.RTSpecialName);
    }

    public object Activate()
    {
        s_type ??= _typeBuilder.CreateType();
        return Activator.CreateInstance(_typeBuilder.CreateType())!;
    }

    public void AddParameter(string name, Type type, bool mandatory)
    {
        FieldBuilder fieldBuilder = _typeBuilder.DefineField("_" + name, type, FieldAttributes.Private);

        PropertyBuilder propertyBuilder = _typeBuilder.DefineProperty(name, PropertyAttributes.HasDefault, type, null);
        MethodBuilder getPropMthdBldr = _typeBuilder.DefineMethod("get_" + name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, type, Type.EmptyTypes);
        ILGenerator getIl = getPropMthdBldr.GetILGenerator();

        getIl.Emit(OpCodes.Ldarg_0);
        getIl.Emit(OpCodes.Ldfld, fieldBuilder);
        getIl.Emit(OpCodes.Ret);

        MethodBuilder setPropMthdBldr = _typeBuilder.DefineMethod("set_" + name,
              MethodAttributes.Public |
              MethodAttributes.SpecialName |
              MethodAttributes.HideBySig,
              null, new[] { type });

        ILGenerator setIl = setPropMthdBldr.GetILGenerator();
        Label modifyProperty = setIl.DefineLabel();
        Label exitSet = setIl.DefineLabel();

        setIl.MarkLabel(modifyProperty);
        setIl.Emit(OpCodes.Ldarg_0);
        setIl.Emit(OpCodes.Ldarg_1);
        setIl.Emit(OpCodes.Stfld, fieldBuilder);

        setIl.Emit(OpCodes.Nop);
        setIl.MarkLabel(exitSet);
        setIl.Emit(OpCodes.Ret);

        propertyBuilder.SetGetMethod(getPropMthdBldr);
        propertyBuilder.SetSetMethod(setPropMthdBldr);

        var attributeCtor = typeof(ParameterAttribute).GetConstructor(Array.Empty<Type>());
        var mandatoryProperty = typeof(ParameterAttribute).GetProperty("Mandatory");
        var attributeBuilder = new CustomAttributeBuilder(attributeCtor, Array.Empty<object>(),
                                                            namedProperties: new[] { mandatoryProperty },
                                                            propertyValues: new object[] { mandatory });

        propertyBuilder.SetCustomAttribute(attributeBuilder);
    }
}


// public class MyClassBuilder
// {
//     AssemblyName assemblyName;
//     public MyClassBuilder(string ClassName)
//     {
//         this.assemblyName = new AssemblyName(ClassName);
//     }
//     public object CreateObject(string[] PropertyNames, Type[] Types)
//     {
//         if (PropertyNames.Length != Types.Length)
//         {
//             Console.WriteLine("The number of property names should match their corresopnding types number");
//         }

//         TypeBuilder DynamicClass = this.CreateClass();
//         this.CreateConstructor(DynamicClass);
//         for (int ind = 0; ind < PropertyNames.Count(); ind++)
//             CreateProperty(DynamicClass, PropertyNames[ind], Types[ind]);
//         Type type = DynamicClass.CreateType();

//         return Activator.CreateInstance(type);
//     }
//     private TypeBuilder CreateClass()
//     {

//         AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(this.assemblyName, AssemblyBuilderAccess.Run);
//         ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
//         return moduleBuilder.DefineType(this.assemblyName.FullName
//                             , TypeAttributes.Public |
//                             TypeAttributes.Class |
//                             TypeAttributes.AutoClass |
//                             TypeAttributes.AnsiClass |
//                             TypeAttributes.BeforeFieldInit |
//                             TypeAttributes.AutoLayout
//                             , null);
//     }
//     private void CreateConstructor(TypeBuilder typeBuilder)
//     {
//         typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
//     }
//     private void CreateProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
//     {
//         FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

//         PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
//         MethodBuilder getPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
//         ILGenerator getIl = getPropMthdBldr.GetILGenerator();

//         getIl.Emit(OpCodes.Ldarg_0);
//         getIl.Emit(OpCodes.Ldfld, fieldBuilder);
//         getIl.Emit(OpCodes.Ret);

//         MethodBuilder setPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName,
//               MethodAttributes.Public |
//               MethodAttributes.SpecialName |
//               MethodAttributes.HideBySig,
//               null, new[] { propertyType });

//         ILGenerator setIl = setPropMthdBldr.GetILGenerator();
//         Label modifyProperty = setIl.DefineLabel();
//         Label exitSet = setIl.DefineLabel();

//         setIl.MarkLabel(modifyProperty);
//         setIl.Emit(OpCodes.Ldarg_0);
//         setIl.Emit(OpCodes.Ldarg_1);
//         setIl.Emit(OpCodes.Stfld, fieldBuilder);

//         setIl.Emit(OpCodes.Nop);
//         setIl.MarkLabel(exitSet);
//         setIl.Emit(OpCodes.Ret);

//         propertyBuilder.SetGetMethod(getPropMthdBldr);
//         propertyBuilder.SetSetMethod(setPropMthdBldr);
//     }
// }