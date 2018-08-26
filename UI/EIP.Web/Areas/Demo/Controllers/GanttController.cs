using System.Web.Mvc;

namespace EIP.Web.Areas.Demo.Controllers
{
    /// <summary>
    /// 甘特图
    /// </summary>
    public class GanttController : Controller
    {
        #region JQuery.Gantt
        /// <summary>
        /// JQuery.Gantt
        /// </summary>
        /// <returns></returns>
        public ViewResultBase JQueryGantt()
        {
            return View();
        }
        #endregion

        #region ExtGantt

        /// <summary>
        /// ExtGantt
        /// </summary>
        /// <returns></returns>
        public ViewResultBase ExtGantt()
        {
            return View();
        }

        public JsonResult Get()
        {
            return Json(null);
        }

        public JsonResult Create()
        {
            return Json(null);
        }

        public JsonResult Delete()
        {
             return Json(null);
        }

        public JsonResult Update()
        {
            return Json(null);
        }
        #endregion
    }
}
