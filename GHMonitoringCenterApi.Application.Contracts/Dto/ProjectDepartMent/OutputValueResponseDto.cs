using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectDepartMent
{
    /// <summary>
    /// 项目部产值dto
    /// </summary>
    public class OutputValueResponseDto
    {
        /// <summary>
        /// 今日计划产值（万元）
        /// </summary>
        public decimal TodayPlanOutput { get; set; }
        /// <summary>
        /// 今日完成产值（万元）
        /// </summary>
        public decimal TodayActualOutput { get; set; }
        /// <summary>
        /// 昨日产值统计（万元）
        /// </summary>
        public decimal yesterdayOutputTotal { get; set; }
        /// <summary>
        /// 近7天产值统计（万元）
        /// </summary>
        public decimal SevenDayOutputTotal { get; set; }
        /// <summary>
        /// 状态标识 在建正常显示数据  停工则显示项目暂停暂无计划
        /// </summary>
        public string State { get; set; }
    }
}
