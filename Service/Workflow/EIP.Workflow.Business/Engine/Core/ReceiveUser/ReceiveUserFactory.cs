using System.Collections.Generic;
using EIP.Workflow.Models.Dtos.Engine;

namespace EIP.Workflow.Business.Engine.Core.ReceiveUser
{
    /// <summary>
    ///     获取下一步处理人工厂
    /// </summary>
    public abstract class ReceiveUserFactory
    {
        /// <summary>
        ///     获取处理人员
        /// </summary>
        /// <param name="doubleWay"></param>
        /// <returns></returns>
        public abstract IEnumerable<WorkflowEngineReceiveUserOutput> GetWorkflowEngineReceiveUserOutputs(
            WorkflowEngineNextActivitysDoubleWay doubleWay);
    }
}