using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using EIP.Common.Core.Extensions;
using EIP.Common.Core.Quartz.Dtos;
using EIP.Common.Core.Quartz.Enums;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Calendar;
using Quartz.Impl.Matchers;
using Quartz.Spi;

namespace EIP.Common.Core.Quartz
{
    /// <summary>
    /// 调度框架管理
    /// </summary>
    public class StdSchedulerManager
    {
        #region 初始化
        /// <summary>
        /// 用来保存调度框架属性集合对象
        /// </summary>
        private static readonly NameValueCollection Properties = new NameValueCollection();

        /// <summary>
        /// 初始化调度框架数据库属性
        /// </summary>
        static StdSchedulerManager()
        {
            //存储类型
            Properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX,Quartz";
            //表明前缀
            Properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
            //驱动类型
            Properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz";
            //线程数目
            Properties["quartz.threadPool.threadCount"] = ConfigurationManager.AppSettings["threadCount"];
            //数据源名称
            var dbName = ConfigurationManager.AppSettings["dataSource"];
            //写入数据名称
            Properties["quartz.jobStore.dataSource"] = dbName;
            //拼接连接属性名称
            var conPropertieName = string.Format("quartz.dataSource.{0}.connectionString", dbName);
            //连接字符串
            Properties[conPropertieName] = ConfigurationManager.AppSettings["EIP_Quartz"];
            //拼接驱动属性名称
            var proPropertieName = string.Format("quartz.dataSource.{0}.provider", dbName);
            //sqlserver版本
            Properties[proPropertieName] = "SqlServer-20";

            Properties["quartz.jobStore.clustered"] = "true";

            Properties["quartz.scheduler.instanceId"] = "AUTO";

            SchedulerFactory = new StdSchedulerFactory(Properties);
            Scheduler = SchedulerFactory.GetScheduler();
        }

        /// <summary>
        /// 调度工厂
        /// </summary>
        private static StdSchedulerFactory SchedulerFactory { get; set; }

        /// <summary>
        /// 调度接口
        /// </summary>
        private static IScheduler Scheduler { get; set; }

        public IListenerManager ListenerManager
        {
            get
            {
                return Scheduler.ListenerManager;
            }
        }

        /// <summary>
        /// 调度器名称
        /// </summary>
        public string SchedulerName
        {
            get
            {
                return Scheduler.SchedulerName;
            }
        }

        public bool InStandbyMode
        {
            get
            {
                return Scheduler.InStandbyMode;
            }
        }

        /// <summary>
        /// 初始化配置参数
        /// </summary>
        /// <param name="props"></param>
        public static void Initialize(NameValueCollection props)
        {
            SchedulerFactory.Initialize(props);
        }

        /// <summary>
        /// 调用开启方法
        /// </summary>
        public static void Start()
        {
            try
            {
                Scheduler.Start();
            }
            catch (Exception)
            {
                throw new Exception("确定配置的参数是否有错误");
            }
        }

        #endregion

        #region 添加

        /// <summary>
        /// 添加Job
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static void ScheduleJob(ScheduleJobInput input)
        {
            #region JobDetail
            JobBuilder jobBuilder = JobBuilder
                .Create()
                .OfType(Type.GetType(input.FullName + "," + input.AssemblyName, true))
                .WithDescription(input.JobDescription)
                .WithIdentity(new JobKey(input.JobName, input.JobGroup))
                .UsingJobData(GetJobDataMap(input));

            if (input.IsRequest)
            {
                //在服务器异常时候,重启调度之后,接着执行调度
                jobBuilder = jobBuilder.RequestRecovery();
            }
            if (input.IsSave)
            {
                //保存到数据库中
                jobBuilder.StoreDurably();
            }
            IJobDetail detail = jobBuilder.Build();
            #endregion

            #region trigger

            var triggerBuilder = TriggerBuilder
                .Create()
                .ForJob(detail);

            if (!input.ChoicedCalendar.IsNullOrEmpty())
                triggerBuilder.ModifiedByCalendar(input.ChoicedCalendar);
            if (!input.TriggerName.IsNullOrEmpty() && !input.TriggerGroup.IsNullOrEmpty())
            {
                triggerBuilder.WithDescription(input.TriggerDescription)
                   .WithIdentity(new TriggerKey(input.TriggerName, input.TriggerGroup));
            }
            #endregion

            //是否替换
            if (input.ReplaceExists)
            {
                var triggers = new global::Quartz.Collection.HashSet<ITrigger>();
                //如果是Cron触发器
                if (input.TriggerType == "CronTriggerImpl")
                {
                    triggers.Add(triggerBuilder.WithCronSchedule(input.Cron).Build());
                }
                else
                {
                    var simpleBuilder = SimpleScheduleBuilder.Create();
                    if (input.Repeat)
                    {
                        simpleBuilder.RepeatForever();
                    }
                    simpleBuilder.WithInterval(input.Interval);
                    triggers.Add(triggerBuilder.WithSchedule(simpleBuilder).Build());
                }
                ScheduleJob(detail, triggers, true);
            }
            else
            {
                //如果是Cron触发器
                if (input.TriggerType == "CronTriggerImpl")
                {
                    ScheduleJob(detail, triggerBuilder.WithCronSchedule(input.Cron).Build());
                }
                else
                {
                    var simpleBuilder = SimpleScheduleBuilder.Create();
                    if (input.Repeat)
                    {
                        simpleBuilder.RepeatForever();
                    }
                    simpleBuilder.WithInterval(input.Interval);
                    ScheduleJob(detail, triggerBuilder.WithSchedule(simpleBuilder).Build());
                }
            }
        }

        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static JobDataMap GetJobDataMap(ScheduleJobInput input)
        {
            JobDataMap map = new JobDataMap();
            foreach (var param in input.Parameters)
            {
                map.Add(param.Key, param.Value);
            }

            return map;
        }

        /// <summary>
        /// 触发Job
        /// </summary>
        /// <param name="jobDetail">jobDetail</param>
        /// <param name="trigger">trigger触发器</param>
        /// <returns></returns>
        public static DateTimeOffset ScheduleJob(IJobDetail jobDetail,
            ITrigger trigger)
        {
            return Scheduler.ScheduleJob(jobDetail, trigger);
        }

        /// <summary>
        /// 添加作业
        /// </summary>
        /// <param name="jobDetail"></param>
        /// <param name="triggers"></param>
        /// <param name="replace"></param>
        public static void ScheduleJob(IJobDetail jobDetail,
           global::Quartz.Collection.ISet<ITrigger> triggers,
            bool replace = false)
        {
            Scheduler.ScheduleJob(jobDetail, triggers, replace);
        }

        /// <summary>
        /// 把作业与触发器添加到调度里去
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="groupName"></param>
        public static void TriggerJob(string jobName, string groupName)
        {
            Scheduler.TriggerJob(new JobKey(jobName, groupName));
        }
        #endregion

        #region 开启
        /// <summary>
        /// 开启所有
        /// </summary>
        public static void ResumeAll()
        {
            Scheduler.ResumeAll();
        }
        /// <summary>
        /// 根据组名开启作业
        /// </summary>
        /// <param name="groupName">组名</param>
        public static void ResumeJobGroup(string groupName)
        {
            Scheduler.ResumeJobs(GroupMatcher<JobKey>.GroupEquals(groupName));
        }
        /// <summary>
        /// 根据触发器组名开启作业
        /// </summary>
        /// <param name="groupName">组名</param>
        public static void ResumeTriggerGroup(string groupName)
        {
            Scheduler.ResumeTriggers(GroupMatcher<TriggerKey>.GroupEquals(groupName));
        }

        /// <summary>
        /// 根据名称开启作业
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="groupName"></param>
        public static void ResumeJob(string jobName, string groupName)
        {
            Scheduler.ResumeJob(new JobKey(jobName, groupName));
        }

        /// <summary>
        /// 恢复触发器
        /// </summary>
        /// <param name="triggerName"></param>
        /// <param name="groupName"></param>
        public static void ResumeTrigger(string triggerName, string groupName)
        {
            Scheduler.ResumeTrigger(new TriggerKey(triggerName, groupName));
        }
        #endregion

        #region 暂停
        /// <summary>
        /// 暂停所有
        /// </summary>
        public static void PauseAll()
        {
            Scheduler.PauseAll();
        }

        /// <summary>
        /// 根据组名暂停作业
        /// </summary>
        /// <param name="groupName">组名</param>
        public static void PauseJobGroup(string groupName)
        {
            Scheduler.PauseJobs(GroupMatcher<JobKey>.GroupEquals(groupName));
        }

        /// <summary>
        /// 根据组名暂停触发器
        /// </summary>
        /// <param name="groupName">组名</param>
        public static void PauseTriggerGroup(string groupName)
        {
            Scheduler.PauseTriggers(GroupMatcher<TriggerKey>.GroupEquals(groupName));
        }
        /// <summary>
        /// 通过名称分组暂停作业
        /// </summary>
        /// <param name="jobName">作业名</param>
        /// <param name="groupName">分组名</param>
        public static void PauseJob(string jobName, string groupName)
        {
            Scheduler.PauseJob(new JobKey(jobName, groupName));
        }

        /// <summary>
        /// 暂停触发器
        /// </summary>
        /// <param name="triggerName"></param>
        /// <param name="groupName"></param>
        public static void PauseTrigger(string triggerName, string groupName)
        {
            Scheduler.PauseTrigger(new TriggerKey(triggerName, groupName));
        }

        /// <summary>
        /// 判断某组作业是否被暂停
        /// </summary>
        /// <param name="jobGroupName">组名</param>
        /// <returns></returns>
        public static bool? IsJobGroupPaused(string jobGroupName)
        {
            try
            {
                return Scheduler.IsJobGroupPaused(jobGroupName);
            }
            catch (NotImplementedException)
            {
                return null;
            }
        }

        /// <summary>
        /// 判断某组触发器是否暂停
        /// </summary>
        /// <param name="triggerGroupName"></param>
        /// <returns></returns>
        public static bool? IsTriggerGroupPaused(string triggerGroupName)
        {
            try
            {
                return Scheduler.IsTriggerGroupPaused(triggerGroupName);
            }
            catch (NotImplementedException)
            {
                return null;
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 移除全局作业监听
        /// </summary>
        /// <param name="name">监听名称</param>
        public static void RemoveGlobalJobListener(string name)
        {
            Scheduler.ListenerManager.RemoveJobListener(name);
        }

        /// <summary>
        /// 移除全局触发器监听
        /// </summary>
        /// <param name="name">监听名称</param>
        public static void RemoveGlobalTriggerListener(string name)
        {
            Scheduler.ListenerManager.RemoveTriggerListener(name);
        }

        /// <summary>
        /// 通过名称分组删除作业
        /// </summary>
        /// <param name="jobName">作业名</param>
        /// <param name="groupName">分组名</param>
        /// <returns></returns>
        public static bool DeleteJob(string jobName, string groupName)
        {
            return Scheduler.DeleteJob(new JobKey(jobName, groupName));
        }
        #endregion

        #region 停止
        /// <summary>
        /// 停止调度
        /// </summary>
        public static void Shutdown()
        {
            Scheduler.Shutdown();
        }

        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="triggerName"></param>
        /// <param name="groupName"></param>
        public static void UnscheduleJob(string triggerName, string groupName)
        {
            Scheduler.UnscheduleJob(new TriggerKey(triggerName, groupName));
        }
        #endregion

        #region 日历
        /// <summary>
        /// 获取所有日历名称
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetCalendarNames()
        {
            return Scheduler.GetCalendarNames();
        }

        /// <summary>
        /// 根据名称获取日历
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ICalendar GetCalendar(string name)
        {
            return Scheduler.GetCalendar(name);
        }

        /// <summary>
        /// 根据日历关键字删除日历
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DeleteCalendar(string key)
        {
            return Scheduler.DeleteCalendar(key);
        }

        /// <summary>
        /// 添加日历
        /// </summary>
        /// <param name="input"></param>
        public static void AddCalendar(CalendarInput input)
        {
            var calendar = new BaseCalendar();
            switch (input.EnumCalendar)
            {
                case EnumCalendar.Cron表达式:
                    calendar = new CronCalendar(input.Expression);
                    break;
            }
            calendar.TimeZone = TimeZoneInfo.Local;
            calendar.Description = input.Description;
            Scheduler.AddCalendar(input.CalendarName, calendar, input.ReplaceExists, input.UpdateTriggers);
        }
        #endregion

        #region 备用
        /// <summary>
        /// 备用
        /// </summary>
        public static void Standby()
        {
            Scheduler.Standby();
        }
        #endregion

        #region 中断
        /// <summary>
        /// 中断
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="groupName"></param>
        public static void Interrupt(string jobName, string groupName)
        {
            Scheduler.Interrupt(new JobKey(jobName, groupName));
        }
        #endregion

        #region 方法
        /// <summary>
        /// 返回运行作业集合
        /// </summary>
        /// <returns></returns>
        public static IEnumerable GetCurrentlyExecutingJobs()
        {
            return Scheduler.GetCurrentlyExecutingJobs();
        }

        /// <summary>
        /// 获取所有作业键集合
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static global::Quartz.Collection.ISet<JobKey> GetJobKeys(GroupMatcher<JobKey> key)
        {
            return Scheduler.GetJobKeys(key);
        }

        /// <summary>
        /// 获取某个作业
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IJobDetail GetJobDetail(JobKey key)
        {
            return Scheduler.GetJobDetail(key);
        }

        /// <summary>
        /// 获取调度器数据
        /// </summary>
        /// <returns></returns>
        public static SchedulerMetaData GetMetaData()
        {
            return Scheduler.GetMetaData();
        }

        public static IEnumerable GetTriggersOfJob(JobKey jobKey)
        {
            try
            {
                return Scheduler.GetTriggersOfJob(jobKey);
            }
            catch (NotImplementedException)
            {
                return null;
            }
        }

        public static global::Quartz.Collection.ISet<TriggerKey> GetTriggerKeys(GroupMatcher<TriggerKey> matcher)
        {
            try
            {
                return Scheduler.GetTriggerKeys(matcher);
            }
            catch (NotImplementedException)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取触发器组名
        /// </summary>
        /// <returns></returns>
        public static IEnumerable GetTriggerGroupNames()
        {
            return Scheduler.GetTriggerGroupNames();
        }

        /// <summary>
        /// 获取工作名
        /// </summary>
        /// <returns></returns>
        public static IEnumerable GetJobGroupNames()
        {
            return Scheduler.GetJobGroupNames();
        }

        /// <summary>
        /// 获取Triiger信息
        /// </summary>
        /// <param name="triggerKey"></param>
        /// <returns></returns>
        public static ITrigger GetTrigger(TriggerKey triggerKey)
        {
            return Scheduler.GetTrigger(triggerKey);
        }

        /// <summary>
        /// 检查触发器是否存在
        /// </summary>
        /// <returns></returns>
        public static bool CheckExists(TriggerKey triggerKey)
        {
            return Scheduler.CheckExists(triggerKey);
        }

        /// <summary>
        /// 检查任务是否存在
        /// </summary>
        /// <returns></returns>
        public static bool CheckExists(JobKey jobKey)
        {
            return Scheduler.CheckExists(jobKey);
        }

        /// <summary>
        /// 获取触发器状态
        /// </summary>
        /// <param name="triggerKey"></param>
        /// <returns></returns>
        public static TriggerState GetTriggerState(TriggerKey triggerKey)
        {
            return Scheduler.GetTriggerState(triggerKey);
        }

        /// <summary>
        ///     获取任务在未来周期内哪些时间会运行
        /// </summary>
        /// <param name="cronExpressionString">Cron表达式</param>
        /// <param name="numTimes">运行次数</param>
        /// <returns>运行时间段</returns>
        public static List<string> GetTaskeFireTime(string cronExpressionString, int numTimes)
        {
            if (numTimes < 0)
            {
                throw new Exception("参数numTimes值大于等于0");
            }
            //时间表达式
            var trigger = TriggerBuilder.Create().WithCronSchedule(cronExpressionString).Build();
            var dates = TriggerUtils.ComputeFireTimes(trigger as IOperableTrigger, null, numTimes);
            return dates.Select(dtf => TimeZoneInfo.ConvertTimeFromUtc(dtf.DateTime, TimeZoneInfo.Local).ToString(CultureInfo.InvariantCulture)).ToList();
        }
        #endregion

    }
}