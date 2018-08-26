using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EIP.Workflow.Models.Entities;
using EIP.Common.Core.Extensions;
using EIP.Common.Dapper;
using EIP.Common.DataAccess;
using EIP.Common.Entities.Dtos;

namespace EIP.Workflow.DataAccess.Config
{
    /// <summary>
    ///     工作流实例接口实现
    /// </summary>
    public class WorkflowProcessRepository : DapperAsyncRepository<WorkflowProcess>, IWorkflowProcessRepository
    {
        /// <summary>
        /// 根据流程类型获取所有流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<WorkflowProcess>> GetWorkflowByProcessType(NullableIdInput input)
        {
            StringBuilder sql = new StringBuilder("SELECT * FROM Workflow_Process");
            if (!input.Id.IsNullOrEmptyGuid())
            {
                sql.Append(" WHERE ProcessType=@processType");
            }
            return SqlMapperUtil.SqlWithParams<WorkflowProcess>(sql.ToString(), new { processType = input.Id });
        }

        /// <summary>
        /// 删除活动及连线
        /// </summary>
        /// <param name="input"></param>
        public Task<int> DeleteWorkflowActivityAndLine(IdInput input)
        {
            const string sql = " DELETE Workflow_ProcessActivity WHERE ProcessId=@id" +
                               " DELETE Workflow_ProcessLine WHERE ProcessId=@id";
            return SqlMapperUtil.InsertUpdateOrDeleteSql<WorkflowProcess>(sql, new { id = input.Id });
        }
    }
}