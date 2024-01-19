using MiniExcelLibs.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ShipSurvey
{
    /// <summary>
    /// 船舶检验响应DTO
    /// </summary>
    public class ShipSurveyResponseDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 如果当前时间小于倒计时90天（IsBeyond=true） 泽这条数据整行标红  
        /// </summary>
        public bool IsBeyond { get; set; }
        /// <summary>
        /// 船名
        /// </summary>
        [ExcelColumnName("船名")]
        public string? ShipName { get; set; }

        /// <summary>
        /// 船舶登记号
        /// </summary>
        [ExcelColumnName("船舶登记号")]
        public string? ShipRegistNumber { get; set; }
        /// <summary>
        /// 船级社
        /// </summary>
        [ExcelColumnName("船级社")]
        public string? ClassSociety { get; set; }

        /// <summary>
        /// 上次检验时间
        /// </summary>
        [ExcelColumnName("上次检验时间")]
        public DateTime? PreSurveyTime { get; set; }
        /// <summary>
        /// 下次年检时间
        /// </summary>
        [ExcelColumnName("下次年检时间")]
        public DateTime? NextYearSurveyTime { get; set; }
        /// <summary>
        /// 中间检验
        /// </summary>
        [ExcelColumnName("中间检验")]
        public DateTime? IntermediateInspection { get; set; }
        /// <summary>
        /// 坞检时间
        /// </summary>
        [ExcelColumnName("坞检时间")]
        public DateTime? DockingInspectionTime { get; set; }
        /// <summary>
        /// 尾轴车叶
        /// </summary>
        [ExcelColumnName("尾轴车叶")]
        public DateTime? TailAxleBlade { get; set; }
        /// <summary>
        /// 特检
        /// </summary>
        [ExcelColumnName("特检")]
        public DateTime? SpecialSurvey { get; set; }
        /// <summary>
        /// 倒计时
        /// </summary>
        [ExcelColumnName("倒计时")]
        public string? Countdown { get; set; }
        /// <summary>
        /// 船级证书状态
        /// </summary>
        [ExcelColumnName("船级证书状态")]
        public string? ShipCertStatus { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        [ExcelColumnName("国籍")]
        public string? Nationality { get; set; }
        /// <summary>
        /// 国籍证书有效期
        /// </summary>
        [ExcelColumnName("国籍证书有效期")]
        public DateTime? NationalityCertValidTime { get; set; }
        /// <summary>
        /// 国籍证书状态
        /// </summary>
        [ExcelColumnName("国籍证书状态")]
        public string? NationalityCertStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [ExcelColumnName("备注")]
        public string? Break { get; set; }
        /// <summary>
        /// 航区
        /// </summary>
        [ExcelColumnName("航区")]
        public string? NavigationZone { get; set; }
        /// <summary>
        /// 航线 
        /// </summary>
        [ExcelColumnName("航线")]
        public string? AirRoute { get; set; }

        public int Sort { get; set; }
    }
}
