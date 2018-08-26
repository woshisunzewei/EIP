using Quartz;

namespace EIP.Common.Core.Quartz
{
    /// <summary>
    /// 调度器监听器
    /// </summary>
    public class QuartzSchedulerListener : ISchedulerListener
    {

        public void JobAdded(IJobDetail jobDetail)
        {
            throw new System.NotImplementedException();
        }

        public void JobDeleted(JobKey jobKey)
        {
            throw new System.NotImplementedException();
        }

        public void JobPaused(JobKey jobKey)
        {
            throw new System.NotImplementedException();
        }

        public void JobResumed(JobKey jobKey)
        {
            throw new System.NotImplementedException();
        }

        public void JobScheduled(ITrigger trigger)
        {
            throw new System.NotImplementedException();
        }

        public void JobUnscheduled(TriggerKey triggerKey)
        {
            throw new System.NotImplementedException();
        }

        public void JobsPaused(string jobGroup)
        {
            throw new System.NotImplementedException();
        }

        public void JobsResumed(string jobGroup)
        {
            throw new System.NotImplementedException();
        }

        public void SchedulerError(string msg, SchedulerException cause)
        {
            throw new System.NotImplementedException();
        }

        public void SchedulerInStandbyMode()
        {
            throw new System.NotImplementedException();
        }

        public void SchedulerShutdown()
        {
            throw new System.NotImplementedException();
        }

        public void SchedulerShuttingdown()
        {
            throw new System.NotImplementedException();
        }

        public void SchedulerStarted()
        {
            throw new System.NotImplementedException();
        }

        public void SchedulerStarting()
        {
            throw new System.NotImplementedException();
        }

        public void SchedulingDataCleared()
        {
            throw new System.NotImplementedException();
        }

        public void TriggerFinalized(ITrigger trigger)
        {
            throw new System.NotImplementedException();
        }

        public void TriggerPaused(TriggerKey triggerKey)
        {
            throw new System.NotImplementedException();
        }

        public void TriggerResumed(TriggerKey triggerKey)
        {
            throw new System.NotImplementedException();
        }

        public void TriggersPaused(string triggerGroup)
        {
            throw new System.NotImplementedException();
        }

        public void TriggersResumed(string triggerGroup)
        {
            throw new System.NotImplementedException();
        }
    }
}