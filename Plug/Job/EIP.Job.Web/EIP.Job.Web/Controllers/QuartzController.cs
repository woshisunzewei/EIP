using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Core.Quartz;
using EIP.Common.Core.Quartz.Dtos;
using EIP.Common.Entities;
using EIP.Common.Web;
using EIP.Job.Service.System.Dto;
using EIP.Job.Web.Models;
using Quartz;
using Quartz.Impl.Calendar;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;

namespace EIP.Job.Web.Controllers
{
    //<summary>
    //    基础任务调度控制器
    //</summary>
    public class QuartzController : BaseController
    {
        #region 调度作业

        /// <summary>
        ///     调度列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("任务调度-视图-列表")]
        public ViewResultBase JobList()
        {
            return View();
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("任务调度-视图-编辑")]
        public ViewResultBase JobEdit(JobDetailInput input)
        {
            var output = new SystemQuartzOutput();
            if (!input.JobName.IsNullOrEmpty() && !input.JobGroup.IsNullOrEmpty())
            {

                var key = new JobKey(input.JobName, input.JobGroup);
                //作业详情
                var detal = StdSchedulerManager.GetJobDetail(key);
                //触发器
                var triggerKey = new TriggerKey(input.TriggerName, input.TriggerGroup);
                var trigger = StdSchedulerManager.GetTrigger(triggerKey);

                output.JobType = detal.JobType.FullName;
                output.JobGroup = detal.Key.Group;
                output.JobName = detal.Key.Name;
                output.JobDescription = detal.Description;

                output.TriggerType = trigger.GetType().Name;
                output.TriggerName = trigger.Key.Name;
                output.TriggerGroup = trigger.Key.Group;
                output.TriggerDescription = trigger.Description;

                //获取trigger类型
                switch (trigger.GetType().Name)
                {
                    case "SimpleTriggerImpl":
                        var simpleTriggerImpl = (SimpleTriggerImpl)trigger;
                        output.Interval = simpleTriggerImpl.RepeatInterval;
                        break;
                    case "CronTriggerImpl":
                        //获取表达式
                        var cronTriggerImpl = (CronTriggerImpl)trigger;
                        output.Cron = cronTriggerImpl.CronExpressionString;
                        break;
                }
                output.ReplaceExists = true;
            }
            return View(output);
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("任务调度-视图-Cron表达式")]
        public ViewResultBase Cron()
        {
            return View();
        }

        /// <summary>
        ///     获取所有调度任务
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("任务调度-方法-列表-获取所有调度任务信息")]
        public JsonResult GetAllJobs()
        {
            IList<SystemQuartzJobModel> models = new List<SystemQuartzJobModel>();
            var jobGroups = StdSchedulerManager.GetJobGroupNames();
            foreach (string group in jobGroups)
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = StdSchedulerManager.GetJobKeys(groupMatcher);
                foreach (var jobKey in jobKeys)
                {
                    var detail = StdSchedulerManager.GetJobDetail(jobKey);
                    var triggers = StdSchedulerManager.GetTriggersOfJob(jobKey);

                    foreach (ITrigger trigger in triggers)
                    {
                        var model = new SystemQuartzJobModel
                        {
                            JobGroup = group,
                            JobName = jobKey.Name,
                            JobDescription = detail.Description,
                            TriggerState = "Complete",
                            ClassName = detail.JobType.FullName
                        };
                        model.TriggerName = trigger.Key.Name;
                        model.TriggerGroupName = trigger.Key.Group;
                        model.TriggerType = trigger.GetType().Name;
                        model.TriggerState = StdSchedulerManager.GetTriggerState(trigger.Key).ToString();
                        var nextFireTime = trigger.GetNextFireTimeUtc();
                        if (nextFireTime.HasValue)
                        {
                            model.NextFireTime = TimeZone.CurrentTimeZone.ToLocalTime(nextFireTime.Value.DateTime);
                        }

                        var previousFireTime = trigger.GetPreviousFireTimeUtc();
                        if (previousFireTime.HasValue)
                        {
                            model.PreviousFireTime =
                                TimeZone.CurrentTimeZone.ToLocalTime(previousFireTime.Value.DateTime);
                        }
                        models.Add(model);
                    }
                }
            }
            return Json(models.OrderBy(o => o.NextFireTime));
        }

        /// <summary>
        ///     通过名称分组删除作业
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("任务调度-方法-列表-通过名称分组删除作业")]
        public JsonResult DeleteJob(JobDetailInput input)
        {
            var status = new OperateStatus();
            if (StdSchedulerManager.DeleteJob(input.JobName, input.JobGroup))
            {
                status.ResultSign = ResultSign.Successful;
                status.Message = "删除作业成功";
            }
            return Json(status);
        }

        /// <summary>
        ///     通过作业名称及组名称获取作业参数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("任务调度-方法-新增/编辑-通过名称分组删除作业")]
        public JsonResult GetDetailJobDataMap(JobDetailInput input)
        {
            if (!input.JobGroup.IsNullOrEmpty() && !input.JobName.IsNullOrEmpty())
            {
                var detail = StdSchedulerManager.GetJobDetail(new JobKey(input.JobName, input.JobGroup));
                var maps = detail.JobDataMap;
                return Json(maps.Select(map => new Parameters { Key = map.Key, Value = map.Value }).ToList());
            }
            return null;
        }

        /// <summary>
        ///     通过名称分组暂停作业
        /// </summary>
        /// <param name="input"></param>
        [CreateBy("孙泽伟")]
        [Description("任务调度-方法-列表-通过名称分组暂停作业")]
        public JsonResult PauseJob(JobDetailInput input)
        {
            var status = new OperateStatus();
            try
            {
                StdSchedulerManager.PauseJob(input.JobName, input.JobGroup);
                status.ResultSign = ResultSign.Successful;
                status.Message = "暂停作业成功";
            }
            catch (Exception ex)
            {
                status.Message = ex.Message;
            }
            return Json(status);
        }

        /// <summary>
        ///     通过名称分组暂停所有作业
        /// </summary>
        [CreateBy("孙泽伟")]
        [Description("任务调度-方法-列表-暂停所有作业")]
        public JsonResult PauseAll()
        {
            var status = new OperateStatus();
            try
            {
                StdSchedulerManager.PauseAll();
                status.ResultSign = ResultSign.Successful;
                status.Message = "暂停所有作业成功";
            }
            catch (Exception ex)
            {
                status.Message = ex.Message;
            }
            return Json(status);
        }

        /// <summary>
        ///     通过名称分组恢复作业
        /// </summary>
        /// <param name="input"></param>
        [CreateBy("孙泽伟")]
        [Description("任务调度-方法-列表-通过名称分组启动作业")]
        public JsonResult ResumeJob(JobDetailInput input)
        {
            var status = new OperateStatus();
            try
            {
                StdSchedulerManager.ResumeJob(input.JobName, input.JobGroup);
                status.ResultSign = ResultSign.Successful;
                status.Message = "恢复作业成功";
            }
            catch (Exception ex)
            {
                status.Message = ex.Message;
            }
            return Json(status);
        }

        /// <summary>
        ///    启动所有作业
        /// </summary>
        /// <param name="input"></param>
        [CreateBy("孙泽伟")]
        [Description("任务调度-方法-列表-启动所有作业")]
        public JsonResult ResumeAll(JobDetailInput input)
        {
            var status = new OperateStatus();
            try
            {
                StdSchedulerManager.ResumeAll();
                status.ResultSign = ResultSign.Successful;
                status.Message = "恢复作业成功";
            }
            catch (Exception ex)
            {
                status.Message = ex.Message;
            }
            return Json(status);
        }

        /// <summary>
        ///     保存调度作业
        /// </summary>
        /// <param name="input">调度作业实体</param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("任务调度-方法-列表-保存调度作业")]
        public JsonResult ScheduleJob(ScheduleJobInput input)
        {
            var status = new OperateStatus();
            try
            {
                if (!input.ReplaceExists)
                {
                    //if (StdSchedulerManager.CheckExists(new TriggerKey(input.TriggerName, input.TriggerGroup)))
                    //{
                    //    status.Message = "指定的触发器已经存在，请重新指定名称";
                    //    return Json(status);
                    //}
                    if (StdSchedulerManager.CheckExists(new JobKey(input.JobName, input.JobGroup)))
                    {
                        status.Message = "指定的任务已经存在，请重新指定名称";
                        return Json(status);
                    }
                }
                input.IsSave = true;
                StdSchedulerManager.ScheduleJob(input);
                status.ResultSign = ResultSign.Successful;
                status.Message = "保存调度作业成功";
            }
            catch (Exception ex)
            {
                status.Message = ex.Message;
            }
            return Json(status);
        }

        /// <summary>
        ///     获得Corn表达式
        /// </summary>
        /// <param name="cronExpression"></param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("任务调度-方法-Cron-获得Corn表达式")]
        public JsonResult CalcRunTime(string cronExpression)
        {
            try
            {
                return Json(StdSchedulerManager.GetTaskeFireTime(cronExpression, 5));
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region 日历

        /// <summary>
        ///     日历
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("任务调度-视图-日历")]
        public ViewResultBase CalendarList()
        {
            return View();
        }

        /// <summary>
        ///     日历编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("任务调度-视图-日历编辑")]
        public ViewResultBase CalendarEdit(string calendarName)
        {
            var model = new SystemQuartzCalendarModel();
            if (!calendarName.IsNullOrEmpty())
            {
                model.ReplaceExists = true;
                var calendar = (CronCalendar)StdSchedulerManager.GetCalendar(calendarName);
                model.Expression = calendar.CronExpression.ToString();
                model.Description = calendar.Description;
            }
            return View(model);
        }

        /// <summary>
        ///     获取所有日历
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("任务调度-方法-日历-获得所有日历")]
        public JsonResult GetCalendar()
        {
            var calendars = StdSchedulerManager.GetCalendarNames().ToList().Select(n => new
            {
                Name = n,
                Calendar = StdSchedulerManager.GetCalendar(n)
            }).ToDictionary(n => n.Name, n => n.Calendar);
            IList<SystemQuartzCalendarModel> calendarModels = calendars.Select(cal => new SystemQuartzCalendarModel
            {
                Description = cal.Value.Description,
                CalendarName = cal.Key
            }).ToList();
            return Json(calendarModels);
        }

        /// <summary>
        ///     删除日历
        /// </summary>
        /// <param name="calendarName"></param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("任务调度-方法-日历-根据日历名称删除日历")]
        public JsonResult DeleteCalendar(string calendarName)
        {
            var status = new OperateStatus();
            try
            {
                if (StdSchedulerManager.DeleteCalendar(calendarName))
                {
                    status.ResultSign = ResultSign.Successful;
                    status.Message = "删除日历成功";
                }
            }
            catch (Exception ex)
            {
                status.Message = ex.Message;
            }
            return Json(status);
        }

        /// <summary>
        ///     保存日历
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("任务调度-方法-日历-保存日历")]
        public JsonResult SaveCalendar(CalendarInput input)
        {
            var status = new OperateStatus();
            try
            {
                if (!input.ReplaceExists && StdSchedulerManager.GetCalendar(input.CalendarName) != null)
                {
                    status.Message = "日历已存在，请换个其它名称或选择替换现有日历";
                    return Json(status);
                }
                StdSchedulerManager.AddCalendar(input);
                status.ResultSign = ResultSign.Successful;
                status.Message = "保存日历成功";
            }
            catch (Exception ex)
            {
                status.Message = ex.Message;
            }
            return Json(status);
        }
        #endregion
    }
}