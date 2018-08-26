using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Core.Extensions;
using EIP.Common.Core.Utils;
using EIP.Common.Entities;
using EIP.System.DataAccess.Gantt;
using EIP.System.Models.Entities;

namespace EIP.System.Business.Gantt
{
    /// <summary>
    ///     甘特图图像记录表业务逻辑接口实现
    /// </summary>
    public class GanttDependencieLogic : AsyncLogic<GanttDependencie>, IGanttDependencieLogic
    {
        #region 构造函数
        private readonly IGanttDependencieRepository _ganttDependencieRepository;
        public GanttDependencieLogic(IGanttDependencieRepository ganttDependencieRepository)
            : base(ganttDependencieRepository)
        {
            _ganttDependencieRepository = ganttDependencieRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        ///     保存
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(GanttDependencie entity)
        {
            if (!entity.Id.IsEmptyGuid())
                return await UpdateAsync(entity);
            entity.Id = CombUtil.NewComb();
            return await InsertAsync(entity);
        }
        #endregion
    }
}