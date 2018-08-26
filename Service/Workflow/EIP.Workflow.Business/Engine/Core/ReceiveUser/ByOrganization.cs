using System;
using System.Collections.Generic;
using EIP.Workflow.Models.Dtos.Engine;

namespace EIP.Workflow.Business.Engine.Core.ReceiveUser
{
    /// <summary>
    ///     下一步处理人员:部门
    /// </summary>
    public class ByOrganization : ReceiveUserFactory
    {
        public override IEnumerable<WorkflowEngineReceiveUserOutput> GetWorkflowEngineReceiveUserOutputs(
            WorkflowEngineNextActivitysDoubleWay doubleWay)
        {
            throw new NotImplementedException();
        }
    }
}