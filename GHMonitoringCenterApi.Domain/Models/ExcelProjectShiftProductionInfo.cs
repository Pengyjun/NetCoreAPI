using SqlSugar;
namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// Excel项目带班生产动态
    /// </summary>
    [SugarTable("t_excelprojectshiftproductioninfo", IsDisabledDelete = true)]
    public class ExcelProjectShiftProductionInfo : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ProjectName { get; set; }
        /// <summary>
        /// 现场带班领导
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ShiftLeader { get; set; }
        /// <summary>
        /// 值班电话
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ShiftPhone { get; set; }
        /// <summary>
        /// 现场管理人员
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? SiteManagementPersonNum { get; set; }
        /// <summary>
        /// 陆域作业人员
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? SiteConstructionPersonNum { get; set; }
        /// <summary>
        /// 路域设备
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? ConstructionDeviceNum { get; set; }
        /// <summary>
        /// 陆域3-9人以上作业地点（处）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? FewLandWorkplace { get; set; }
        /// <summary>
        /// 陆域10人以上作业地点（处）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? LandWorkplace { get; set; }
        /// <summary>
        /// 在场船舶（艘）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? SiteShipNum { get; set; }
        /// <summary>
        /// 在船人员（人）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? OnShipPersonNum { get; set; }
        /// <summary>
        /// 当日危大工程施工（项）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? HazardousConstructionNum { get; set; }
        /// <summary>
        ///  简述当日危大工程施工内容
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? HazardousConstructionDescription { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Year { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Month { get; set; }
        /// <summary>
        /// 天
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }
    }
}
