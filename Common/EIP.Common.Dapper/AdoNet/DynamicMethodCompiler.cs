using System;
using System.Reflection;
using System.Reflection.Emit;

namespace EIP.Common.Dapper.AdoNet
{
    //定义GetHandler委托
    public delegate object GetHandler(object source);

    //定义SetHandler委托
    public delegate void SetHandler(object source, object value);

    //定义InstantiateObjectHandler委托
    public delegate object InstantiateObjectHandler();

    public sealed class DynamicMethodCompiler
    {
        // DynamicMethodCompiler
        private DynamicMethodCompiler()
        {
        }

        // 创建InstantiateObject委托
        internal static InstantiateObjectHandler CreateInstantiateObjectHandler(Type type)
        {
            var constructorInfo = type.GetConstructor(BindingFlags.Public |
                                                      BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0],
                null);

            if (constructorInfo == null)
            {
                throw new ApplicationException(
                    string.Format(
                        "The type {0} must declare an empty constructor (the constructor may be private, internal, protected, protected internal, or public).",
                        type));
            }

            var dynamicMethod = new DynamicMethod("InstantiateObject",
                MethodAttributes.Static |
                MethodAttributes.Public, CallingConventions.Standard, typeof (object),
                null, type, true);

            var generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Newobj, constructorInfo);
            generator.Emit(OpCodes.Ret);
            return (InstantiateObjectHandler) dynamicMethod.CreateDelegate
                (typeof (InstantiateObjectHandler));
        }

        // 创建Get委托
        internal static GetHandler CreateGetHandler(Type type, PropertyInfo propertyInfo)
        {
            var getMethodInfo = propertyInfo.GetGetMethod(true);
            var dynamicGet = CreateGetDynamicMethod(type);
            var getGenerator = dynamicGet.GetILGenerator();

            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Call, getMethodInfo);
            BoxIfNeeded(getMethodInfo.ReturnType, getGenerator);
            getGenerator.Emit(OpCodes.Ret);

            return (GetHandler) dynamicGet.CreateDelegate(typeof (GetHandler));
        }

        // 创建Get委托
        internal static GetHandler CreateGetHandler(Type type, FieldInfo fieldInfo)
        {
            var dynamicGet = CreateGetDynamicMethod(type);
            var getGenerator = dynamicGet.GetILGenerator();

            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Ldfld, fieldInfo);
            BoxIfNeeded(fieldInfo.FieldType, getGenerator);
            getGenerator.Emit(OpCodes.Ret);

            return (GetHandler) dynamicGet.CreateDelegate(typeof (GetHandler));
        }

        // 创建Set委托
        internal static SetHandler CreateSetHandler(Type type, PropertyInfo propertyInfo)
        {
            var setMethodInfo = propertyInfo.GetSetMethod(true);
            var dynamicSet = CreateSetDynamicMethod(type);
            var setGenerator = dynamicSet.GetILGenerator();

            setGenerator.Emit(OpCodes.Ldarg_0);
            setGenerator.Emit(OpCodes.Ldarg_1);
            UnboxIfNeeded(setMethodInfo.GetParameters()[0].ParameterType, setGenerator);
            setGenerator.Emit(OpCodes.Call, setMethodInfo);
            setGenerator.Emit(OpCodes.Ret);

            return (SetHandler) dynamicSet.CreateDelegate(typeof (SetHandler));
        }

        // 创建Set委托
        internal static SetHandler CreateSetHandler(Type type, FieldInfo fieldInfo)
        {
            var dynamicSet = CreateSetDynamicMethod(type);
            var setGenerator = dynamicSet.GetILGenerator();

            setGenerator.Emit(OpCodes.Ldarg_0);
            setGenerator.Emit(OpCodes.Ldarg_1);
            UnboxIfNeeded(fieldInfo.FieldType, setGenerator);
            setGenerator.Emit(OpCodes.Stfld, fieldInfo);
            setGenerator.Emit(OpCodes.Ret);

            return (SetHandler) dynamicSet.CreateDelegate(typeof (SetHandler));
        }

        // 创建Get动态方法
        private static DynamicMethod CreateGetDynamicMethod(Type type)
        {
            return new DynamicMethod("DynamicGet", typeof (object),
                new[] {typeof (object)}, type, true);
        }

        // 创建Set动态方法
        private static DynamicMethod CreateSetDynamicMethod(Type type)
        {
            return new DynamicMethod("DynamicSet", typeof (void),
                new[] {typeof (object), typeof (object)}, type, true);
        }

        // BoxIfNeeded
        private static void BoxIfNeeded(Type type, ILGenerator generator)
        {
            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Box, type);
            }
        }

        // UnboxIfNeeded
        private static void UnboxIfNeeded(Type type, ILGenerator generator)
        {
            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Unbox_Any, type);
            }
        }
    }
}