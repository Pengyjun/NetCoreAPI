using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// ProjectWBS 开累数调整  调整2024年12月之前的开累数，2025年之后就采用调整后的开累数，2025年之前的采用之前的逻辑
    /// 2025年之后的开累数一定是对的   之前的可能不对  
    /// </summary>
    [SugarTable("t_projectadjustwbs", IsDisabledDelete =true)]
    public class ProjectAdjustWBS : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        [SugarColumn(Length =36)]
        public string? ProjectId { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        [SugarColumn(Length = 50,IsNullable =true)]
        public string? ProjectNum { get; set; }
       
        /// <summary>
        /// 项目WBSId
        /// </summary>
        [SugarColumn(Length = 50,IsNullable = true)]
        public string? ProjectWBSId { get; set; }
        
        /// <summary>
        /// keyId
        /// </summary>
        [SugarColumn(Length = 50,IsNullable = true)]
        public string? KeyId { get; set; }
        /// <summary>
        /// Pid
        /// </summary>
        [SugarColumn(Length = 50,IsNullable = true)]
        public string? Pid { get; set; }
        /// <summary>
        /// 中欧
        /// </summary>
        [SugarColumn(Length =100,IsNullable = true)]
        public string? Def { get; set; }
        /// <summary>
        /// 项目数
        /// </summary>
        [SugarColumn(Length = 100,IsNullable = true)]
        public string? ItemNum { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 200,IsNullable =true)]
        public string? Name { get; set; }
        /// <summary>
        /// 昨日
        /// </summary>
        [SugarColumn(Length = 50 ,IsNullable = true)]
        public string? Prev { get; set; }   
        /// <summary>
        /// 下一个
        /// </summary>
        [SugarColumn(Length = 50,IsNullable =true)]
        public string? DownOne { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// 工程量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? EngQuantity { get; set; }
        /// <summary>
        /// 合同额(目前此字段并未采用，pom系统并无此字段，当前合同额=单价*工程量)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", IsNullable = true)]
        public decimal? ContractAmount { get; set; }
        /// <summary>
        /// 地位
        /// </summary>
        [SugarColumn(Length = 100,IsNullable = true)]
        public string? Status { get; set; }
    }
}
