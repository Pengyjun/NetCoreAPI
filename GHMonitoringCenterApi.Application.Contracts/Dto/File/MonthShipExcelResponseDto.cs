using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.File
{
    /// <summary>
    /// 公司主要船舶施工情况月报
    /// </summary>
    public class MonthShipExcelResponseDto
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string TimeValue { get; set; }

        public List<MonthShipInfo> monthShipInfos { get; set; }

    }

    /// <summary>
    /// 基本信息
    /// </summary>
    public class MonthShipInfo
    {
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string ShipName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 完成方量（m³）
        /// </summary>
        public int CompletedQuantity { get; set; }
        /// <summary>
        /// 效率
        /// </summary>
        public int? Efficiency { get; set; }
        /// <summary>
        /// 运转(h)
        /// </summary>
        public string OperateHours { get; set; }
        /// <summary>
        /// 时间利用率%
        /// </summary>
        public int UtilizationRate { get; set; }
        /// <summary>
        /// 生产停歇(h)
        /// </summary>
        public string ProductionStoppage { get; set; }
        /// <summary>
        /// 船机(h)
        /// </summary>
        public string MarineEngine { get; set; }
        /// <summary>
        /// 管线(h)
        /// </summary>
        public string PipeLine { get; set; }
        /// <summary>
        /// 自然(h)
        /// </summary>
        public string Nature { get; set; }
        /// <summary>
        /// 其他(h)
        /// </summary>
        public string Other { get; set; }
        /// <summary>
        /// 月船机完好率
        /// </summary>
        public int MonthMarineEngine { get; set; }
        /// <summary>
        /// 年船机完好率
        /// </summary>
        public int YearMarineEngine { get; set; }
        /// <summary>
        /// 完成产值
        /// </summary>
        public int CompletedOutputValue { get; set; }
        /// <summary>
        /// 备注/说明
        /// </summary>
        public string? Remarks { get; set; }
    }
}
