using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Core.Utils;
using EIP.Common.Entities;
using EIP.Common.Web;
using EIP.System.Business.Config;
using EIP.System.Business.Permission;
using EIP.System.Models.Dtos.Config;
using EIP.System.Models.Entities;
using EIP.System.Models.Enums;
using EIP.Web.Areas.System.Models;
using System.ComponentModel;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    ///     代码生成器
    /// </summary>
    public class CodeGenerationController : BaseController
    {
        #region 构造函数

        private readonly ISystemDataBaseLogic _dataBaseLogic;
        private readonly ISystemMenuButtonLogic _menuButtonLogic;
        public CodeGenerationController(ISystemDataBaseLogic dataBaseLogic, ISystemMenuButtonLogic menuButtonLogic)
        {
            _dataBaseLogic = dataBaseLogic;
            _menuButtonLogic = menuButtonLogic;
        }

        #endregion

        #region Pc

        #region 视图

        /// <summary>
        ///     Pc视图
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("代码生成器-视图-Pc视图")]
        public ViewResultBase Pc()
        {
            return View();
        }

        /// <summary>
        ///     表单生成编辑界面
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("代码生成器-视图-表单生成编辑界面")]
        public ViewResultBase PcCodeGenerationFormEdit(string editJson)
        {
            var doubleWay = editJson.JsonStringToObject<SystemCodeGenerationPcEditDoubleWay>();
            return View(doubleWay);
        }

        /// <summary>
        ///     Pc代码生成器界面
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("代码生成器-视图-Pc代码生成器界面")]
        public async Task<ViewResultBase> PcCodeGeneration(SystemDataBaseTableDoubleWay doubleWay)
        {
            return View(await _dataBaseLogic.GetDataBaseColumns(doubleWay));
        }

        #endregion

        #region 方法

        #region 生成代码
        /// <summary>
        ///     生成代码
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> CreateCode(SystemCodeGenerationCreateCodeViewModel viewModel)
        {
            var input = new SystemCodeGenerationInput
            {
                Base = viewModel.Base.JsonStringToObject<SystemCodeGenerationBaseInput>(),
                List = viewModel.List.JsonStringToList<SystemCodeGenerationListForListInput>(),
                Edit = viewModel.Edit.JsonStringToList<SystemCodeGenerationEditInput>()
            };
            var output = new SystemCodeGenerationOutput
            {
                Entities = await CreateEntity(input.Base),
                DataAccessInterface = await CreteDataAccessInterface(input.Base),
                DataAccess = await CreteDataAccess(input.Base),
                BusinessInterface = await CreteLogicInterface(input.Base),
                Business = await CreteLogic(input.Base),
                Controller = await CreateController(input.Base),
                List = await CreateList(input.Base),
                ListJs = await CreateListJs(input.Base, input.List),
                Edit = await CreateEdit(input.Base, input.Edit),
                EditJs = await CreateEditJs(input.Base)
            };
            return Json(output);
        }

        /// <summary>
        ///     生成实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<string> CreateEntity(SystemCodeGenerationBaseInput input)
        {
            //返回值
            var columnContent = new StringBuilder();
            //获取文件内容
            var returnContent = FileUtil.ReadFile(Server.MapPath("/DataUsers/Templates/CodeGeneration/Entities.txt"));
            //获取对应列
            var columns = (await _dataBaseLogic.GetDataBaseColumns(input)).ToList();
            //替换操作

            //1:命名空间
            returnContent = returnContent.Replace("{{NameSpace}}", input.TableName.Split('_')[0]);
            //2:表名
            returnContent = returnContent.Replace("{{TableName}}", input.TableName);
            //3:类名称
            returnContent = returnContent.Replace("{{ClassName}}", input.TableName.Replace("_", ""));
            //4:主键名称
            returnContent = returnContent.Replace("{{KeyName}}", input.TableKey);
            //5:动态生成列
            foreach (var column in columns.Where(column => column.ColumnName != input.TableKey))
            {
                columnContent.Append("        /// <summary>\r\n");
                columnContent.Append(string.Format("        /// {0}\r\n", column.ColumnDescription));
                columnContent.Append("        /// </summary>\r\n");
                columnContent.Append("        public " + ChangeToCSharpType(column.DataType) + " " + column.ColumnName + " { get; set; }\r\n\r\n");
            }
            //6:是否分页
            if (input.IsPaging)
            {
                columnContent.Append("        /// <summary>\r\n");
                columnContent.Append("        /// 记录总数\r\n");
                columnContent.Append("        /// </summary>\r\n");
                columnContent.Append("        /// [Column(IsInsert = false,IsUpdate = false,IsSelect = false,IsSort = false)]\r\n");
                columnContent.Append("        public int RecordCount { get; set; }\r\n");
            }
            returnContent = returnContent.Replace("{{EntitiesBody}}", columnContent.ToString());
            //7:主键类型
            var key = columns.Where(column => column.ColumnName == input.TableKey).FirstOrDefault();
            returnContent = returnContent.Replace("{{GenerationType}}", key != null && key.DataType == "uniqueidentifier" ? "GenerationType.Sequence" : "GenerationType.Indentity");
            //8:描述
            returnContent = returnContent.Replace("{{Description}}", input.Description);
            return returnContent;
        }

        /// <summary>
        /// 生成DataAccess接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<string> CreteDataAccessInterface(SystemCodeGenerationBaseInput input)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(Server.MapPath("/DataUsers/Templates/CodeGeneration/IRepository.txt"));
            //替换操作

            //1:命名空间
            returnContent = returnContent.Replace("{{NameSpace}}", input.TableName.Split('_')[0]);
            //2:表名
            returnContent = returnContent.Replace("{{ClassName}}", input.TableName.Replace("_", ""));
            //3:描述
            returnContent = returnContent.Replace("{{Description}}", input.Description);

            return returnContent;
        }

        /// <summary>
        /// 生成DataAccess接口实现
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<string> CreteDataAccess(SystemCodeGenerationBaseInput input)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(Server.MapPath("/DataUsers/Templates/CodeGeneration/Repository.txt"));
            //替换操作

            //1:命名空间
            returnContent = returnContent.Replace("{{NameSpace}}", input.TableName.Split('_')[0]);
            //2:表名
            returnContent = returnContent.Replace("{{ClassName}}", input.TableName.Replace("_", ""));
            //3:描述
            returnContent = returnContent.Replace("{{Description}}", input.Description);

            return returnContent;
        }

        /// <summary>
        /// 生成Logic接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<string> CreteLogicInterface(SystemCodeGenerationBaseInput input)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(Server.MapPath("/DataUsers/Templates/CodeGeneration/ILogic.txt"));
            //替换操作

            //1:命名空间
            returnContent = returnContent.Replace("{{NameSpace}}", input.TableName.Split('_')[0]);
            //2:表名
            returnContent = returnContent.Replace("{{ClassName}}", input.TableName.Replace("_", ""));
            //3:描述
            returnContent = returnContent.Replace("{{Description}}", input.Description);

            return returnContent;
        }

        /// <summary>
        /// 生成Logic接口实现
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<string> CreteLogic(SystemCodeGenerationBaseInput input)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(Server.MapPath("/DataUsers/Templates/CodeGeneration/Logic.txt"));
            //替换操作

            //1:命名空间
            returnContent = returnContent.Replace("{{NameSpace}}", input.TableName.Split('_')[0]);
            //2:表名
            returnContent = returnContent.Replace("{{ClassName}}", input.TableName.Replace("_", ""));
            //3:表名转换为小写
            returnContent = returnContent.Replace("{{ClassNameLower}}", input.TableName.Replace("_", "").ReplaceFistLower());
            //4:主键
            returnContent = returnContent.Replace("{{KeyName}}", input.TableKey);
            //5:描述
            returnContent = returnContent.Replace("{{Description}}", input.Description);

            return returnContent;
        }

        /// <summary>
        /// 生成控制器
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<string> CreateController(SystemCodeGenerationBaseInput input)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(Server.MapPath("/DataUsers/Templates/CodeGeneration/Controller.txt"));
            //替换操作

            //1:命名空间
            returnContent = returnContent.Replace("{{NameSpace}}", input.TableName.Split('_')[0]);
            //2:表名
            returnContent = returnContent.Replace("{{ClassName}}", input.TableName.Replace("_", ""));
            //3:表名转换为小写
            returnContent = returnContent.Replace("{{ClassNameLower}}", input.TableName.Replace("_", "").ReplaceFistLower());
            //4:控制器名称
            returnContent = returnContent.Replace("{{ControllerName}}", input.TableName.Replace(input.TableName.Split('_')[0], "").Replace("_", ""));
            //描述
            returnContent = returnContent.Replace("{{Description}}", input.Description);

            return returnContent;
        }

        /// <summary>
        /// 生成列表界面
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<string> CreateList(SystemCodeGenerationBaseInput input)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(Server.MapPath("/DataUsers/Templates/CodeGeneration/List.txt"));
            //替换操作

            //1:命名空间
            returnContent = returnContent.Replace("{{NameSpace}}", input.TableName.Split('_')[0].ToLower());
            //2:控制器名称
            returnContent = returnContent.Replace("{{ControllerName}}", input.TableName.Replace(input.TableName.Split('_')[0], "").Replace("_", "").ToLower());
            //3:描述
            returnContent = returnContent.Replace("{{Description}}", input.Description);

            return returnContent;
        }

        /// <summary>
        /// 生成列表界面Js
        /// </summary>
        /// <param name="baseInput"></param>
        /// <param name="listInput"></param>
        /// <returns></returns>
        private async Task<string> CreateListJs(SystemCodeGenerationBaseInput baseInput,
            IEnumerable<SystemCodeGenerationListForListInput> listInput)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(Server.MapPath(baseInput.IsPaging ? "/DataUsers/Templates/CodeGeneration/ListJsPaging.txt" : "/DataUsers/Templates/CodeGeneration/ListJsLoadonce.txt"));
            //替换操作

            //1:命名空间
            returnContent = returnContent.Replace("{{NameSpace}}", baseInput.TableName.Split('_')[0]);
            //2:控制器名称
            returnContent = returnContent.Replace("{{ControllerName}}", baseInput.TableName.Replace(baseInput.TableName.Split('_')[0], "").Replace("_", ""));
            //3:主键名称
            returnContent = returnContent.Replace("{{KeyName}}", baseInput.TableKey);
            //4:列表项
            returnContent = returnContent.Replace("{{ColModel}}", GetColModel(listInput.OrderBy(o => o.OrderNo).ToList(), baseInput));
            //5:描述
            returnContent = returnContent.Replace("{{Description}}", baseInput.Description);
            //6:编辑框高
            returnContent = returnContent.Replace("{{EditHeight}}", baseInput.EditHeight == 0 ? "100%" : baseInput.EditHeight.ToString());
            //7:编辑框宽
            returnContent = returnContent.Replace("{{EditWidth}}", baseInput.EditWidth == 0 ? "100%" : baseInput.EditWidth.ToString());
            return returnContent;
        }

        /// <summary>
        /// 生成编辑界面
        /// </summary>
        /// <param name="baseInput"></param>
        /// <param name="editInputs"></param>
        /// <returns></returns>
        private async Task<string> CreateEdit(SystemCodeGenerationBaseInput baseInput, IEnumerable<SystemCodeGenerationEditInput> editInputs)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(Server.MapPath("/DataUsers/Templates/CodeGeneration/Edit.txt"));
            //替换操作

            //1:命名空间
            returnContent = returnContent.Replace("{{NameSpaceLower}}", baseInput.TableName.Split('_')[0].ToLower());
            returnContent = returnContent.Replace("{{NameSpace}}", baseInput.TableName.Split('_')[0]);
            //2:控制器名称
            returnContent = returnContent.Replace("{{ControllerName}}", baseInput.TableName.Replace(baseInput.TableName.Split('_')[0], "").Replace("_", "").ToLower());
            //3:描述
            returnContent = returnContent.Replace("{{Description}}", baseInput.Description);
            //4:类名称
            returnContent = returnContent.Replace("{{ClassName}}", baseInput.TableName.Replace("_", ""));
            //5:主键名称
            returnContent = returnContent.Replace("{{KeyName}}", baseInput.TableKey);
            //6:主体
            foreach (var edit in editInputs)
            {
                switch (edit.ControlType)
                {
                    case EnumControlType.文本框:

                        break;
                }
            }
            return returnContent;
        }

        /// <summary>
        /// 生成编辑界面Js
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<string> CreateEditJs(SystemCodeGenerationBaseInput input)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(Server.MapPath("/DataUsers/Templates/CodeGeneration/EditJs.txt"));
            //替换操作

            //1:命名空间
            returnContent = returnContent.Replace("{{NameSpace}}", input.TableName.Split('_')[0]);
            //2:控制器名称
            returnContent = returnContent.Replace("{{ControllerName}}", input.TableName.Replace(input.TableName.Split('_')[0], "").Replace("_", ""));
            //3:主键名称
            returnContent = returnContent.Replace("{{KeyName}}", input.TableKey);

            return returnContent;
        }

        /// <summary>
        ///     根据用户的字段权限拼接界面显示字段表达式
        /// </summary>
        /// <param name="listInput"></param>
        /// <param name="baseInput"></param>
        /// <returns>拼接后的表达式</returns>
        private string GetColModel(IList<SystemCodeGenerationListForListInput> listInput, SystemCodeGenerationBaseInput baseInput)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("[\r\n");
            foreach (SystemCodeGenerationListForListInput input in listInput)
            {
                stringBuilder.Append("            {");

                stringBuilder.Append("name:\"" + input.Name + "\",");
                //是否隐藏
                if (!input.Hidden)
                {
                    stringBuilder.Append("label:\"" + input.Label + "\",");
                    //是否分页
                    if (baseInput.IsPaging)
                    {
                        if (!string.IsNullOrEmpty(input.Index))
                        {
                            stringBuilder.Append("index:\"" + input.Index + "\",");
                        }
                    }

                    //是否自适应
                    if (input.Fixed)
                    {
                        stringBuilder.Append("fixed:true,");
                    }
                    else
                    {
                        stringBuilder.Append("width:" + input.Width + ",");
                    }

                    //是否排序
                    if (input.Sortable)
                    {
                        if (!input.Sorttype.IsNullOrEmpty())
                        {
                            stringBuilder.Append("sorttype:\"" + input.Sorttype.ToLower() + "\",");
                        }
                    }
                    else
                    {
                        stringBuilder.Append("sortable:false,");
                    }

                    //对齐格式
                    if (input.Align.IsNullOrEmpty())
                    {
                        input.Align = EnumFieldAlign.Center.ToString().ToLower();
                    }

                    //格式化
                    if (!input.Formatter.IsNullOrEmpty())
                    {
                        stringBuilder.Append("align:\"" + input.Align + "\",");
                        stringBuilder.Append("formatter:\"" + input.Formatter + "\"},\r\n");
                    }
                    else
                    {
                        stringBuilder.Append("align:\"" + input.Align + "\"");
                        stringBuilder.Append(input == listInput.Last() ? "}\r\n" : "},\r\n");
                    }
                }
                else
                {
                    stringBuilder.Append("hidden:true");
                }
            }
            string returnStr = stringBuilder.ToString().TrimEnd(',');
            returnStr = returnStr + "        ]";
            return returnStr;
        }

        /// <summary>
        ///     数据库中与C#中的数据类型对照
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string ChangeToCSharpType(string type)
        {
            string reval;
            switch (type.ToLower())
            {
                case "int":
                case "bigint":
                case "smallint":
                    reval = "int";
                    break;
                case "text":
                case "char":
                case "nchar":
                case "ntext":
                case "nvarchar":
                case "varchar":
                    reval = "string";
                    break;
                case "binary":
                case "varbinary":
                case "tinyint":
                case "image":
                    reval = "byte[]";
                    break;
                case "bit":
                    reval = "bool";
                    break;
                case "datetime":
                case "smalldatetime":
                case "timestamp":
                    reval = "DateTime";
                    break;
                case "decimal":
                case "smallmoney":
                case "money":
                case "numeric":
                    reval = "decimal";
                    break;
                case "float":
                    reval = "double";
                    break;
                case "real":
                    reval = "System.Single";
                    break;
                case "uniqueidentifier":
                    reval = "Guid";
                    break;
                case "Variant":
                    reval = "Object";
                    break;
                default:
                    reval = "String";
                    break;
            }
            return reval;
        }
        #endregion

        #region 生成文件
        /// <summary>
        /// 生成文件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult CreateFile(SystemCodeGenerationCreateFileViewModel model)
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                SystemCodeGenerationBaseInput baseInput = model.Base.JsonStringToObject<SystemCodeGenerationBaseInput>();
                //实体
                FileUtil.WriteFile(baseInput.EntityPath + "\\" + baseInput.Entity + ".cs", model.Entities);
                //DataAccess接口
                FileUtil.WriteFile(baseInput.DataAccessInterfacePath + "\\" + baseInput.DataAccessInterface + ".cs", model.DataAccessInterface);
                //DataAccess实现
                FileUtil.WriteFile(baseInput.DataAccessPath + "\\" + baseInput.DataAccess + ".cs", model.DataAccess);
                //Business接口
                FileUtil.WriteFile(baseInput.BusinessInterfacePath + "\\" + baseInput.BusinessInterface + ".cs", model.BusinessInterface);
                //Business实现
                FileUtil.WriteFile(baseInput.BusinessPath + "\\" + baseInput.Business + ".cs", model.Business);
                //控制器
                FileUtil.WriteFile(baseInput.ControllerPath + "\\" + baseInput.Controller + ".cs", model.Controller);
                //控制器
                FileUtil.WriteFile(baseInput.ControllerPath.Replace("Controllers", "Views") + "\\" + baseInput.Controller.Replace("Controller", "") + "\\" + baseInput.List + ".cshtml", model.List);
                //控制器
                FileUtil.WriteFile(baseInput.ControllerPath.Replace("Controllers", "Views") + "\\" + baseInput.Controller.Replace("Controller", "") + "\\" + baseInput.Edit + ".cshtml", model.Edit);
                //列表Js
                FileUtil.WriteFile(baseInput.ListJsPath + "\\" + baseInput.ListJs + ".js", model.ListJs);
                //编辑Js
                FileUtil.WriteFile(baseInput.EditJsPath + "\\" + baseInput.EditJs + ".js", model.EditJs);
            }
            catch (Exception ex)
            {
                operateStatus.Message = ex.Message;
                return Json(operateStatus);
            }
            operateStatus.Message = "生成成功";
            operateStatus.ResultSign = ResultSign.Successful;
            return Json(operateStatus);
        }

        #endregion

        #region 菜单按钮

        /// <summary>
        /// 保存菜单按钮
        /// </summary>
        /// <param name="buttons">菜单按钮</param>
        /// <returns></returns>
        public async Task<JsonResult> SaveMenuButton(string buttons)
        {
            IEnumerable<SystemMenuButton> menuButtons = buttons.JsonStringToList<SystemMenuButton>();
            return Json(await _menuButtonLogic.InsertMultipleDapperAsync(menuButtons));
        }

        #endregion

        #endregion

        #endregion
    }
}