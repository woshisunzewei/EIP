using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIP.Common.Core.Extensions;
using EIP.Common.Core.Log;
using EIP.Common.Core.Utils;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Paging;
using EIP.Workflow.Business.Config;
using EIP.Workflow.Business.Engine.Core.ReceiveUser;
using EIP.Workflow.DataAccess.Engine;
using EIP.Workflow.Models.Dtos.Engine;
using EIP.Workflow.Models.Entities;
using EIP.Workflow.Models.Enums;

namespace EIP.Workflow.Business.Engine
{
    /// <summary>
    ///     工作流引擎实现
    /// </summary>
    public class WorkflowEngineLogic : IWorkflowEngineLogic
    {
        #region 归档

        /// <summary>
        ///     保存归档信息
        /// </summary>
        /// <returns></returns>
        public Task<OperateStatus> SaveWorkflowEngineArchives()
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        ///     获取第一步开始流程信息
        /// </summary>
        /// <returns></returns>
        public Task<WorkflowEngineDealWithTaskOutput> GetWorkflowEngineStartTaskOutput(
            WorkflowEngineStartTaskInput input)
        {
            return _workflowEngineRepository.GetWorkflowEngineStartTaskOutput(input);
        }

        /// <summary>
        ///     获取下一步所有连线
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<WorkflowEngineNextLinesDoubleWay>> GetWorkflowEngineNextLinesOutput(
            WorkflowEngineRunnerInput input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     获取下一步所有活动
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WorkflowEngineNextActivitysDoubleWay>> GetWorkflowEngineNextActivitysDoubleWay(
            WorkflowEngineRunnerInput input)
        {
            return await _workflowProcessInstanceActivityLogic.GetWorkflowEngineNextActivitysDoubleWay(input);
        }

        /// <summary>
        ///     需要处理的任务(待处理事项)
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<WorkflowEngineNeedDoTaskOutput>> GetWorkflowEngineNeedDoTaskOutput(
            WorkflowEngineRunnerInput input)
        {
            return await _workflowEngineRepository.GetWorkflowEngineNeedDoTaskOutput(input);
        }

        /// <summary>
        /// 已完成任务信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<WorkflowEngineHaveDoTaskOutput>> GetWorkflowEngineHaveDoTaskOutput(WorkflowEngineRunnerInput input)
        {
            return _workflowProcessTaskLogic.GetWorkflowEngineHaveDoTaskOutput(input);
        }

        /// <summary>
        ///     保存流程到下一步
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveWorkflowEngineTaskNext(WorkflowEngineRunnerInput input)
        {
            //根据当前模块Id及实例Id获取下一步流程信息
            var nextActivitys = await GetWorkflowEngineNextActivitysDoubleWay(input);
            IList<WorkflowProcessInstanceTask> tasks = new List<WorkflowProcessInstanceTask>();
            //更新状态
            var markedInput = new WorkflowEngineActivityOrLineMarkedInput
            {
                Activity = input.CurrentActivityId,
                ProcessInstanceId = input.ProcessInstanceId
            };
            //插入流程任务表
            foreach (var activity in nextActivitys)
            {
                //判断该节点是否为结束节点
                var processTask = new WorkflowProcessInstanceTask
                {
                    TaskId = CombUtil.NewComb(),
                    Title = input.Title,
                    ProcessInstanceId = input.ProcessInstanceId,
                    PrevTaskId = input.CurrentTaskId,
                    PrevActivityId = input.CurrentActivityId,
                    PrevActivityName = input.CurrentActivityName,
                    CurrentActivityId = activity.ActivityId,
                    CurrentActivityName = activity.Name,
                    SendUserId = input.CurrentUser.UserId,
                    SendUserName = input.CurrentUser.Name,
                    SendTime = DateTime.Now,
                    Status = (byte)EnumTask.正在处理,
                    ReceiveTime = DateTime.Now
                };
                if (activity.AcitvityType == EnumAcitvityType.结束)
                {
                    processTask.Status = (byte)EnumTask.完成;
                    processTask.ReceiveUserId = input.CurrentUser.UserId;
                    processTask.ReceiveUserName = input.CurrentUser.Name;
                    processTask.DoUserId = input.CurrentUser.UserId;
                    processTask.DoUserName = input.CurrentUser.Name;
                    processTask.DoTime = DateTime.Now;
                    markedInput.ToActivity = processTask.CurrentActivityId;
                    await _workflowProcessInstanceLineLogic.UpdateLineMarked(markedInput);
                    tasks.Add(processTask);
                    //修改流程总状态及处理人
                    var processInstance = await _workflowProcessInstanceLogic.GetByIdAsync(input.ProcessInstanceId);
                    processInstance.Status = (byte)EnumProcessStatu.已完成;
                    processInstance.EndUserId = input.CurrentUser.UserId;
                    processInstance.EndUserName = input.CurrentUser.Name;
                    processInstance.EndTime = DateTime.Now;
                    processInstance.EndUserOrganization = input.CurrentUser.OrganizationName;
                    await _workflowProcessInstanceLogic.UpdateAsync(processInstance);
                    break;
                }
                //获取流程下一步处理人员
                var receiveUsers = await Task.Run(() => GetWorkflowEngineTaskNextReceiveUser(activity).ToList());
                if (!receiveUsers.Any())
                {
                    return new OperateStatus
                    {
                        ResultSign = ResultSign.Error,
                        Message = string.Format(ResourceWorkflow.处理人为空, activity.Name)
                    };
                }
                //获取流程处理人,为该流程所有处理人新增一条待办记录
                foreach (var receive in receiveUsers)
                {
                    processTask.ReceiveUserId = receive.ReceiveUserId;
                    processTask.ReceiveUserName = receive.ReceiveUserName;
                    markedInput.ToActivity = processTask.CurrentActivityId;
                    await _workflowProcessInstanceLineLogic.UpdateLineMarked(markedInput);
                    tasks.Add(processTask);
                }
            }
            //更新状态
            markedInput = new WorkflowEngineActivityOrLineMarkedInput
           {
               Activity = input.CurrentActivityId,
               ProcessInstanceId = input.ProcessInstanceId
           };
            //更新活动状态
            await _workflowProcessInstanceActivityLogic.UpdateActivityMarked(markedInput);

            //修改当前步骤为完成
            await _workflowProcessTaskLogic.UpdateProcessTaskStatus(new WorkflowEngineProcessTaskStatusInput()
                {
                    Comment = input.Comment,
                    PrincipalUser = input.CurrentUser,
                    Status = EnumActivityStatus.完成,
                    TaskId = input.CurrentTaskId
                });
            return await _workflowProcessTaskLogic.InsertMultipleDapperAsync(tasks);
        }

        /// <summary>
        ///     保存任务上一步
        /// </summary>
        /// <returns></returns>
        public Task<OperateStatus> SaveWorkflowEngineTaskSeedBack()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     跳转到某一步任务
        /// </summary>
        /// <returns></returns>
        public Task<OperateStatus> SaveWorkflowEngineTaskJump()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     撤回一步任务
        /// </summary>
        /// <returns></returns>
        public Task<OperateStatus> SaveWorkflowEngineTaskWithdraw()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     设置某一步为已读
        /// </summary>
        /// <returns></returns>
        public Task<OperateStatus> SaveWorkflowEngineSetTaskRead()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     设置委托任务
        /// </summary>
        /// <returns></returns>
        public Task<OperateStatus> SaveWorkflowEngineEntrust()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     是否为最后一步任务
        /// </summary>
        /// <returns></returns>
        public Task<OperateStatus> IsLastTask()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     获取流程监控信息:查看当前流程已走过步骤信息
        /// </summary>
        /// <returns></returns>
        public Task<WorkflowEngineMonitoringProcessOutput> GetWorkflowEngineMonitoringProcessOutput()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     开始启动流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> StartWorkflowEngineProcess(WorkflowEngineStartTaskInput input)
        {
            try
            {
                //查询流程信息
                var processActivity = await _workflowProcessActivityLogic.GetWorkflowProcessActivityByProcessId(new IdInput { Id = input.ProcessId });
                var processLine = await _workflowProcessLineLogic.GetWorkflowProcessLineByProcessId(new IdInput { Id = input.ProcessId });
                //插入流程实例信息
                var processInstance = new WorkflowProcessInstance
                {
                    Status = (byte)EnumProcessStatu.处理中,
                    ProcessInstanceId = CombUtil.NewComb(),
                    ProcessId = input.ProcessId,
                    Title = input.Title,
                    Urgency = input.Urgency,
                    CreateTime = DateTime.Now,
                    CreateUserId = input.PrincipalUser.UserId,
                    CreateUserName = input.PrincipalUser.Name,
                    CreateUserOrganization = input.PrincipalUser.OrganizationName,
                    UpdateTime = DateTime.Now,
                    UpdateUserId = input.PrincipalUser.UserId,
                    UpdateUserName = input.PrincipalUser.Name,
                    UpdateUserOrganization = input.PrincipalUser.OrganizationName,
                };
                //插入流程实例活动信息
                IList<WorkflowProcessInstanceActivity> processInstanceActivitys =
                    new List<WorkflowProcessInstanceActivity>();
                foreach (var activity in processActivity)
                {
                    var processInstanceActivity = activity.MapTo<WorkflowProcessInstanceActivity>();
                    processInstanceActivity.ProcessInstanceId = processInstance.ProcessInstanceId;
                    processInstanceActivity.Marked = false;
                    processInstanceActivitys.Add(processInstanceActivity);
                }
                //插入流程实例连线信息
                IList<WorkflowProcessInstanceLine> processInstanceLines = new List<WorkflowProcessInstanceLine>();
                foreach (var line in processLine)
                {
                    var processInstanceLine = line.MapTo<WorkflowProcessInstanceLine>();
                    processInstanceLine.ProcessInstanceId = processInstance.ProcessInstanceId;
                    processInstanceLine.Marked = false;
                    processInstanceLines.Add(processInstanceLine);
                }
                //插入数据
                await _workflowProcessInstanceLogic.InsertAsync(processInstance);
                await _workflowProcessInstanceActivityLogic.InsertMultipleDapperAsync(processInstanceActivitys);
                await _workflowProcessInstanceLineLogic.InsertMultipleDapperAsync(processInstanceLines);
                //获取发起步骤
                var startTask = await GetWorkflowEngineStartTaskOutput(input);
                //插入第一步流程任务信息
                var processTask = new WorkflowProcessInstanceTask
                {
                    TaskId = CombUtil.NewComb(),
                    Title = input.Title,
                    ProcessInstanceId = processInstance.ProcessInstanceId,
                    CurrentActivityId = startTask.ActivityId,
                    CurrentActivityName = startTask.Name,
                    SendUserId = input.PrincipalUser.UserId,
                    SendUserName = input.PrincipalUser.Name,
                    SendTime = DateTime.Now,
                    ReceiveUserId = input.PrincipalUser.UserId,
                    ReceiveUserName = input.PrincipalUser.Name,
                    ReceiveTime = DateTime.Now,
                    DoUserId = input.PrincipalUser.UserId,
                    DoUserName = input.PrincipalUser.Name,
                    DoTime = DateTime.Now,
                    Status = (byte)EnumActivityStatus.完成
                };
                await _workflowProcessTaskLogic.InsertAsync(processTask);

                //下一步
                var operateStatus = await SaveWorkflowEngineTaskNext(new WorkflowEngineRunnerInput
                {
                    Title = processInstance.Title,
                    ProcessInstanceId = processInstance.ProcessInstanceId,
                    CurrentActivityId = startTask.ActivityId,
                    CurrentActivityName = startTask.Name,
                    CurrentTaskId = processTask.TaskId,
                    CurrentUser = input.PrincipalUser,
                    ProcessId = startTask.ProcessId
                });

                return operateStatus;
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(FolderName.Workflow, "启动流程发生异常:" + ex.Message);
                return new OperateStatus
                {
                    ResultSign = ResultSign.Error,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        ///     保存该流程:填写好信息后并未发送该流程
        /// </summary>
        /// <returns></returns>
        public Task<OperateStatus> SaveWorkflowEngineProcess()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     删除流程信息
        /// </summary>
        /// <returns></returns>
        public Task<OperateStatus> DeleteWorkflowEngineProcess()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     更新流程信息
        /// </summary>
        /// <returns></returns>
        public Task<OperateStatus> UpdateWorkflowEngineProcess()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     取消流程
        /// </summary>
        /// <returns></returns>
        public Task<OperateStatus> SaveWorkflowEngineProcessCancel()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     所有发起过的流程(我的请求)
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<WorkflowEngineHaveSendProcessOutput>> GetWorkflowEngineHaveSendOutput(WorkflowEngineRunnerInput input)
        {
            return await _workflowEngineRepository.GetWorkflowEngineHaveSendOutput(input);
        }

        /// <summary>
        ///     获取监控-流程图形式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WorkflowEngineTrackForMapOutput> GetWorkflowEngineTrackForMap(WorkflowEngineRunnerInput input)
        {
            var process = await _workflowProcessLogic.GetByIdAsync(input.ProcessId);
            var nodes = await _workflowProcessInstanceActivityLogic.GetWorkflowEngineTrackActivityOutput(input);
            var lines = await _workflowProcessInstanceLineLogic.GetWorkflowEngineTrackLineOutput(input);
            var areas = await _workflowProcessAreasLogic.GetWorkflowEngineTrackAreasOutput(input);
            var returnRunJson = new StringBuilder();
            var runJson = new StringBuilder("{\"title\":\"" + process.Name + "\",\"nodes\":{");
            foreach (var node in nodes)
            {
                runJson.Append("\"" + node.ActivityId + "\":{\"name\":\"" + node.Name + "\",\"left\":" + node.Left +
                               ",\"top\":" + node.Top + ",\"type\":\"" + node.Type + "\",\"width\":" + node.Width +
                               ",\"height\":" + node.Height + ",\"alt\":true,\"marked\":" + node.Marked.ToLower() + "},");
            }
            returnRunJson.Append(runJson.ToString().TrimEnd(','));
            returnRunJson.Append("},\"lines\": {");
            runJson = new StringBuilder();
            foreach (var line in lines)
            {
                runJson.Append("\"" + line.LineId + "\":{\"name\":\"" + line.Name + "\",\"M\":" + line.M +
                               ",\"type\":\"" + line.Type + "\",\"from\":\"" + line.From + "\",\"to\":\"" + line.To +
                               "\",\"marked\":" + line.Marked.ToLower() + "},");
            }
            returnRunJson.Append(runJson.ToString().TrimEnd(','));
            returnRunJson.Append("},\"areas\": {");
            runJson = new StringBuilder();
            foreach (var area in areas)
            {
                runJson.Append("\"" + area.AreasId + "\":{\"name\":\"" + area.Name + "\",\"left\":" + area.Left +
                               ",\"top\":" + area.Top + ",\"color\":\"" + area.Color + "\",\"width\":" + area.Width +
                               ",\"height\":" + area.Height + ",\"alt\":true},");
            }
            returnRunJson.Append(runJson.ToString().TrimEnd(','));
            returnRunJson.Append("}}");
            return new WorkflowEngineTrackForMapOutput
            {
                DesignJson = returnRunJson.ToString()
            };
        }

        /// <summary>
        ///     获取监控-表格形式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<WorkflowEngineTrackForTableOutput>> GetWorkflowEngineTrackForTable(WorkflowEngineRunnerInput input)
        {
            return _workflowProcessTaskLogic.GetWorkflowEngineTrackForTable(input);
        }

        /// <summary>
        ///     获取需要处理任务信息
        /// </summary>
        /// <returns></returns>
        public async Task<WorkflowEngineDealWithTaskOutput> GetWorkflowEngineDealWithTaskOutput(WorkflowEngineRunnerInput input)
        {
            return await _workflowProcessInstanceActivityLogic.GetWorkflowEngineDealWithTaskOutput(input);
        }

        /// <summary>
        ///     获取下一步处理人员信息
        /// </summary>
        /// <param name="doubleWay"></param>
        /// <returns></returns>
        private IEnumerable<WorkflowEngineReceiveUserOutput> GetWorkflowEngineTaskNextReceiveUser(WorkflowEngineNextActivitysDoubleWay doubleWay)
        {
            ReceiveUserFactory receiveUserFactory = new ByUser();
            switch (doubleWay.ProcessorType)
            {
                case EnumActivityProcessorType.岗位:
                    receiveUserFactory = new ByPost();
                    break;
                case EnumActivityProcessorType.工作组:
                    receiveUserFactory = new ByGroup();
                    break;
                case EnumActivityProcessorType.所有成员:
                    receiveUserFactory = new ByAll();
                    break;
                case EnumActivityProcessorType.部门:
                    receiveUserFactory = new ByOrganization();
                    break;
            }
            return receiveUserFactory.GetWorkflowEngineReceiveUserOutputs(doubleWay);
        }

        #region 构造函数

        private readonly IWorkflowProcessLogic _workflowProcessLogic;
        private readonly IWorkflowProcessActivityLogic _workflowProcessActivityLogic;
        private readonly IWorkflowProcessLineLogic _workflowProcessLineLogic;
        private readonly IWorkflowEngineRepository _workflowEngineRepository;
        private readonly IWorkflowProcessInstanceLogic _workflowProcessInstanceLogic;
        private readonly IWorkflowProcessInstanceActivityLogic _workflowProcessInstanceActivityLogic;
        private readonly IWorkflowProcessInstanceLineLogic _workflowProcessInstanceLineLogic;
        private readonly IWorkflowProcessTaskLogic _workflowProcessTaskLogic;
        private readonly IWorkflowProcessAreasLogic _workflowProcessAreasLogic;

        public WorkflowEngineLogic(IWorkflowEngineRepository workflowEngineRepository,
            IWorkflowProcessLogic workflowProcessLogic,
            IWorkflowProcessActivityLogic workflowProcessActivityLogic,
            IWorkflowProcessLineLogic workflowProcessLineLogic,
            IWorkflowProcessInstanceLogic workflowProcessInstanceLogic,
            IWorkflowProcessInstanceActivityLogic workflowProcessInstanceActivityLogic,
            IWorkflowProcessInstanceLineLogic workflowProcessInstanceLineLogic,
            IWorkflowProcessTaskLogic workflowProcessTaskLogic,
            IWorkflowProcessAreasLogic workflowProcessAreasLogic)
        {
            _workflowProcessLogic = workflowProcessLogic;
            _workflowProcessActivityLogic = workflowProcessActivityLogic;
            _workflowProcessLineLogic = workflowProcessLineLogic;
            _workflowProcessInstanceLogic = workflowProcessInstanceLogic;
            _workflowProcessInstanceActivityLogic = workflowProcessInstanceActivityLogic;
            _workflowProcessInstanceLineLogic = workflowProcessInstanceLineLogic;
            _workflowProcessTaskLogic = workflowProcessTaskLogic;
            _workflowProcessAreasLogic = workflowProcessAreasLogic;
            _workflowEngineRepository = workflowEngineRepository;
        }

        #endregion
    }
}