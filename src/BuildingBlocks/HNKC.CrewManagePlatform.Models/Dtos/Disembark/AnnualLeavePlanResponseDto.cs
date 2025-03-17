using HNKC.CrewManagePlatform.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Disembark
{
    /// <summary>
    /// 年休假计划列表
    /// </summary>
    public class AnnualLeavePlanResponseDto
    {
        /// <summary>
        /// 船舶id
        /// </summary>
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 休假年份
        /// </summary>
        public int LeaveYear { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 船舶首字母拼音
        /// </summary>
        public string ShipNamePinyin { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public ShipTypeEnum ShipType { get; set; }
        /// <summary>
        /// 船舶类型名称
        /// </summary>
        public string ShipTypeName { get; set; }
        /// <summary>
        /// 所在国家Id
        /// </summary>
        public Guid? CountryId { get; set; }
        /// <summary>
        /// 所在国家名称
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// 所在项目
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 所属公司Id
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 所属公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 船长
        /// </summary>
        public string Captain { get; set; }
        /// <summary>
        /// 书记
        /// </summary>
        public string Secretary { get; set; }
        /// <summary>
        /// 轮机长
        /// </summary>
        public string ChiefEngineer { get; set; }
        /// <summary>
        /// 填报状态
        /// </summary>
        public bool SubStatus { get; set; }
        /// <summary>
        /// 填报人
        /// </summary>
        public string SubUser { get; set; }
        /// <summary>
        /// 填报时间
        /// </summary>
        public string SubTime { get; set; }
    }
}
