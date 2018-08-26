/**
* EIP.Common.Web.Api.Cors-跨域帮助类:跨域资源共享
* 2016-06-04 add by 孙泽伟
1、Install-Package Microsoft.AspNet.WebApi.Cors
     注意 Web api 2对,net framework的要求必须是4.5以上
2、全局
   var cors = new EnableCorsAttribute("http://localhost:49729", "*", "*");
   config.EnableCors(cors);
*/