using EIP.Common.DataAccess;
using EIP.System.Models.Entities;

namespace EIP.System.DataAccess.Gantt
{
    /// <summary>
    ///     甘特图图像记录表数据访问接口实现
    /// </summary>
    public class GanttDependencieRepository : DapperAsyncRepository<GanttDependencie>, IGanttDependencieRepository
    {
    }
}