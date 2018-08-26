using EIP.Common.Entities.Dtos;

namespace EIP.System.Models.Dtos.Identity
{
    /// <summary>
    /// 用户登录输入实体
    /// </summary>
    public class UserLoginInput : IInputDto
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Verify { get; set; }

        /// <summary>
        /// 记住我
        /// </summary>
        public bool Remberme { get; set; }

        /// <summary>
        /// 跳转url
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}