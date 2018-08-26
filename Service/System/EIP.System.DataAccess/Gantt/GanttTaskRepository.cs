using EIP.Common.DataAccess;
using EIP.System.Models.Entities;

namespace EIP.System.DataAccess.Gantt
{
    /// <summary>
    ///     任务表数据访问接口实现
    /// </summary>
    public class GanttTaskRepository : DapperAsyncRepository<GanttTask>, IGanttTaskRepository
    {
    }
}