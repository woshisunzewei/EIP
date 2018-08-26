using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIP.Common.Entities;
using EIP.System.DataAccess.Permission;
using EIP.System.Models.Entities;

namespace EIP.System.Business.Permission.Engine
{
    /// <summary>
    /// 权限引擎实现
    /// </summary>
    public class PermissionEngineLogic : IPermissionEngineLogic
    {
        /// <summary>
        /// 根据Mvc规则及用户Id获取字段权限字符串
        /// </summary>
        /// <param name="rote">Mvc规则</param>
        /// <param name="userId">当前用户Id</param>
        /// <returns></returns>
        public string GetFiledPermissionStrByMvcRote(MvcRote rote, Guid userId)
        {
            SystemFieldRepository repository = new SystemFieldRepository();
            //获取字段权限信息
            return GetFieldStrByMenuIdAndUserId(repository.GetFieldByMenuIdAndUserId(rote, userId).ToList());
        }

        /// <summary>
        /// 根据指定的url路径获取字段权限字符串
        /// </summary>
        /// <param name="url">url地址:区域/控制器/方法</param>
        /// <param name="userId">当前用户Id</param>
        /// <returns></returns>
        public async Task<string> GetFiledPermissionStrByUrl(string url, Guid userId)
        {
            throw new global::System.NotImplementedException();
        }

        /// <summary>
        ///     根据用户的字段权限拼接界面显示字段表达式
        /// </summary>
        /// <param name="bpfFields"></param>
        /// <returns>拼接后的表达式</returns>
        private string GetFieldStrByMenuIdAndUserId(IList<SystemField> bpfFields)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("[");


            foreach (SystemField bpfField in bpfFields)
            {
                stringBuilder.Append("{");
                stringBuilder.Append("label:\"" + bpfField.Label + "\",");
                stringBuilder.Append("name:\"" + bpfField.Name + "\",");
                if (!string.IsNullOrEmpty(bpfField.Index))
                {
                    stringBuilder.Append("index:\"" + bpfField.Index + "\",");
                }

                stringBuilder.Append("align:\"" + bpfField.Align + "\",");
                stringBuilder.Append("width:\"" + bpfField.Width + "\",");

                if (!string.IsNullOrEmpty(bpfField.Fixed.ToString()))
                {
                    stringBuilder.Append("fixed:" + (bpfField.Fixed).ToString().ToLower() + ",");
                }

                stringBuilder.Append("hidden:" + (!bpfField.Hidden).ToString().ToLower() + "");
                if (!string.IsNullOrEmpty(bpfField.Formatter))
                {
                    stringBuilder.Append(",formatter:\"" + bpfField.Formatter + "\"");
                    stringBuilder.Append("},");
                }
                else
                {
                    stringBuilder.Append("},");
                }
            }
            string returnStr = stringBuilder.ToString().TrimEnd(',');
            returnStr = returnStr + "]";
            return returnStr;
        }
    }
}