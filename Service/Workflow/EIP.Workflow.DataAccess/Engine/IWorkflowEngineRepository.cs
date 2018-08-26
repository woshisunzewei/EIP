using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Workflow.Models.Dtos.Engine;

namespace EIP.Workflow.DataAccess.Engine
{
    public interface IWorkflowEngineRepository
    {
        /// <summary>
        ///     获取第一步开始流程信息
        /// </summary>
        /// <returns></returns>
        Task<WorkflowEngineDealWithTaskOutput> GetWorkflowEngineStartTaskOutput(WorkflowEngineStartTaskInput input);

        /// <summary>
        ///     需要处理的任务(待处理事项)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<WorkflowEngineNeedDoTaskOutput>> GetWorkflowEngineNeedDoTaskOutput(WorkflowEngineRunnerInput input);

        /// <summary>
        ///     所有发起过的流程(我的请求)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<WorkflowEngineHaveSendProcessOutput>> GetWorkflowEngineHaveSendOutput(WorkflowEngineRunnerInput input);
        
    }
}