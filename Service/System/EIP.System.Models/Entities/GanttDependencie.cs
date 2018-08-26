using System;
using EIP.Common.Entities;
using EIP.Common.Entities.CustomAttributes;

namespace EIP.System.Models.Entities
{
    /// <summary>
    /// Gantt_Dependencie表实体类
    /// </summary>
    [Serializable]
    [Table(Name = "Gantt_Dependencie")]
    public class GanttDependencie : EntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Id(true)]
        public Guid Id { get; set; }

        /// <summary>
        /// 前置任务的ID
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// 任务的ID
        /// </summary>
        public int To { get; set; }

        /// <summary>
        /// 任务跟任务之间的可以有四种关系：完成-完成(FF) 0，完成-开始(FS) 1，开始-完成(SF) 2，开始-开始(SS) 3
        /// </summary>
        public int Type { get; set; }
    }
}