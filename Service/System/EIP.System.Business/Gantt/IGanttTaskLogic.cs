using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Entities;
using EIP.System.Models.Entities;

namespace EIP.System.Business.Gantt
{
	/// <summary>
    /// 任务表业务逻辑接口
    /// </summary>
    public interface IGanttTaskLogic : IAsyncLogic<GanttTask>
    {
        /// <summary>
        ///     保存
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<OperateStatus> Save(GanttTask entity);
    }
}