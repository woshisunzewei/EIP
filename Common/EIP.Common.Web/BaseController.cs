using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using EIP.Common.Core.Auth;
using EIP.Common.Core.Log;
using EIP.Common.Entities;
using EIP.Common.Entities.Paging;
using EIP.Common.Entities.Tree;
using EIP.Common.Web.Attributes;
using SystemWeb = System.Web;
namespace EIP.Common.Web
{
    /// <summary>
    ///     基础控制器
    /// </summary>
    /// <summary>
    ///     基础控制器
    /// </summary>
    [LoginFilter]
    [AuthorizeFilter]
    [ExceptionFilter]
    public class BaseController : Controller
    {
        #region 初始化

        /// <summary>
        ///     重写Controller,此处可写入登录日志
        /// </summary>
        /// <param name="requestContext">上下文对象</param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            //从Cookie里面获取用户信息
            CurrentUser = FormAuthenticationExtension.Current(SystemWeb.HttpContext.Current.Request);
            if (CurrentUser != null)
            {
                _operationLogHandler = new OperationLogHandler(Request)
                {
                    log =
                    {
                        CreateUserCode = CurrentUser.Code,
                        CreateUserName = CurrentUser.Name
                    }
                };
            }
        }

        #endregion

        #region 错误

        /// <summary>
        ///     当请求与此控制器匹配但在此控制器中找不到任何具有指定操作名称的方法时调用
        /// </summary>
        /// <param name="actionName"></param>
        protected override void HandleUnknownAction(string actionName)
        {
            IController errorController = new ErrorController();
            var errorRoute = new RouteData();
            errorRoute.Values.Add("controller", "Error");
            errorRoute.Values.Add("action", "ActionUnknown");
            if (Request.Url != null) errorRoute.Values.Add("url", Request.Url.OriginalString);
            errorRoute.Values.Add("unkonwnAction", actionName);
            errorController.Execute(new RequestContext(HttpContext, errorRoute));
        }

        /// <summary>
        ///     出现错误
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            //跳转页面
            IController errorController = new ErrorController();
            var errorRoute = new RouteData();
            errorRoute.Values.Add("controller", "Error");
            errorRoute.Values.Add("action", "Exception");
            errorRoute.Values.Add("msg", filterContext.Exception);
            errorController.Execute(new RequestContext(HttpContext, errorRoute));
            filterContext.ExceptionHandled = true;
            base.OnException(filterContext);
        }

        #endregion

        #region 属性

        /// <summary>
        ///     当前登录用户信息
        /// </summary>
        protected virtual PrincipalUser CurrentUser { get; set; }

        /// <summary>
        ///     用户操作日志
        /// </summary>
        private OperationLogHandler _operationLogHandler;

        #endregion

        #region Action

        /// <summary>
        ///     Action开始执行触发
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //获取Action特性
            var descriptionAttribute = filterContext.ActionDescriptor.GetCustomAttributes(
                typeof(DescriptionAttribute), false);
            if (descriptionAttribute.Any())
            {
                var info = descriptionAttribute[0] as DescriptionAttribute;
                if (info != null)
                {
                    var description = info.Description;
                    _operationLogHandler.log.ControllerName =
                        filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;
                    _operationLogHandler.log.ActionName = filterContext.ActionDescriptor.ActionName;
                    _operationLogHandler.log.Describe = description;
                }
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        ///     Action结束执行触发
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (!string.IsNullOrEmpty(_operationLogHandler.log.Describe))
            {
                _operationLogHandler.ActionExecuted();
            }
        }

        /// <summary>
        ///     开始返回结果
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            //记录日志
            if (filterContext.Result.GetType() == typeof(RedirectResult))
            {
                if (!string.IsNullOrEmpty(_operationLogHandler.log.Describe))
                {
                    _operationLogHandler.ActionExecuted();
                }
            }
        }

        /// <summary>
        ///     结果返回结束
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            //记录日志
            if (!string.IsNullOrEmpty(_operationLogHandler.log.Describe))
            {
                _operationLogHandler.ResultExecuted(Response);
                _operationLogHandler.WriteLog();
            }
        }

        #endregion

        #region Json

        /// <summary>
        ///     加载Json树节点
        /// </summary>
        /// <param name="treeEntitys">TreeEntity的集合</param>
        /// <param name="isexpand">各节点是否展开</param>
        /// <returns></returns>
        protected JsonResult JsonForWdTree(IList<TreeEntity> treeEntitys,
            bool isexpand = false)
        {
            IList<WdTreePermission> permissions = null;

            if (treeEntitys == null || treeEntitys.Count <= 0)
            {
                permissions = new List<WdTreePermission>();
                return Json(permissions, JsonRequestBehavior.AllowGet);
            }
            //判断有多少个模块
            IList<TreeEntity> roots = treeEntitys.Where(f => f.pId.ToString() == Guid.Empty.ToString()).ToList();
            if (roots.Count <= 0)
            {
                permissions = new List<WdTreePermission>();
                return Json(permissions, JsonRequestBehavior.AllowGet);
            }

            //总模块
            permissions = new List<WdTreePermission>(roots.Count);
            foreach (var root in roots)
            {
                permissions.Add(new WdTreePermission
                {
                    id = Guid.Parse(root.id.ToString()),
                    icon = root.icon,
                    name = root.name
                });
            }
            //便利子模块
            foreach (var permission in permissions)
            {
                //判断有多少个模块
                IList<TreeEntity> perRoots =
                    treeEntitys.Where(f => f.pId.ToString() == permission.id.ToString()).ToList();
                IList<WdTreeEntity> trees = new List<WdTreeEntity>();
                WdTreeEntity tree = null;
                foreach (var treeEntity in perRoots)
                {
                    tree = new WdTreeEntity(treeEntity);
                    tree.url = string.IsNullOrEmpty(tree.url) ? "" : tree.url;
                    tree.ChildNodes = GetWdChildNodes(ref treeEntitys, isexpand, treeEntity);
                    trees.Add(tree);
                }
                permission.tree = trees;
                tree = null;
            }

            return Json(permissions, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     根据当前节点，加载子节点
        /// </summary>
        /// <param name="treeEntitys">TreeEntity的集合</param>
        /// <param name="isexpand">各节点是否展开</param>
        /// <param name="currTreeEntity">当前节点</param>
        private IList<WdTreeEntity> GetWdChildNodes(ref IList<TreeEntity> treeEntitys,
            bool isexpand,
            TreeEntity currTreeEntity)
        {
            IList<TreeEntity> childNodes =
                treeEntitys.Where(f => f.pId.ToString() == currTreeEntity.id.ToString()).ToList();
            if (childNodes.Count <= 0)
            {
                return null;
            }
            IList<WdTreeEntity> childTrees = new List<WdTreeEntity>(childNodes.Count);
            WdTreeEntity tree = null;
            foreach (var treeEntity in childNodes)
            {
                tree = new WdTreeEntity(treeEntity);
                tree.url = string.IsNullOrEmpty(tree.url) ? "" : tree.url;
                tree.isexpand = isexpand;
                tree.ChildNodes = GetWdChildNodes(ref treeEntitys, isexpand, treeEntity);
                childTrees.Add(tree);
            }
            return childTrees;
        }

        /// <summary>
        ///     加载Json树节点:ZTree(排除连接不上部分)
        /// </summary>
        /// <param name="treeEntitys">TreeEntity的集合</param>
        /// <returns></returns>
        protected JsonResult JsonForZtree(IList<TreeEntity> treeEntitys)
        {
            if (treeEntitys == null || treeEntitys.Count <= 0)
            {
                treeEntitys = new List<TreeEntity>();
                return Json(treeEntitys, JsonRequestBehavior.AllowGet);
            }
            //判断有多少个模块
            IList<TreeEntity> roots = treeEntitys.Where(f => f.pId.ToString() == Guid.Empty.ToString()).ToList();
            if (roots.Count <= 0)
            {
                treeEntitys = new List<TreeEntity>();
                return Json(treeEntitys, JsonRequestBehavior.AllowGet);
            }
            //总模块
            IList<TreeEntity> returnTreeEntities = new List<TreeEntity>(treeEntitys.Count);

            //便利子模块
            foreach (var permission in roots)
            {
                returnTreeEntities.Add(new TreeEntity
                {
                    id = Guid.Parse(permission.id.ToString()),
                    pId = Guid.Parse(permission.pId.ToString()),
                    icon = permission.icon,
                    name = permission.name
                });

                //判断有多少个模块
                IList<TreeEntity> perRoots =
                    treeEntitys.Where(f => f.pId.ToString() == permission.id.ToString()).ToList();
                foreach (var treeEntity in perRoots)
                {
                    returnTreeEntities.Add(treeEntity);
                    GetZtreeChildNodes(ref treeEntitys, ref returnTreeEntities, treeEntity);
                }
            }
            return Json(returnTreeEntities, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     根据当前节点，加载子节点
        /// </summary>
        /// <param name="treeEntitys">TreeEntity的集合</param>
        /// <param name="returnTreeEntities"></param>
        /// <param name="currTreeEntity">当前节点</param>
        private void GetZtreeChildNodes(ref IList<TreeEntity> treeEntitys,
            ref IList<TreeEntity> returnTreeEntities,
            TreeEntity currTreeEntity)
        {
            IList<TreeEntity> childNodes =
                treeEntitys.Where(f => f.pId.ToString() == currTreeEntity.id.ToString()).ToList();
            foreach (var treeEntity in childNodes)
            {
                returnTreeEntities.Add(treeEntity);
                GetZtreeChildNodes(ref treeEntitys, ref returnTreeEntities, treeEntity);
            }
        }

        /// <summary>
        ///     读取树结构:排除下级
        /// </summary>
        /// <param name="treeEntitys">TreeEntity的集合</param>
        /// <param name="id">需要排除下级的id</param>
        /// <returns></returns>
        protected JsonResult JsonForZtreeRemoveChildren(IList<TreeEntity> treeEntitys, Guid? id = null)
        {
            if (treeEntitys == null || treeEntitys.Count <= 0)
            {
                treeEntitys = new List<TreeEntity>();
                return Json(treeEntitys, JsonRequestBehavior.AllowGet);
            }
            if (id == null)
            {
                return Json(treeEntitys, JsonRequestBehavior.AllowGet);
            }
            //判断有多少个模块
            IList<TreeEntity> roots = treeEntitys.Where(f => f.pId.ToString() == Guid.Empty.ToString()).ToList();
            if (roots.Count <= 0)
            {
                treeEntitys = new List<TreeEntity>();
                return Json(treeEntitys, JsonRequestBehavior.AllowGet);
            }
            //总模块
            IList<TreeEntity> returnTreeEntities = new List<TreeEntity>(treeEntitys.Count);

            //便利子模块
            foreach (var permission in roots)
            {
                if (id.ToString() != permission.id.ToString())
                {
                    returnTreeEntities.Add(new TreeEntity
                    {
                        id = Guid.Parse(permission.id.ToString()),
                        pId = Guid.Parse(permission.pId.ToString()),
                        icon = permission.icon,
                        name = permission.name,
                        code = permission.code
                    });

                    //判断有多少个模块
                    IList<TreeEntity> perRoots =
                        treeEntitys.Where(f => f.pId.ToString() == permission.id.ToString()).ToList();
                    foreach (var treeEntity in perRoots)
                    {
                        if (id.ToString() != treeEntity.id.ToString())
                        {
                            returnTreeEntities.Add(treeEntity);
                            GetZtreeChildNodesChildren(ref treeEntitys, ref returnTreeEntities, treeEntity, id);
                        }
                    }
                }
            }
            return Json(returnTreeEntities, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     根据当前节点，加载子节点
        /// </summary>
        /// <param name="treeEntitys">TreeEntity的集合</param>
        /// <param name="returnTreeEntities"></param>
        /// <param name="currTreeEntity">当前节点</param>
        /// <param name="id">需要排除下级的id</param>
        private void GetZtreeChildNodesChildren(ref IList<TreeEntity> treeEntitys,
            ref IList<TreeEntity> returnTreeEntities,
            TreeEntity currTreeEntity
            , Guid? id = null)
        {
            IList<TreeEntity> childNodes =
                treeEntitys.Where(f => f.pId.ToString() == currTreeEntity.id.ToString()).ToList();
            foreach (var treeEntity in childNodes)
            {
                if (id.ToString() != treeEntity.id.ToString())
                {
                    returnTreeEntities.Add(treeEntity);
                    GetZtreeChildNodesChildren(ref treeEntitys, ref returnTreeEntities, treeEntity, id);
                }
            }
        }

        /// <summary>
        ///     重写基础的Json
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        protected override JsonResult Json(object data,
            string contentType,
            Encoding contentEncoding,
            JsonRequestBehavior behavior)
        {
            return new JsonResultExtension
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                Format = "yyyy-MM-dd HH:mm:ss"
            };
        }

        /// <summary>
        ///     一次性加载数据返回Json数据格式,此方法可从后台去除一次性大数据(默认时间格式为"yyyy-MM-dd HH:mm:ss")
        /// </summary>
        /// <typeparam name="T">业务实体</typeparam>
        /// <param name="data">集合数据</param>
        /// <returns>执行结果数据</returns>
        protected ActionResult JsonForGridLoadonce<T>(IList<T> data)
        {
            return new ContentResult
            {
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(data),
                ContentType = "application/json"
            };
        }

        /// <summary>
        ///     返回分页后信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pagedResults"></param>
        /// <returns></returns>
        protected JsonResult JsonForGridPaging<T>(PagedResults<T> pagedResults)
        {
            return Json(new
            {
                total = pagedResults.PagerInfo.PageCount,
                page = pagedResults.PagerInfo.Page,
                records = pagedResults.PagerInfo.RecordCount,
                rows = pagedResults.Data
            });
        }

        /// <summary>
        ///     返回分页后信息
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="pagedResults">数据</param>
        /// <param name="format">自定义的格式</param>
        /// <returns>Json数据</returns>
        protected JsonResult JsonForGridPaging<T>(PagedResults<T> pagedResults,
            string format)
        {
            return new JsonResultExtension
            {
                Data =
                    new
                    {
                        total = pagedResults.PagerInfo.PageCount,
                        page = pagedResults.PagerInfo.Page,
                        records = pagedResults.PagerInfo.RecordCount,
                        rows = pagedResults.Data
                    },
                ContentType = "application/json",
                Format = format
            };
        }

        /// <summary>
        ///     自定义时间格式返回Json
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="format">时间格式</param>
        /// <returns>Json数据</returns>
        protected new JsonResult Json(object data,
            string format)
        {
            return new JsonResultExtension
            {
                Data = data,
                Format = format
            };
        }

        /// <summary>
        ///     检查值是否相同
        /// </summary>
        /// <param name="operateStatus"></param>
        /// <returns></returns>
        protected JsonResult JsonForCheckSameValue(OperateStatus operateStatus)
        {
            return Json(new
            {
                info = operateStatus.Message,
                status = operateStatus.ResultSign == ResultSign.Successful ? "y" : "n"
            });
        }

        #endregion
    }
}