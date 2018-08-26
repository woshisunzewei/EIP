using System.Linq;
using System.Web;

namespace EIP.Common.Core.Utils
{
    public static class UrlUtil
    {
        /// <summary>
        /// 本地路径转换成URL相对路径
        /// </summary>
        /// <returns></returns>
        public static string UrlConvertor()
        {
            string url = string.Empty;
            string tmpRootDir = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            var rawUrl = HttpContext.Current.Request.RawUrl;
            string mapPath = rawUrl.Contains("?") ? rawUrl.Substring(0,  rawUrl.IndexOf("?")) : rawUrl;
            string viewPath = HttpContext.Current.Request.MapPath(mapPath);
            string fileUrl = viewPath.Replace(tmpRootDir, ""); //转换成相对路径
            fileUrl = fileUrl.Replace(@"\", @"/");
            if (fileUrl.Contains("/"))
            {
                for (int i = 0; i < fileUrl.Split('/').Count(); i++)
                {
                    url = "../" + url;
                }
            }
            return url.TrimEnd('/');
        }

        /// <summary>
        /// 相对路径转换成服务器本地物理路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlConvertorLocal(string url)
        {
            string tmpRootDir = System.Web.HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            string imagesurl2 = tmpRootDir + url.Replace(@"/", @"\"); //转换成绝对路径
            return imagesurl2;
        }
    }
}