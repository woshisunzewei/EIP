using System.Web.Http;
using EIP.Common.Web.Api.Attibutes;

namespace EIP.Common.Web.Api
{
    /// <summary>
    /// 所有需要验证权限的Api接口
    /// </summary>
    [ApiExceptionFilter]
    public class BaseApiController : ApiController
    {

    }
}