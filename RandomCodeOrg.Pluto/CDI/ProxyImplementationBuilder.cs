using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.CDI {
    public class ProxyImplementationBuilder {

        private readonly AssemblyBuilder assemblyBuilder;
        private readonly ModuleBuilder moduleBuilder;
        

        private int counter = 0;

        private readonly Type baseType = typeof(ProxyImplementation);

        public bool ExportGenerated { get; set; } = true;

        public ProxyImplementationBuilder(Assembly assembly) {
            assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new System.Reflection.AssemblyName(assembly.GetName().Name + "generated"), AssemblyBuilderAccess.RunAndSave);
            moduleBuilder = assemblyBuilder.DefineDynamicModule(assembly.GetName().Name + "generated", "test.dll");
        }

        public AssemblyBuilder Builder {
            get {
                return assemblyBuilder;
            }
        }
        
    

        public Type Build(params Type[] interfaces) {
            TypeBuilder tb = moduleBuilder.DefineType(string.Format("DynamicProxy{0}", counter++), TypeAttributes.Public, baseType, interfaces);
            foreach (Type toImplement in interfaces) {
                tb.AddInterfaceImplementation(toImplement);
                foreach (PropertyInfo propInfo in toImplement.GetProperties()) {
                    var propertyBuilder = tb.DefineProperty(propInfo.Name, PropertyAttributes.None, propInfo.PropertyType, null);

                    if (propInfo.CanRead) {

                        MethodAttributes getPropAttr = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.SpecialName | MethodAttributes.NewSlot;
                        MethodBuilder mb = tb.DefineMethod("get_" + propInfo.Name, getPropAttr, propInfo.PropertyType, null);

                        ILGenerator generator = mb.GetILGenerator();
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldstr, propInfo.Name);
                        generator.EmitCall(OpCodes.Callvirt, baseType.GetMethod("GetProperty", new Type[] { typeof(string) }), null);
                        if (!propInfo.PropertyType.IsByRef)
                            generator.Emit(OpCodes.Unbox_Any, propInfo.PropertyType);
                        generator.Emit(OpCodes.Ret);
                        propertyBuilder.SetGetMethod(mb);
               
                    }
                    if (propInfo.CanWrite) {
                        MethodAttributes setPropAttr = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.SpecialName | MethodAttributes.NewSlot;
                        MethodBuilder mb = tb.DefineMethod("set_" + propInfo.Name, setPropAttr, typeof(void), new Type[] { propInfo.PropertyType });
                        ILGenerator generator = mb.GetILGenerator();
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldstr, propInfo.Name);
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.EmitCall(OpCodes.Callvirt, baseType.GetMethod("SetProperty", new Type[] { typeof(string), typeof(object) }), null);
                        generator.Emit(OpCodes.Ret);
                        propertyBuilder.SetSetMethod(mb);
                    }

                }
                foreach(MethodInfo methodInfo in toImplement.GetMethods()) {
                    if (methodInfo.Attributes.HasFlag(MethodAttributes.SpecialName))
                        continue;
                    MethodAttributes methodAttributes = MethodAttributes.Public | MethodAttributes.Virtual;
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    MethodBuilder mb = tb.DefineMethod(methodInfo.Name, methodAttributes, methodInfo.ReturnType, parameters.Select(p => p.ParameterType).ToArray());
                    ILGenerator generator = mb.GetILGenerator();
                    generator.DeclareLocal(typeof(object[]));
                    generator.Emit(OpCodes.Ldc_I4, parameters.Length);
                    generator.Emit(OpCodes.Newarr, typeof(object));
                    generator.Emit(OpCodes.Stloc_0);
                    for(int i=0; i<parameters.Length; i++) {
                        generator.Emit(OpCodes.Ldloc_0);
                        generator.Emit(OpCodes.Ldc_I4, i);
                        generator.Emit(OpCodes.Ldarg, (UInt16)(i+1));
                        if (!parameters[i].ParameterType.IsByRef) {
                            generator.Emit(OpCodes.Box, parameters[i].ParameterType);
                        }
                        generator.Emit(OpCodes.Stelem_Ref);
                    }
                    generator.Emit(OpCodes.Ldarg_0);
                    string identifier = BuildIdentifier(methodInfo);
                    generator.Emit(OpCodes.Ldstr, identifier);
                    generator.Emit(OpCodes.Ldloc_0);
                    generator.EmitCall(OpCodes.Callvirt, baseType.GetMethod("CallMethod", new Type[] { typeof(string), typeof(object[]) }), null);
                    if (methodInfo.ReturnType == typeof(void)) {
                        generator.Emit(OpCodes.Pop);
                    } else if (!methodInfo.ReturnType.IsByRef)
                        generator.Emit(OpCodes.Box);
                    generator.Emit(OpCodes.Ret);
                    
                    tb.DefineMethodOverride(mb, methodInfo);
                }
            }

         

            return tb.CreateType();
            
        }
        

        public static string BuildIdentifier(MethodInfo mi) {
            StringBuilder sb = new StringBuilder();
            sb.Append(mi.Name);
            sb.Append("(");
            ParameterInfo[] parameters = mi.GetParameters();
            for(int i=0; i<parameters.Length; i++) {
                if (i > 0)
                    sb.Append(", ");
                sb.Append(parameters[i].ParameterType.FullName);
            }
            sb.Append(")");
            sb.Append(":");
            sb.Append(mi.ReturnType.FullName);
            return sb.ToString();
        }


    }
}
