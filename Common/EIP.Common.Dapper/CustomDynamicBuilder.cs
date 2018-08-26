using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using DapperEx;

namespace EIP.Common.Dapper
{
    public class CustomDynamicBuilder
    {
        /// <summary>
        /// 动态的创建一个类
        /// </summary>
        /// <param name="className"></param>
        /// <param name="lm"></param>
        /// <returns></returns>
        public static Type DynamicCreateType(string className, IList<DynamicPropertyModel> lm)
        {
            //动态创建程序集
            AssemblyName DemoName = new AssemblyName("DynamicClass");
            AssemblyBuilder dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(DemoName, AssemblyBuilderAccess.RunAndSave);
            //动态创建模块
            ModuleBuilder mb = dynamicAssembly.DefineDynamicModule(DemoName.Name, DemoName.Name + ".dll");
            //动态创建类
            TypeBuilder tb = mb.DefineType(className + Guid.NewGuid().ToString().Replace("-", ""), TypeAttributes.Public);
            //动态创建属性            
            if (lm != null && lm.Count > 0)
            {
                foreach (var item in lm)
                {
                    createProperty(tb, item.Name, item.PropertyType);
                }
            }
            //创建动态类型
            Type classType = tb.CreateType();
            return classType;
        }
        /// <summary>
        /// 动态创建类的属性
        /// </summary>
        /// <param name="tb">承载该属性的类型</param>
        /// <param name="properityName">属性名，首字母应大写</param>
        /// <param name="properityType">属性数据类型</param>
        public static FieldBuilder createProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            //定义属性对应的私有字段
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
            //定义属性
            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            //定义与属性相关的get方法
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                propertyType,
                Type.EmptyTypes);
            ILGenerator getIL = getPropMthdBldr.GetILGenerator();//获取il 生成器
            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getIL.Emit(OpCodes.Ret);
            //定义与属性相关的set方法
            MethodBuilder setPropMthdBldr = tb.DefineMethod("set_" + propertyName,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null, new Type[] { propertyType });
            ILGenerator setIL = setPropMthdBldr.GetILGenerator();
            /*
             * OpCodes.Ldarg_0:Ldarg是加载方法参数的意思。这里Ldarg_0事实上是对当前对象的引用即this。
             * 因为类的实例方法（非静态方法）在调用时，this 是会作为第一个参数传入的。
             */
            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);//OpCodes.Ldarg_1:加载参数列表的第一个参数了。
            setIL.Emit(OpCodes.Stfld, fieldBuilder);//OpCodes.Stfld:用新值替换在对象引用或指针的字段中存储的值。
            setIL.Emit(OpCodes.Ret);
            //把get/set方法和属性联系起来
            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
            return fieldBuilder;
        }
    }
}
