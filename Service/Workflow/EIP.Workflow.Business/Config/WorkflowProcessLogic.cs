using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Core.Extensions;
using EIP.Common.Core.Utils;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.Workflow.DataAccess.Config;
using EIP.Workflow.Models.Dtos;
using EIP.Workflow.Models.Entities;

namespace EIP.Workflow.Business.Config
{
    /// <summary>
    ///     工作流处理界面按钮接口实现
    /// </summary>
    public class WorkflowProcessLogic : AsyncLogic<WorkflowProcess>, IWorkflowProcessLogic
    {
        #region 构造函数

        private readonly IWorkflowProcessRepository _processRepository;
        private readonly IWorkflowProcessActivityRepository _activityRepository;
        private readonly IWorkflowProcessLineRepository _lineRepository;
        private readonly IWorkflowProcessAreasLogic _areasLogic;
        public WorkflowProcessLogic(IWorkflowProcessRepository processRepository,
            IWorkflowProcessActivityRepository activityRepository,
            IWorkflowProcessLineRepository lineRepository,
            IWorkflowProcessAreasLogic areasLogic)
            : base(processRepository)
        {
            _processRepository = processRepository;
            _activityRepository = activityRepository;
            _lineRepository = lineRepository;
            _areasLogic = areasLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        ///     根据流程类型获取所有流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WorkflowProcess>> GetWorkflowByProcessType(NullableIdInput input)
        {
            return await _processRepository.GetWorkflowByProcessType(input);
        }

        /// <summary>
        ///     保存
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(WorkflowProcess process)
        {
            if (process.ProcessId.IsEmptyGuid())
            {
                process.CreateTime = DateTime.Now;
                process.CreateUserId = process.UpdateUserId;
                process.CreateUserName = process.UpdateUserName;
                process.DesignJson = process.DesignJson.Replace("{0}", process.Name);
                process.DesignJson = process.DesignJson.Replace("{1}", CombUtil.NewComb().ToString());
                process.DesignJson = process.DesignJson.Replace("{2}", CombUtil.NewComb().ToString());
                process.ProcessId = CombUtil.NewComb();
                return await InsertAsync(process);
            }
            var workflowProcess = await GetByIdAsync(process.ProcessId);
            process.CreateTime = workflowProcess.CreateTime;
            process.CreateUserId = workflowProcess.CreateUserId;
            process.CreateUserName = workflowProcess.CreateUserName;
            process.DesignJson = workflowProcess.DesignJson;
            return await UpdateAsync(process);
        }

        /// <summary>
        ///     保存流程设计图
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveWorkflowProcessJson(WorkflowSaveProcessInput process)
        {
            var operateStatus = new OperateStatus();
            var workflowProcess = await GetByIdAsync(process.ProcessId);
            try
            {
                workflowProcess.DesignJson = process.DesignJson;
                workflowProcess.UpdateUserId = process.UpdateUserId;
                workflowProcess.UpdateUserName = process.UpdateUserName;
                workflowProcess.UpdateTime = process.UpdateTime;
                await _processRepository.DeleteWorkflowActivityAndLine(new IdInput { Id = process.ProcessId });
                process.Activities = process.Activity.JsonStringToList<WorkflowProcessActivity>();
                process.Lines = process.Line.JsonStringToList<WorkflowProcessLine>();
                process.Areases = process.Areas.JsonStringToList<WorkflowProcessAreas>();
                foreach (var ac in process.Activities)
                {
                    ac.Buttons = JsonExtension.ListToJsonString(ac.ButtonList);
                    ac.ProcessId = process.ProcessId;
                }
                foreach (var ac in process.Lines)
                {
                    ac.ProcessId = process.ProcessId;
                }
                foreach (var ac in process.Areases)
                {
                    ac.ProcessId = process.ProcessId;
                }
                await _activityRepository.InsertMultipleDapperAsync(process.Activities);
                await _lineRepository.InsertMultipleDapperAsync(process.Lines);
                await _areasLogic.InsertMultipleDapperAsync(process.Areases);
                operateStatus = await UpdateAsync(workflowProcess);
            }
            catch (Exception exception)
            {
                operateStatus.Message = exception.Message;
            }
            return operateStatus;
        }

        #endregion
    }
}