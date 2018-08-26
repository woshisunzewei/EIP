using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Entities;

namespace EIP.Common.Web
{
    /// <summary>
    /// 功能项导入
    /// </summary>
    public static class FunctionListImport
    {
        /// <summary>
        /// 导入程序集Mvc信息
        /// </summary>
        /// <param name="assemblyStrings">需要获取的程序集</param>
        /// <returns>Mvc路由信息</returns>
        public static IList<MvcRote> Import(Dictionary<string, string> assemblyStrings)
        {
            IList<MvcRote> rotes = new List<MvcRote>();
            foreach (var assembly in assemblyStrings)
            {
                var types = Assembly.LoadFile(assembly.Value).GetTypes();
                //控制器
                var baseControllerType = typeof(BaseController);
                var controllerType = typeof(Controller);
                //界面
                var actionResultType = typeof(ActionResult);
                var viewResultType = typeof(ViewResultBase);
                var taskActionResultType = typeof(Task<ActionResult>);
                var taskviewResultType = typeof(Task<ViewResultBase>);
                //方法
                var jsonType = typeof(JsonResult);
                var taskJsonType = typeof(Task<JsonResult>);

                foreach (var type in types)
                {
                    //是否为控制器类型:Controller或者是BaseController
                    var isController = controllerType.IsAssignableFrom(type) || baseControllerType.IsAssignableFrom(type);
                    // 跳过不是Controller的类型
                    if (!isController)
                    {
                        continue;
                    }
                    //区域
                    string area = string.Empty;
                    var areas = type.FullName.Split('.').ToList();
                    for (int i = 0; i < areas.Count; i++)
                    {
                        if (areas[i] == "Areas")
                        {
                            area = areas[i + 1];
                            break;
                        }
                    }

                    //控制器名称
                    var controller = type.Name.Substring(0, type.Name.Length - "Controller".Length);
                    var methodInfos = type.GetMethods();
                    foreach (var method in methodInfos)
                    {
                        //是否为界面
                        bool isPage = viewResultType.IsAssignableFrom(method.ReturnType) ||
                            actionResultType.IsAssignableFrom(method.ReturnType) ||
                            taskActionResultType.IsAssignableFrom(method.ReturnType) ||
                            taskviewResultType.IsAssignableFrom(method.ReturnType);
                        //是否为方法
                        bool isAction = isPage || jsonType.IsAssignableFrom(method.ReturnType) ||
                              taskJsonType.IsAssignableFrom(method.ReturnType);
                        // 跳过不是Action的方法
                        if (!isAction)
                        {
                            continue;
                        }
                        //方法名称
                        string action = method.Name;
                        //该方法、界面的描述
                        string description,
                            byDeveloperCode = string.Empty,
                            byDeveloperTime = string.Empty;

                        // 如果打有描述文本标记则使用描述文本作为操作说明，否则试用Action方法名
                        var descriptionAttrs = method.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptionAttrs.Length > 0)
                        {
                            description = ((DescriptionAttribute)descriptionAttrs[0]).Description;
                            if (string.IsNullOrEmpty(description))
                            {
                                description = action;
                            }
                        }
                        else
                        {
                            description = action;
                        }

                        // 如果打有描述文本标记则使用描述文本作为编写者说明
                        var byAttrs = method.GetCustomAttributes(typeof(CreateByAttribute), false);
                        if (byAttrs.Length > 0)
                        {
                            byDeveloperCode = ((CreateByAttribute)byAttrs[0]).Name;
                            byDeveloperTime = ((CreateByAttribute)byAttrs[0]).Time;
                        }
                        rotes.Add(new MvcRote()
                        {
                            AppCode = assembly.Key,
                            Area = area,
                            Controller = controller,
                            Action = action,
                            Description = description,
                            IsPage = isPage,
                            ByDeveloperCode = byDeveloperCode,
                            ByDeveloperTime = byDeveloperTime
                        });
                    }
                }
            }
            return rotes;
        }
    }
}