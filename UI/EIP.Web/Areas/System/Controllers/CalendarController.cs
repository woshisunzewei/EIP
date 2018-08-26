using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using EIP.Common.Web;
using EIP.System.Business.Config;
using EIP.System.Models.Entities;

namespace EIP.Web.Areas.System.Controllers
{
    public class CalendarController : BaseController
    {
        /// <summary>
        /// 我的日历
        /// </summary>
        /// <returns></returns>
        public async Task<ViewResultBase> MyCalendar()
        {
            var format = new CalendarViewFormat(CalendarViewType.week, DateTime.Now, DayOfWeek.Monday);
            return View((await _calendarLogic.QueryCalendars(format.StartDate, format.EndDate, CurrentUser.UserId)).ToList());
        }
        
        /// <summary>
        /// 日历详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ViewResultBase> ViewCalendar(int? id)
        {
            SystemCalendar calendar = id.HasValue ? await _calendarLogic.GetByIdAsync((id.Value)) : new SystemCalendar();
            return View(calendar);
        }

        /// <summary>
        /// 编辑日历
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="isallday"></param>
        /// <returns></returns>
        public async Task<ViewResultBase> EditCalendar(int? id,
            string start,
            string end,
            int? isallday)
        {
            SystemCalendar calendar;
            if (id.HasValue && id > 0)
            {
                calendar = await _calendarLogic.GetByIdAsync(id.Value);
                int clientzone = calendar.MasterId.HasValue ? calendar.MasterId.Value : 8;
                int serverzone = TimeHelper.GetTimeZone();
                var zonediff = clientzone - serverzone;
                //时区转换
                calendar.StartTime = calendar.StartTime.AddHours(zonediff);
                calendar.EndTime = calendar.EndTime.AddHours(zonediff);
            }
            else
            {
                calendar = new SystemCalendar();
                calendar.StartTime = Convert.ToDateTime(start);
                calendar.EndTime = Convert.ToDateTime(end);
                calendar.IsAllDayEvent = isallday.HasValue && isallday.Value == 1;

            }
            return View(calendar);
        }

        /// <summary>
        /// 保存日历
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> SaveCalendar(int id, FormCollection form)
        {
            JsonReturnMessages r = new JsonReturnMessages();
            try
            {
                SystemCalendar data;
                if (id > 0)
                {
                    data = await _calendarLogic.GetByIdAsync(id);
                }
                else
                {
                    data = new SystemCalendar();
                }
                data.Subject = form["Subject"];
                data.Location = form["Location"];
                data.Description = form["Description"];
                data.IsAllDayEvent = form["IsAllDayEvent"] != "false";
                data.Category = form["colorvalue"];
                string sdate = form["stpartdate"];
                string edate = form["etpartdate"];
                string stime = form["stparttime"];
                string etime = form["etparttime"];
                int clientzone = Convert.ToInt32(form["timezone"]);
                int serverzone = TimeHelper.GetTimeZone();
                var zonediff = serverzone - clientzone;

                if (data.IsAllDayEvent)
                {
                    data.StartTime = Convert.ToDateTime(sdate).AddHours(zonediff);
                    data.EndTime = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59).AddHours(zonediff);
                }
                else
                {
                    data.StartTime = Convert.ToDateTime(sdate + " " + stime).AddHours(zonediff);
                    data.EndTime = Convert.ToDateTime(edate + " " + etime).AddHours(zonediff);
                }
                if (data.EndTime <= data.StartTime)
                {
                    throw new Exception("开始时间不能大于结束时间");
                }
                data.CalendarType = 1;
                data.InstanceType = 0;
                data.MasterId = clientzone;
                data.UPAccount = CurrentUser.UserId;
                data.UPName = CurrentUser.Name;
                data.UPTime = DateTime.Now;

                if (data.Id > 0)
                {
                    await _calendarLogic.UpdateAsync(data);
                }
                else
                {
                    await _calendarLogic.InsertAsync(data);
                }
                r.IsSuccess = true;
                r.Msg = "操作成功!";
            }
            catch (Exception ex)
            {
                r.IsSuccess = false;
                r.Msg = ex.Message;
            }
            return Json(r);
        }

        /// <summary>
        /// 快速删除个人日程
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> QuickDeletePersonalCal(FormCollection form)
        {
            JsonReturnMessages r = new JsonReturnMessages();
            string id = form["calendarId"];
            if (string.IsNullOrEmpty(id))
            {
                r.IsSuccess = false;
                r.Msg = "参数id无效";
                return Json(r);
            }
            try
            {
                await _calendarLogic.DeleteAsync(Convert.ToInt32(id));
                r.IsSuccess = true;
                r.Msg = "操作成功!";
            }
            catch (Exception ex)
            {
                r.IsSuccess = false;
                r.Msg = ex.Message;
            }
            return Json(r);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<string> GetPersonalCalViewData(FormCollection form)
        {
            CalendarViewType viewType = (CalendarViewType)Enum.Parse(typeof(CalendarViewType), form["viewtype"]);
            string strshowday = form["showdate"];
            DateTime showdate;
            int clientzone = Convert.ToInt32(form["timezone"]);
            int serverzone = TimeHelper.GetTimeZone();
            var zonediff = serverzone - clientzone;
            bool b = DateTime.TryParse(strshowday, out showdate);
            var jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;//增加最大长度
            if (!b)
            {
                var ret = new JsonCalendarViewData(new JsonError("NotVolidDateTimeFormat", "错误的时间格式"));
                return jss.Serialize(ret);
            }
            var format = new CalendarViewFormat(viewType, showdate, DayOfWeek.Monday);
            var qstart = format.StartDate.AddHours(zonediff);
            var qend = format.EndDate.AddHours(zonediff);
            List<SystemCalendar> list = (await _calendarLogic.QueryCalendars(qstart, qend, CurrentUser.UserId)).ToList();
            var data = new JsonCalendarViewData(ConvertToStringArray(list), format.StartDate, format.EndDate);
            return jss.Serialize(data);
        }

        /// <summary>
        /// 快速新增
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> QuickAddPersonalCal(FormCollection form)
        {
            JsonReturnMessages r = new JsonReturnMessages();
            string subject = form["CalendarTitle"];
            string strStartTime = form["CalendarStartTime"];
            string strEndTime = form["CalendarEndTime"];
            string isallday = form["IsAllDayEvent"];
            int clientzone = Convert.ToInt32(form["timezone"]);
            int serverzone = TimeHelper.GetTimeZone();
            var zonediff = serverzone - clientzone;
            DateTime st;
            DateTime et;

            bool a = DateTime.TryParse(strEndTime, out et);
            bool b = DateTime.TryParse(strStartTime, out st);
            if (!b)
            {
                r.IsSuccess = false;
                r.Msg = "错误的时间格式" + ":" + strStartTime;
                return Json(r);
            }
            if (!a)
            {
                r.IsSuccess = false;
                r.Msg = "错误的时间格式" + ":" + strEndTime;
                return Json(r);
            }

            try
            {
                SystemCalendar entity = new SystemCalendar();
                entity.CalendarType = 1;
                entity.InstanceType = 0;
                entity.Subject = subject;
                entity.StartTime = st.AddHours(zonediff);
                entity.EndTime = et.AddHours(zonediff);
                entity.IsAllDayEvent = isallday == "1";
                entity.UPAccount = CurrentUser.UserId;
                entity.UPName = CurrentUser.Name;
                entity.UPTime = DateTime.Now;
                entity.MasterId = clientzone;
                r.Data = (await _calendarLogic.InsertScalarAsync(entity)).Data;
                r.IsSuccess = true;
                r.Msg = "操作成功!";
            }
            catch (Exception ex)
            {
                r.IsSuccess = false;
                r.Msg = ex.Message;
            }
            return Json(r);
        }

        /// <summary>
        /// 快速更新
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> QuickUpdatePersonalCal(FormCollection form)
        {
            JsonReturnMessages r = new JsonReturnMessages();
            string id = form["calendarId"];
            string strStartTime = form["CalendarStartTime"];
            string strEndTime = form["CalendarEndTime"];
            int clientzone = Convert.ToInt32(form["timezone"]);
            int serverzone = TimeHelper.GetTimeZone();
            var zonediff = serverzone - clientzone;
            DateTime st, et;

            bool a = DateTime.TryParse(strStartTime, out st);
            bool b = DateTime.TryParse(strEndTime, out et);
            if (!a || !b)
            {
                r.IsSuccess = false;
                r.Msg = "错误的时间格式" + ":" + strStartTime;
                return Json(r);
            }
            try
            {
                SystemCalendar c = await _calendarLogic.GetByIdAsync(Convert.ToInt32(id));
                c.StartTime = st.AddHours(zonediff);
                c.EndTime = et.AddHours(zonediff);
                c.MasterId = clientzone;
                await _calendarLogic.UpdateAsync(c);
                r.IsSuccess = true;
                r.Msg = "操作成功!";
            }
            catch (Exception ex)
            {
                r.IsSuccess = false;
                r.Msg = ex.Message;
            }
            return Json(r);
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static List<object[]> ConvertToStringArray(ICollection<SystemCalendar> list)
        {
            List<object[]> relist = new List<object[]>();

            if (list != null && list.Count > 0)
            {
                int serverzone = TimeHelper.GetTimeZone();
                foreach (SystemCalendar entity in list)
                {

                    int clientzone = entity.MasterId.HasValue ? entity.MasterId.Value : 8;

                    var zonediff = clientzone - serverzone;
                    //时区转换
                    var s = entity.StartTime.AddHours(zonediff);
                    var e = entity.EndTime.AddHours(zonediff);
                    var attends = entity.AttendeeNames + (string.IsNullOrEmpty(entity.OtherAttendee) ? "" : "," + entity.OtherAttendee);
                    relist.Add(new object[] { entity.Id, 
                       entity.Subject, 
                       entity.StartTime, 
                       entity.EndTime, 
                       entity.IsAllDayEvent ?1: 0, 
                       s.ToShortDateString() != e.ToShortDateString() ?1 : 0,
                       entity.InstanceType== 2?1:0,
                       string.IsNullOrEmpty(entity.Category)?-1:Convert.ToInt32(entity.Category)
                       ,1,
                       entity.Location,attends });
                }
            }
            return relist;
        }

        #region 构造函数

        private readonly ISystemCalendarLogic _calendarLogic;

        public CalendarController(ISystemCalendarLogic calendarLogic)
        {
            _calendarLogic = calendarLogic;
        }

        #endregion
    }

    public class JsonError
    {
        public JsonError(string code, string msg)
        {
            ErrorCode = code;
            msg = ErrorMsg;
        }

        public string ErrorCode { get; private set; }
        public string ErrorMsg { get; private set; }
    }

    public class JsonCalendarViewData
    {
        public JsonCalendarViewData(List<object[]> eventList, DateTime startDate, DateTime endDate)
        {
            events = eventList;
            start = startDate;
            end = endDate;
            issort = true;
        }

        public JsonCalendarViewData(List<object[]> eventList, DateTime startDate, DateTime endDate, bool isSort)
        {
            start = startDate;
            end = endDate;
            events = eventList;
            issort = isSort;
        }
        public JsonCalendarViewData(JsonError jsonError)
        {
            error = jsonError;
        }
        public List<object[]> events { get; private set; }
        public bool issort
        {
            get;
            private set;
        }

        public DateTime start { get; private set; }
        public DateTime end { get; private set; }
        public JsonError error { get; private set; }
    }

    public class JsonReturnMessages
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is success.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is success; otherwise, <c>false</c>.
        /// </value>

        public bool IsSuccess
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the MSG.
        /// </summary>
        /// <value>The MSG.</value>

        public string Msg
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>The public key.</value>

        public object Data
        {
            get;
            set;
        }
    }

    /// <summary>
    ///     格式化
    /// </summary>
    public class CalendarViewFormat
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CalendarViewFormat" /> class.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        /// <param name="showday">The showday.</param>
        /// <param name="weekStartDay">The week start day.</param>
        public CalendarViewFormat(CalendarViewType viewType, DateTime showday, DayOfWeek weekStartDay)
        {
            int index, w;
            switch (viewType)
            {
                case CalendarViewType.day: //日
                    StartDate = showday.Date;
                    EndDate = showday.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                    break;
                case CalendarViewType.week: // 周            
                    index = weekStartDay.GetHashCode(); //0                  
                    w = index - showday.DayOfWeek.GetHashCode(); //0-1
                    if (w > 0) w = w - 7;
                    StartDate = showday.AddDays(w).Date;
                    EndDate = StartDate.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);
                    break;
                case CalendarViewType.month: // 月         
                    var firstdate = new DateTime(showday.Year, showday.Month, 1);
                    index = weekStartDay.GetHashCode(); //0
                    w = index - firstdate.DayOfWeek.GetHashCode(); //0-1
                    if (w > 0)
                    {
                        w -= 7;
                    }
                    StartDate = firstdate.AddDays(w).Date;
                    EndDate = StartDate.AddDays(34);

                    if (EndDate.Year == showday.Year && EndDate.Month == showday.Month &&
                        EndDate.AddDays(1).Month == EndDate.Month)
                    {
                        EndDate = EndDate.AddDays(7);
                    }
                    EndDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                    break;
            }
        }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
    }

    /// <summary>
    ///     日历枚举
    /// </summary>
    public enum CalendarViewType
    {
        day,
        week,
        workweek,
        month
    }

    public class TimeHelper
    {
        /// <summary>
        ///     Millis the time stamp.
        /// </summary>
        /// <param name="theDate">The date.</param>
        /// <returns></returns>
        public static long MilliTimeStamp(DateTime theDate)
        {
            var d1 = new DateTime(1970, 1, 1);
            var d2 = theDate.ToUniversalTime();
            var ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        ///     Gets the time zone.
        /// </summary>
        /// <returns></returns>
        public static int GetTimeZone()
        {
            var now = DateTime.Now;
            var utcnow = now.ToUniversalTime();
            var sp = now - utcnow;
            return sp.Hours;
        }

        public static int CheckIsCrossEvent(SystemCalendar calendar)
        {
            var serverzone = GetTimeZone();
            var clientzone = calendar.MasterId.HasValue ? calendar.MasterId.Value : 8;

            var zonediff = clientzone - serverzone;
            //时区转换
            var s = calendar.StartTime.AddHours(zonediff);
            var e = calendar.EndTime.AddHours(zonediff);
            return s.ToShortDateString() != e.ToShortDateString() ? 1 : 0;
        }
    }
}