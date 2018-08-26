using System;
using System.IO;
using System.Web;
using EIP.Common.Core.Utils;

namespace EIP.Common.Core.Log
{
    public class OperationLogHandler : BaseHandler<OperateLog>
    {
        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="httpRequestBase"></param>
        public OperationLogHandler(HttpRequestBase httpRequestBase)
            : base("OperateLogToDatabase")
        {
            var request = HttpContext.Current.Request;
            log = new OperateLog()
            {
                CreateTime = DateTime.Now,
                ServerHost = String.Format("{0}【{1}】", IpBrowserUtil.GetServerHost(), IpBrowserUtil.GetServerHostIp()),
                ClientHost = String.Format("{0}", IpBrowserUtil.GetClientIp()),
                RequestContentLength = httpRequestBase.ContentLength,
                RequestType = httpRequestBase.RequestType,
                UserAgent = httpRequestBase.UserAgent
            };

            var inputStream = request.InputStream;
            var streamReader = new StreamReader(inputStream);
            var requestData = HttpUtility.UrlDecode(streamReader.ReadToEnd());
            log.RequestData = requestData;
            if (httpRequestBase.Url != null)
            {
                log.Url = httpRequestBase.Url.ToString();
            }
            if (httpRequestBase.UrlReferrer != null)
            {
                log.UrlReferrer = httpRequestBase.UrlReferrer.ToString();
            }
        }

        /// <summary>
        /// 执行时间
        /// </summary>
        public void ActionExecuted()
        {
            log.ActionExecutionTime = (DateTime.Now - log.CreateTime).TotalSeconds;
        }

        /// <summary>
        /// 页面展示时间
        /// </summary>
        /// <param name="responseBase"></param>
        public void ResultExecuted(HttpResponseBase responseBase)
        {
            log.ResponseStatus = responseBase.Status;
            //页面展示时间
            log.ResultExecutionTime = (DateTime.Now - log.CreateTime).TotalSeconds;
        }
    }
}