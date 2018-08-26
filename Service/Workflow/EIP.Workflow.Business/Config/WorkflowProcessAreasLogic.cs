using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Workflow.DataAccess.Config;
using EIP.Workflow.Models.Dtos.Engine;
using EIP.Workflow.Models.Entities;

namespace EIP.Workflow.Business.Config
{
    /// <summary>
    ///     区块
    /// </summary>
    public class WorkflowProcessAreasLogic : AsyncLogic<WorkflowProcessAreas>, IWorkflowProcessAreasLogic
    {
        /// <summary>
        /// 获取区域监控数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<WorkflowProcessAreas>> GetWorkflowEngineTrackAreasOutput(WorkflowEngineRunnerInput input)
        {
            return _workflowProcessAreasRepository.GetWorkflowEngineTrackAreasOutput(input);
        }

        #region 构造函数

        private readonly IWorkflowProcessAreasRepository _workflowProcessAreasRepository;

        public WorkflowProcessAreasLogic(IWorkflowProcessAreasRepository workflowProcessAreasRepository)
            : base(workflowProcessAreasRepository)
        {
            _workflowProcessAreasRepository = workflowProcessAreasRepository;
        }

        #endregion
    }
}