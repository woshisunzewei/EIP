using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.Entities;
using EIP.Common.Entities.Paging;
using EIP.Workflow.Models.Dtos.Engine;

namespace EIP.Workflow.Business.Engine
{
    /// <summary>
    ///     工作流引擎接口
    /// </summary>
    public interface IWorkflowEngineLogic
    {
        #region 归档

        /// <summary>
        ///     保存归档信息
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> SaveWorkflowEngineArchives();

        #endregion

        #region 任务相关

        /// <summary>
        ///     获取第一步开始流程信息
        /// </summary>
        /// <returns></returns>
        Task<WorkflowEngineDealWithTaskOutput> GetWorkflowEngineStartTaskOutput(WorkflowEngineStartTaskInput input);

        /// <summary>
        ///     获取下一步所有连线
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowEngineNextLinesDoubleWay>> GetWorkflowEngineNextLinesOutput(
            WorkflowEngineRunnerInput input);

        /// <summary>
        ///     获取下一步所有活动
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowEngineNextActivitysDoubleWay>> GetWorkflowEngineNextActivitysDoubleWay(
            WorkflowEngineRunnerInput input);

        /// <summary>
        ///     获取需要处理任务信息
        /// </summary>
        /// <returns></returns>
        Task<WorkflowEngineDealWithTaskOutput> GetWorkflowEngineDealWithTaskOutput(WorkflowEngineRunnerInput input);

        /// <summary>
        ///     需要处理的任务(待处理事项)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<WorkflowEngineNeedDoTaskOutput>> GetWorkflowEngineNeedDoTaskOutput(WorkflowEngineRunnerInput input);

        /// <summary>
        ///     已处理的任务(已处理事项)
        /// </summary>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineHaveDoTaskOutput>> GetWorkflowEngineHaveDoTaskOutput(WorkflowEngineRunnerInput input);
        
        /// <summary>
        ///     保存任务到下一步
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> SaveWorkflowEngineTaskNext(WorkflowEngineRunnerInput input);

        /// <summary>
        ///     保存任务上一步
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> SaveWorkflowEngineTaskSeedBack();

        /// <summary>
        ///     跳转到某一步任务
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> SaveWorkflowEngineTaskJump();

        /// <summary>
        ///     撤回一步任务
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> SaveWorkflowEngineTaskWithdraw();

        /// <summary>
        ///     设置某一步为已读
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> SaveWorkflowEngineSetTaskRead();

        /// <summary>
        ///     设置委托任务
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> SaveWorkflowEngineEntrust();

        /// <summary>
        ///     是否为最后一步任务
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> IsLastTask();

        #endregion

        #region 流程相关

        /// <summary>
        ///     获取流程监控信息:查看当前流程已走过步骤信息
        /// </summary>
        /// <returns></returns>
        Task<WorkflowEngineMonitoringProcessOutput> GetWorkflowEngineMonitoringProcessOutput();

        /// <summary>
        ///     开始流程信息
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> StartWorkflowEngineProcess(WorkflowEngineStartTaskInput input);

        /// <summary>
        ///     保存该流程:填写好信息后并未发送该流程
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> SaveWorkflowEngineProcess();

        /// <summary>
        ///     删除流程信息
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> DeleteWorkflowEngineProcess();

        /// <summary>
        ///     更新流程信息
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> UpdateWorkflowEngineProcess();

        /// <summary>
        ///     取消流程
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> SaveWorkflowEngineProcessCancel();

        /// <summary>
        ///     所有发起过的流程(我的请求)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<WorkflowEngineHaveSendProcessOutput>> GetWorkflowEngineHaveSendOutput(WorkflowEngineRunnerInput input);

        #endregion

        #region 监控

        /// <summary>
        /// 获取执行轨迹:流程图
        /// </summary>
        /// <returns></returns>
        Task<WorkflowEngineTrackForMapOutput> GetWorkflowEngineTrackForMap(WorkflowEngineRunnerInput input);

        /// <summary>
        /// 获取执行轨迹:表格
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<WorkflowEngineTrackForTableOutput>> GetWorkflowEngineTrackForTable(WorkflowEngineRunnerInput input);
        #endregion

    }
}