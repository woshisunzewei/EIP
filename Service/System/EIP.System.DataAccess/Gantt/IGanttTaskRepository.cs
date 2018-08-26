using EIP.Common.DataAccess;
using EIP.System.Models.Entities;

namespace EIP.System.DataAccess.Gantt
{
    /// <summary>
    /// 任务表数据访问接口
    /// </summary>
    public interface IGanttTaskRepository : IAsyncRepository<GanttTask>
    {
        
    }
}