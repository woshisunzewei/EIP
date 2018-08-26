using System;
using System.IO;

namespace EIP.Common.Core.Utils
{
    /// <summary>
    /// 文件上传帮助类
    /// </summary>
    public class UploadUtil
    {
        #region 二进制输出文件
        /// <summary>
        /// 二进制输出文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="filePath">文件路径</param>
        public static void ResponsOutFile(string fileName,
            string filePath)
        {
            var context = System.Web.HttpContext.Current;
            if (File.Exists(filePath))
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    const long chunkSize = 102400; //100K 每次读取文件，只读取100Ｋ，这样可以缓解服务器的压力
                    var buffer = new byte[chunkSize];

                    //读取要下载的文件
                    using (var iStream = File.OpenRead(filePath))
                    {
                        var dataLengthToRead = iStream.Length; //获取下载的文件总大小
                        //设置响应信息
                        context.Response.Clear();
                        context.Response.ContentType = "application/octet-stream";
                        //火狐浏览器
                        if (System.Web.HttpContext.Current.Request.UserAgent != null &&
                            System.Web.HttpContext.Current.Request.UserAgent.IndexOf("Firefox", StringComparison.Ordinal) >
                            -1)
                        {
                            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                        }
                        else
                        {
                            context.Response.AddHeader("Content-Disposition",
                                "attachment; filename=" + System.Web.HttpContext.Current.Server.UrlPathEncode(fileName));
                        }

                        //将文件流循环写入到 响应流中
                        while (dataLengthToRead > 0 && context.Response.IsClientConnected)
                        {
                            var lengthRead = iStream.Read(buffer, 0, Convert.ToInt32(chunkSize)); //读取的大小
                            context.Response.OutputStream.Write(buffer, 0, lengthRead);
                            context.Response.Flush();
                            dataLengthToRead = dataLengthToRead - lengthRead;
                        }
                        iStream.Close();
                        context.Response.Close();
                    }
                }
            }
        }
        #endregion

        #region 获取文件图标
        /// <summary>
        /// 获取文件图标
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>图标路径</returns>
        public static string GetFileIco(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return "/Scripts/lib/uploadify/other.png";
            }
            switch (new FileInfo(fileName).Extension.ToLower())
            {
                case ".doc":
                    return "/Scripts/lib/uploadify/doc.png";
                case ".docx":
                    return "/Scripts/lib/uploadify/doc.png";
                case ".xls":
                    return "/Scripts/lib/uploadify/xls.png";
                case ".xlsx":
                    return "/Scripts/lib/uploadify/xls.png";
                case ".ppt":
                    return "/Scripts/lib/uploadify/ppt.png";
                case ".pdf":
                    return "/Scripts/lib/uploadify/pdf.png";
                case ".txt":
                    return "/Scripts/lib/uploadify/txt.png";
                case ".wps":
                    return "/Scripts/lib/uploadify/wps.png";
                case ".rar":
                    return "/Scripts/lib/uploadify/rar.png";
                case ".zip":
                    return "/Scripts/lib/uploadify/rar.png";
                case ".png":
                    return "/Scripts/lib/uploadify/img.png";
                case ".gif":
                    return "/Scripts/lib/uploadify/img.png";
                case ".jpg":
                    return "/Scripts/lib/uploadify/img.png";
                case ".jpeg":
                    return "/Scripts/lib/uploadify/img.png";
                default:
                    return "/Scripts/lib/uploadify/other.png";
            }
        }
        #endregion
    }
}