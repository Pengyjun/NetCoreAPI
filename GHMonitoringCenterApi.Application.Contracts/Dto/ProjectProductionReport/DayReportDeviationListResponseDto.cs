using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport
{
    /// <summary>
    /// 日报偏差值列表
    /// </summary>
    public class DayReportDeviationListResponseDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 状态id
        /// </summary>
        public Guid? StatusId { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string? StatusName { get; set; }
        /// <summary>
        /// 当日实际产值（元）
        /// </summary>
        public decimal DayActualProductionAmount { get; set; }
        /// <summary>
        /// 日报偏差值
        /// </summary>
        public decimal DayActualProductionAmountDeviation { get; set; }
        /// <summary>
        /// 开累产值
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 开累偏差值
        /// </summary>
        public decimal TotalAmountDeviation { get; set; }
        /// <summary>
        ///  false是按日统计    true是按月+日报的形式统计
        /// </summary>
        public bool IsDayCalc { get; set; }
        /// <summary>
        /// 日期 int类型
        /// </summary>
        public int Dateday { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string DatedayStr { get; set; }
    }
}
