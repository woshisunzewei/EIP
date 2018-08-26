using System;
using EIP.Common.Entities;
using EIP.Common.Entities.CustomAttributes;

namespace EIP.System.Models.Entities
{
    /// <summary>
    /// Gantt_Task表实体类
    /// </summary>
    [Serializable]
    [Table(Name = "Gantt_Task")]
    public class GanttTask : EntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Id(true)]
        public Guid Id { get; set; }

        /// <summary>
        /// 开始时间 
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        ///  父任务
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 是否是叶节点
        /// </summary>
        public bool IsLeaf { get; set; }

        /// <summary>
        /// 完成的百分比
        /// </summary>
        public int PercentDone { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 优先
        /// </summary>
        public string Priority { get; set; }


    }
}