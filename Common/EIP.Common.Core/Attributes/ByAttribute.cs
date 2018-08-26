using System;

namespace EIP.Common.Core.Attributes
{
    /// <summary>
    /// 表示一个特性，在该特性中指明标定的类或方法的作者。如果您熟悉了或改了原作者的代码，留下你的名字。系统异常日志记录的时候使用这个名字关联上作者。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor, AllowMultiple = false)]
    public class CreateByAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">开发人员编码</param>
        public CreateByAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">开发人员编码</param>
        /// <param name="time">开发时间</param>
        public CreateByAttribute(string name, string time)
        {
            Name = name;
            Time = time;
        }
        /// <summary>
        /// 开发人员编码
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 开发时间
        /// </summary>
        public string Time { get; set; }
    }

    /// <summary>
    /// 修改
    /// </summary>
    public class EditByAttribute : Attribute
    {
         /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">开发人员编码</param>
        public EditByAttribute(string name)
        {
            Name = name;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">开发人员编码</param>
        /// <param name="time">开发时间</param>
        public EditByAttribute(string name, string time)
        {
            Name = name;
            Time = time;
        }

        /// <summary>
        /// 创建人员名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string Time { get; set; }
    }
}