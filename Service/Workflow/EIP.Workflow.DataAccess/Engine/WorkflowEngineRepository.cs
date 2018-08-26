using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EIP.Common.Core.Extensions;
using EIP.Common.Dapper;
using EIP.Workflow.Models.Dtos.Engine;
using EIP.Workflow.Models.Enums;

namespace EIP.Workflow.DataAccess.Engine
{
    public class WorkflowEngineRepository : IWorkflowEngineRepository
    {
        /// <summary>
        ///     获取第一步开始流程信息
        ///     1、活动Id
        ///     2、对应表单地址
        ///     3、能够操作的按钮
        /// </summary>
        /// <returns></returns>
        public Task<WorkflowEngineDealWithTaskOutput> GetWorkflowEngineStartTaskOutput(
            WorkflowEngineStartTaskInput input)
        {
            const string sql =
                @"select ActivityId,form.Url FormUrl,activity.Buttons,activity.Name,process.Name ProcessName from Workflow_ProcessActivity activity
                                 left join Workflow_Form form on activity.FormId=form.FormId
                                 left join Workflow_Process process on activity.ProcessId=process.ProcessId
                                 where activity.ProcessId=@processId and activity.[Type]=@type";
            return SqlMapperUtil.SqlWithParamsSingle<WorkflowEngineDealWithTaskOutput>(sql, new
            {
                processId = input.ProcessId,
                input.Type
            });
        }

        /// <summary>
        ///     需要处理的任务(待处理事项)
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<WorkflowEngineNeedDoTaskOutput>> GetWorkflowEngineNeedDoTaskOutput(
            WorkflowEngineRunnerInput input)
        {
            var sql = new StringBuilder(
                @"select TaskId,task.Title,task.ProcessInstanceId,CurrentActivityId,CurrentActivityName,SendUserName,SendTime,process.Name ProcessName,process.ProcessId,instance.Urgency from Workflow_ProcessInstance_Task task
                left join Workflow_ProcessInstance instance on task.ProcessInstanceId=instance.ProcessInstanceId
                left join Workflow_Process process on instance.ProcessId=process.ProcessId
                where ReceiveUserId=@receiveUserId and task.[Status]=@status");
            if (!input.ProcessId.IsEmptyGuid())
            {
                sql.Append("  and instance.ProcessId=@processId");
            }
            return SqlMapperUtil.SqlWithParams<WorkflowEngineNeedDoTaskOutput>(sql.ToString(), new
            {
                receiveUserId = input.CurrentUser.UserId,
                status = (byte) EnumTask.正在处理,
                processId = input.ProcessId
            });
        }

        /// <summary>
        ///     所有发起过的流程(我的请求)
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<WorkflowEngineHaveSendProcessOutput>> GetWorkflowEngineHaveSendOutput(
            WorkflowEngineRunnerInput input)
        {
            var sql = new StringBuilder(
                @"select ProcessInstanceId,instance.ProcessId,instance.Title,process.Name,instance.Status,Urgency,instance.CreateTime,EndTime,EndUserName,EndUserOrganization from [Workflow_ProcessInstance] instance
                left join [Workflow_Process] process on instance.ProcessId=process.ProcessId
                where instance.CreateUserId=@userId");
            return SqlMapperUtil.SqlWithParams<WorkflowEngineHaveSendProcessOutput>(sql.ToString(), new
            {
                userId = input.CurrentUser.UserId
            });
        }
    }
}