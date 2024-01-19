using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ShipSurvey
{

    /// <summary>
    /// 新增或修改船舶检验请求DTO
    /// </summary>
    public class SaveShipSurveyRequestDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 船名
        /// </summary>
       
        public string? ShipName { get; set; }

        /// <summary>
        /// 船舶登记号
        /// </summary>
       
        public string? ShipRegistNumber { get; set; }
        /// <summary>
        /// 船级社
        /// </summary>
       
        public string? ClassSociety { get; set; }

        /// <summary>
        /// 上次检验时间
        /// </summary>
     
        public DateTime? PreSurveyTime { get; set; }
        /// <summary>
        /// 下次年检时间
        /// </summary>
     
        public DateTime? NextYearSurveyTime { get; set; }
        /// <summary>
        /// 中间检验
        /// </summary>
     
        public DateTime? IntermediateInspection { get; set; }
        /// <summary>
        /// 坞检时间
        /// </summary>
     
        public DateTime? DockingInspectionTime { get; set; }
        /// <summary>
        /// 尾轴车叶
        /// </summary>
     
        public DateTime? TailAxleBlade { get; set; }
        /// <summary>
        /// 特检
        /// </summary>
     
        public DateTime? SpecialSurvey { get; set; }
        /// <summary>
        /// 倒计时
        /// </summary>
        
        //public string? Countdown { get; set; }
        /// <summary>
        /// 船级证书状态
        /// </summary>
        
        //public string? ShipCertStatus { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        
        public string? Nationality { get; set; }
        /// <summary>
        /// 国籍证书有效期
        /// </summary>
     
        public DateTime? NationalityCertValidTime { get; set; }
        /// <summary>
        /// 国籍证书状态
        /// </summary>
        
        //public string? NationalityCertStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
       
        public string? Break { get; set; }
        /// <summary>
        /// 航区
        /// </summary>
       
        public string? NavigationZone { get; set; }
        /// <summary>
        /// 航线 
        /// </summary>
       
        public string? AirRoute { get; set; }
    }
}
