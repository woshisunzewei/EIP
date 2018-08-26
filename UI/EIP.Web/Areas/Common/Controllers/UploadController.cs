using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EIP.Common.Web;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Web;
using EIP.Common.Core.Attributes;

namespace EIP.Web.Areas.Common.Controllers
{
    /// <summary>
    /// 上传控制器:结合uploadify
    /// </summary>
    public class UploadController : BaseController
    {
        /// <summary>
        /// 图片上传:点击图片进行上传
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("上传-视图-图片上传")]
        public ViewResultBase Image()
        {
            return View();
        }

        #region KindEditor
        /// <summary>
        /// KindEditor 文件上传
        /// </summary>
        /// <returns></returns>
        public JsonResult KindEditorUpload()
        {
            var maps = new Dictionary<string, string>
		    {
		        {"image", "gif,jpg,jpeg,png,bmp"},
                //{"flash", "swf,flv"},
                //{"media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb"},
                {"file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2"}
		    };
            var file = Request.Files["imgFile"];
            if (file == null || file.InputStream.Length == 0)
            {
                return KindEditorMessage(false, "文件为空");
            }
            var path = string.Concat(HttpRuntime.AppDomainAppPath, "DataUsers\\KEUpload\\");
            var url = "/DataUsers/KEUpload/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var dir = Request.QueryString["dir"];
            dir = string.IsNullOrEmpty(dir) ? "image" : dir;
            if (!maps.ContainsKey(dir))
            {
                return KindEditorMessage(false, "目录不正确");
            }

            if (file.InputStream.Length > 1048576L)
            {
                return KindEditorMessage(false, "上传文件大小超过限制");
            }
            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(extension))
            {
                return KindEditorMessage(false, "上传文件没有扩展名");
            }
            var ext = extension.ToLower();
            if (string.IsNullOrEmpty(ext) || Array.IndexOf(maps[dir].Split(','), ext.Substring(1)) == -1)
            {
                return KindEditorMessage(false, "上传文件扩展名是不允许的扩展名");
            }
            path = string.Concat(path, dir, "\\");
            url = string.Concat(url, dir, "/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var ymd = DateTime.Now.ToString("yyyyMMdd",
                DateTimeFormatInfo.InvariantInfo);
            path = string.Concat(path, ymd, "\\");
            url = string.Concat(url, ymd, "/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var newFilename = (DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ext);
            path = string.Concat(path, newFilename);
            url = string.Concat(url, newFilename, "/");
            file.SaveAs(path);
            return KindEditorMessage(true, url: url);
        }

        /// <summary>
        /// 编辑器JSON消息
        /// </summary>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="message">消息</param>
        /// <param name="url">文件地址</param>
        /// <returns></returns>
        private JsonResult KindEditorMessage(bool isSuccess, string message = "", string url = "")
        {
            return isSuccess ? Json(new { error = 0, url }) : Json(new { error = 1, message });
        }
        #endregion
    }
}
