using System;

namespace EIP.Common.Core.Ldap
{

    /// <summary>
    /// 活动目录架构属性类
    /// <remarks>该属性用户标记活动目录实体类的架构类型</remarks>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments"), AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DirectorySchemaAttribute : Attribute
    {

        #region Constructors

        /// <summary>
        /// 实例化一个活动目录架构属性
        /// </summary>
        /// <param name="schema">架构名</param>
        public DirectorySchemaAttribute(string schema)
        {
            Schema = schema;
        }

        public DirectorySchemaAttribute(string schema, string type)
        {
            Schema = schema;
            Type = type;
        }

        /// <summary>
        /// 实例化一个活动目录架构属性
        /// </summary>
        /// <param name="schema">架构名</param>
        /// <param name="activeDsHelperType">指定一个访问类型(Ldap 或 ActiveDs)</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ds")]
        public DirectorySchemaAttribute(string schema, Type activeDsHelperType)
        {
            Schema = schema;
            ActiveDsHelperType = activeDsHelperType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 架构名
        /// </summary>
        public string Schema { get; private set; }

        public string Type { get; set; }

        /// <summary>
        /// 访问类型
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ds")]
        public Type ActiveDsHelperType { get; set; }

        #endregion
    }

    /// <summary>
    /// 活动目录实体属性类
    /// <remarks>活动目录属性类用于标记一个活动目录实体的具体属性,并和活动目录本身属性作一个映射</remarks>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments"), AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DirectoryAttributeAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// 实例化一个活动目录实体属性
        /// 标记在一个实体属性或者域上
        /// </summary>
        /// <param name="attribute">属性名称</param>
        public DirectoryAttributeAttribute(string attribute)
        {
            Attribute = attribute;
            ReadOnly = false;
            QuerySource = DirectoryAttributeType.Ldap;
        }

        public DirectoryAttributeAttribute(string attribute, bool readOnly)
        {
            Attribute = attribute;
            ReadOnly = readOnly;
            QuerySource = DirectoryAttributeType.Ldap;
        }

        /// <summary>
        /// 实例化一个活动目录实体属性
        /// 标记在一个实体属性或者域上
        /// </summary>
        /// <param name="attribute">属性名称</param>
        /// <param name="querySource">指定一个访问类型(Ldap 或 ActiveDs)</param>
        public DirectoryAttributeAttribute(string attribute, DirectoryAttributeType querySource)
        {
            Attribute = attribute;
            ReadOnly = false;
            QuerySource = querySource;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 属性名称
        /// </summary>
        public string Attribute { get; private set; }

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool ReadOnly { get; private set; }

        /// <summary>
        /// 属性类型
        /// </summary>
        public Type AttributeType { get; private set; }

        /// <summary>
        /// 指定一个访问类型(Ldap 或 ActiveDs)
        /// </summary>
        public DirectoryAttributeType QuerySource { get; set; }

        #endregion
    }

    /// <summary>
    /// 活动目录查询类型
    /// </summary>
    public enum DirectoryAttributeType
    {
        /// <summary>
        /// 默认值，通过标准LDAP对活动目录进行查询
        /// </summary>
        Ldap,

        /// <summary>
        /// 使用 Active Ds 对象对活动目录进行查询
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ds")]
        ActiveDs
    }
}