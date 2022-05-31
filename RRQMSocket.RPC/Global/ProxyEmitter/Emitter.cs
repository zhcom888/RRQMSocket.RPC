using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace RRQMSocket.RPC.ProxyEmitter
{
    /// <summary>
    /// Helper class
    /// </summary>
    public static class Emitter
    {
        /// <summary>
        /// Gets the <see cref="AppDomain"/> of current thread
        /// </summary>
        public static AppDomain CurrentDomain
        {
            get { return Thread.GetDomain(); }
        }

        # region Global Methods

        /// <summary>
        /// Creates an <see cref="AssemblyBuilder"/> instance within <see cref="CurrentDomain"/> 
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        public static AssemblyBuilder GetAssemblyBuilder(AssemblyBuilderAccess access = AssemblyBuilderAccess.Run)
        {
            var aname = new AssemblyName("Hayaa.RPCProxy");
            AppDomain currentDomain = AppDomain.CurrentDomain;
            AssemblyBuilder builder = AssemblyBuilder.DefineDynamicAssembly(aname, access);
            return builder;
        }

        /// <summary>
        /// Creates a <see cref="ModuleBuilder"/> instance within <paramref name="asmBuilder"/>
        /// </summary>
        /// <param name="asmBuilder"></param>
        /// <returns></returns>
        public static ModuleBuilder GetModule(AssemblyBuilder asmBuilder)
        {
            ModuleBuilder builder = asmBuilder.DefineDynamicModule("RPCProxy_RemoteServic");
            return builder;
        }

        /// <summary>
        /// Creates an <see cref="EnumBuilder"/> instance within <paramref name="modBuilder"/>
        /// </summary>
        /// <param name="modBuilder"></param>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static EnumBuilder GetEnum(ModuleBuilder modBuilder, string enumName)
        {
            EnumBuilder builder = modBuilder.DefineEnum(enumName, TypeAttributes.Public, typeof(Int32));
            return builder;
        }

        /// <summary>
        /// Create a <see cref="TypeBuilder"/> instance within <paramref name="modBuilder"/>
        /// </summary>
        /// <param name="modBuilder"></param>
        /// <param name="className"></param>
        /// <param name="parent"></param>
        /// <param name="interfaces"></param>
        /// <returns></returns>
        public static TypeBuilder GetType(ModuleBuilder modBuilder, string className, Type parent = null, Type[] interfaces = null)
        {
            if (parent == null)
                parent = typeof(Object);
            TypeBuilder builder = modBuilder.DefineType(className, TypeAttributes.Public, parent, interfaces);
            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modBuilder"></param>
        /// <param name="className"></param>
        /// <param name="genericparameters"></param>
        /// <returns></returns>
        public static TypeBuilder GetType(ModuleBuilder modBuilder, string className, params string[] genericparameters)
        {
            TypeBuilder builder = modBuilder.DefineType(className, TypeAttributes.Public);
            GenericTypeParameterBuilder[] genBuilders = builder.DefineGenericParameters(genericparameters);

            foreach (GenericTypeParameterBuilder genBuilder in genBuilders) // We take each generic type T : class, new()
            {
                genBuilder.SetGenericParameterAttributes(GenericParameterAttributes.ReferenceTypeConstraint | GenericParameterAttributes.DefaultConstructorConstraint);
                //genBuilder.SetInterfaceConstraints(interfaces);
            }

            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="methodName"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static MethodBuilder GetMethod(TypeBuilder typeBuilder, string methodName, MethodAttributes attributes)
        {
            MethodBuilder builder = typeBuilder.DefineMethod(
                methodName,
                attributes);
            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="methodName"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        public static MethodBuilder GetMethod(TypeBuilder typeBuilder, string methodName, MethodAttributes attributes, Type returnType, params Type[] parameterTypes)
        {
            MethodBuilder builder = typeBuilder.DefineMethod(
                methodName,
                attributes,
                CallingConventions.HasThis,
                returnType,
                parameterTypes);
            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="attributes"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        public static ConstructorBuilder GetConstructor(TypeBuilder typeBuilder, MethodAttributes attributes, params Type[] parameterTypes)
        {
            return typeBuilder.DefineConstructor(attributes, CallingConventions.HasThis, parameterTypes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="methodName"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="genericParameters"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        public static MethodBuilder GetMethod(TypeBuilder typeBuilder, string methodName, MethodAttributes attributes, Type returnType, string[] genericParameters, params Type[] parameterTypes)
        {
            MethodBuilder builder = typeBuilder.DefineMethod(
                methodName,
                attributes,
                CallingConventions.HasThis,
                returnType, parameterTypes);

            GenericTypeParameterBuilder[] genBuilders = builder.DefineGenericParameters(genericParameters);

            foreach (GenericTypeParameterBuilder genBuilder in genBuilders) // We take each generic type T : class, new()
            {
                genBuilder.SetGenericParameterAttributes(GenericParameterAttributes.ReferenceTypeConstraint | GenericParameterAttributes.DefaultConstructorConstraint);
                //genBuilder.SetInterfaceConstraints(interfaces);
            }
            return builder;
        }
        # endregion
    }
}
