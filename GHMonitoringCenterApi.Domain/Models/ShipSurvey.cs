using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 船舶检验表
    /// </summary>
    [SugarTable("t_shipsurvey", IsDisabledDelete = true)]
    public class ShipSurvey:BaseEntity<Guid>
    {
        /// <summary>
        /// 船名
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ShipName { get; set; }

        /// <summary>
        /// 船舶登记号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ShipRegistNumber { get; set; }
        /// <summary>
        /// 船级社
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ClassSociety { get; set; }

        /// <summary>
        /// 上次检验时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? PreSurveyTime { get; set; }
        /// <summary>
        /// 下次年检时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? NextYearSurveyTime { get; set; }
        /// <summary>
        /// 中间检验
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? IntermediateInspection { get; set; }
        /// <summary>
        /// 坞检时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? DockingInspectionTime { get; set; }
        /// <summary>
        /// 尾轴车叶
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? TailAxleBlade { get; set; }
        /// <summary>
        /// 特检
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? SpecialSurvey { get; set; }
        /// <summary>
        /// 倒计时
        /// </summary>
        [SugarColumn(Length =50)]
        public string? Countdown { get; set; }
        /// <summary>
        /// 船级证书状态
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ShipCertStatus { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? Nationality { get; set; }
        /// <summary>
        /// 国籍证书有效期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? NationalityCertValidTime { get; set; }
        /// <summary>
        /// 国籍证书状态
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? NationalityCertStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? Break { get; set; }
        /// <summary>
        /// 航区
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? NavigationZone { get; set; }
        /// <summary>
        /// 航线 
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? AirRoute { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int? Sort { get; set; }
    }
}
