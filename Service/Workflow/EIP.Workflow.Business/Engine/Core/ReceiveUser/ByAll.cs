using System.Collections.Generic;
using System.Linq;
using EIP.Common.Entities.Dtos;
using EIP.System.Business.Identity;
using EIP.Workflow.Models.Dtos.Engine;

namespace EIP.Workflow.Business.Engine.Core.ReceiveUser
{
    /// <summary>
    ///     下一步处理人员:所有
    /// </summary>
    public class ByAll : ReceiveUserFactory
    {
        public override IEnumerable<WorkflowEngineReceiveUserOutput> GetWorkflowEngineReceiveUserOutputs(
            WorkflowEngineNextActivitysDoubleWay doubleWay)
        {
            //查询所有未冻结用户信息
            var userInfoLogic = new SystemUserInfoLogic();
            var userInfos = userInfoLogic.GetUser(new FreezeInput(false)).Result;
            return userInfos.Select(user => new WorkflowEngineReceiveUserOutput
            {
                ReceiveUserId = user.UserId,
                ReceiveUserName = user.Name
            });
        }
    }
}