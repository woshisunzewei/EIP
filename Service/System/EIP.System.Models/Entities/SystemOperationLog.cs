using System;
using EIP.Common.Entities;
using EIP.Common.Entities.CustomAttributes;
namespace EIP.System.Models.Entities
{
    /// <summary>
    /// System_OperationLog表实体类
    /// </summary>
	[Serializable]
    [Db(Name = "EIP_Log")]
    [Table(Name = "System_OperationLog")]
    public class SystemOperationLog : EntityBase
    {
        /// <summary>
        /// 主键Id
        /// </summary>		
		[Id(true)]
        public int OperationLogId { get; set; }

        /// <summary>
        ///     客户端
        /// </summary>
        public string ClientHost { get; set; }

        /// <summary>
        ///     服务端IP地址
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        ///     请求内容大小
        /// </summary>
        public int? RequestContentLength { get; set; }

        /// <summary>
        ///     请求类型 get or post
        /// </summary>
        public string RequestType { get; set; }

        /// <summary>
        ///     当前请求Url信息
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     上次请求Url信息
        /// </summary>
        public string UrlReferrer { get; set; }

        /// <summary>
        ///     请求数据
        /// </summary>
        public string RequestData { get; set; }

        /// <summary>
        ///     浏览器代理信息
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        ///     控制器名称
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        ///     操作名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        ///     Action执行时间(秒)
        /// </summary>
        public double ActionExecutionTime { get; set; }

        /// <summary>
        ///     页面展示时间(秒)
        /// </summary>
        public double ResultExecutionTime { get; set; }

        /// <summary>
        ///     响应状态
        /// </summary>
        public string ResponseStatus { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        ///     创建人员
        /// </summary>
        public Guid CreateUserId { get; set; }

        /// <summary>
        ///     创建人员登录代码
        /// </summary>
        public string CreateUserCode { get; set; }

        /// <summary>
        ///     创建人员名字
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
