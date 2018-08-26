using Quartz;

namespace EIP.Common.Core.Quartz
{
    /// <summary>
    /// 自定义触发器监听器
    /// </summary>
    public class QuartzTriggerListener:ITriggerListener
    {
        /// <summary>
        /// 触发器名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 触发器完成时触发
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        /// <param name="triggerInstructionCode"></param>
        public void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 触发器调用时触发
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        public void TriggerFired(ITrigger trigger, IJobExecutionContext context)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 错过触发时调用(例：线程不够用的情况下
        /// </summary>
        /// <param name="trigger"></param>
        public void TriggerMisfired(ITrigger trigger)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Trigger触发后，job执行时调用本方法。true即否决，job后面不执行。
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool VetoJobExecution(ITrigger trigger, IJobExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}