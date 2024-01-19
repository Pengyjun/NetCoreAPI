using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Push
{
    /// <summary>
    /// pom推送项目月报
    /// </summary>
    public class PushPomProjectMonthRequestDto
    {
        public string ProjectMonthJson { get; set; }
    }

    /// <summary>
    /// pom推送项目月报(单位：元，方)
    /// </summary>
    public class PomProjectMonthRepRequestDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 本月计划产值
        /// </summary>
        public decimal? PlannedOutputValueMonth { get; set; }
        /// <summary>
        /// 本月完成产值
        /// </summary>
        public decimal? CompleteOutputValue { get; set; }
        /// <summary>
        /// 本月完成工程量
        /// </summary>
        public decimal? CompleteVolumeMonth { get; set; }
        /// <summary>
        /// 本月甲方确认产值
        /// </summary>
        public decimal ConfirmedOutputValue { get; set; }
        /// <summary>
        /// 本月甲方付款金额
        /// </summary>
        public decimal? PaymentAmountOfParty { get; set; }
        /// <summary>
        /// 本月应收账款
        /// </summary>
        public decimal? MonthAccountReceiva { get; set; }
        /// <summary>
        /// 进度偏差主因 不可为空
        /// </summary>
        public string MainCause { get; set; }
        /// <summary>
        /// 进度偏差主因描述
        /// </summary>
        public string CauseAnalysis { get; set; }
        /// <summary>
        /// 主要形象进度描述 不可为空
        /// </summary>
        public string ProgressDescription { get; set; }
        /// <summary>
        /// 下月预计成本
        /// </summary>
        public decimal? EstimatedCost { get; set; }
        /// <summary>
        /// 本月实际成本
        /// </summary>
        public decimal? ActualCost { get; set; }
        /// <summary>
        /// 本月预计成本
        /// </summary>
        public decimal? EstimatedCostMonth { get; set; }
        /// <summary>
        ///  成本偏差主因 不可为空
        /// </summary>
        public string MainCausesofCostDeviation { get; set; }
        /// <summary>
        /// 为1  代表项目已经过了填报时间 需要强制更改项目月报  否则会根据时间限制数据接收  默认0
        /// </summary>
        public int IsMonthRep { get; set; }
        /// <summary>
        /// 填报月份
        /// </summary>
        public DateTime StatementMonth { get; set; }
    }
}
