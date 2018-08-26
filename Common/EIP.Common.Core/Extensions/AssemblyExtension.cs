using System;
using System.Diagnostics;
using System.Reflection;

namespace EIP.Common.Core.Extensions
{
    /// <summary>
    /// 程序集扩展类
    /// </summary>
    public static class AssemblyExtension
    {
        /// <summary>
        /// 获取程序集的文件版本
        /// </summary>
        public static Version GetFileVersion(this Assembly assembly)
        {
            assembly.CheckNotNull("assembly");
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Version(info.FileVersion);
        }

        /// <summary>
        /// 获取程序集的产品版本
        /// </summary>
        public static Version GetProductVersion(this Assembly assembly)
        {
            assembly.CheckNotNull("assembly");
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Version(info.ProductVersion);
        }
    }
}