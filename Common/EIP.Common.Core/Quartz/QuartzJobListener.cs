using Quartz;

namespace EIP.Common.Core.Quartz
{
    /// <summary>
    /// 自定义Job监听器
    /// </summary>
    public class QuartzJobListener:IJobListener
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void JobExecutionVetoed(IJobExecutionContext context)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void JobToBeExecuted(IJobExecutionContext context)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jobException"></param>
        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            throw new System.NotImplementedException();
        }

        public string Name { get; set; }
    }
}