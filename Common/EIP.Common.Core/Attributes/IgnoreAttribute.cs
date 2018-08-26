using System;

namespace EIP.Common.Core.Attributes
{
    /// <summary>
    /// 表示一个特性,标识该特性的Action方法绕过认证
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class IgnoreAttribute:Attribute
    {
         
    }
}