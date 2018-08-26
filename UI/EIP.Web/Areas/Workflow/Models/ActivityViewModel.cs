using System;

namespace EIP.Web.Areas.Workflow.Models
{
    /// <summary>
    /// 审批活动节点ViewModel
    /// </summary>
    public class ActivityViewModel
    {
        #region 基本
        /// <summary>
        /// 活动Id
        /// </summary>
        public Guid ActivityId { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 意见显示
        /// </summary>
        public byte ActivityOpinion { get; set; }

        /// <summary>
        /// 审签类型
        /// </summary>
        public byte ActivityCommentsBelow { get; set; }

        /// <summary>
        /// 超期提示
        /// </summary>
        public byte ActivityTimeoutRemind { get; set; }

        /// <summary>
        /// 是否归档
        /// </summary>
        public byte ActivityArchive { get; set; }

        /// <summary>
        /// 工时(小时)
        /// </summary>
        public double ActivityWorkTime { get; set; }

        /// <summary>
        /// 超期提示类型
        /// </summary>
        public byte ActivityTimeoutRemindType { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string ActivityRemark { get; set; }

        /// <summary>
        /// 超期邮件提醒
        /// </summary>
        public bool ActivityTimeoutRemindTypeEmail { get; set; }

        /// <summary>
        /// 超期短信提醒
        /// </summary>
        public bool ActivityTimeoutRemindTypeNote { get; set; }

        /// <summary>
        /// 超期微信提醒
        /// </summary>
        public bool ActivityTimeoutRemindTypeWx { get; set; }

        #endregion

        #region 策略
        /// <summary>
        /// 处理者类型
        /// </summary>
        public byte ActivityProcessorType { get; set; }

        /// <summary>
        /// 处理者
        /// </summary>
        public string ActivityProcessorHandler { get; set; }

        /// <summary>
        /// 处理策略
        /// </summary>
        public byte ActivityHandlingStrategy { get; set; }

        /// <summary>
        /// 策略百分比
        /// </summary>
        public double ActivityHandlingStrategyPercentage { get; set; }
        #endregion

        #region 数据

        #endregion

        #region 事件
        /// <summary>
        /// 步骤提交前事件
        /// </summary>
        public string ActivityEventSubmitBefore { get; set; }

        /// <summary>
        /// 步骤提交后事件
        /// </summary>
        public string ActivityEventSubmitAfter { get; set; }

        /// <summary>
        /// 步骤退回前事件
        /// </summary>
        public string ActivityEventBackBefore { get; set; }

        /// <summary>
        /// 步骤退回后事件
        /// </summary>
        public string ActivityEventBackAfter { get; set; }
        #endregion

    }
}