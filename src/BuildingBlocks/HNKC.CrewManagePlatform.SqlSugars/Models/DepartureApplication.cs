using HNKC.CrewManagePlatform.Models.Enums;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 离船申请
    /// </summary>
    [SugarTable("t_departureapplication", IsDisabledDelete = true, TableDescription = "紧急联系人")]
    public class DepartureApplication : BaseEntity<long>
    {
        /// <summary>
        /// 离船用户
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "离船用户")]
        public Guid? UserId { get; set; }
        /// <summary>
        /// 离船日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "离船日期")]
        public DateTime? DisembarkTime { get; set; }
        /// <summary>
        /// 归船日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "归船日期")]
        public DateTime? ReturnShipTime { get; set; }
        /// <summary>
        /// 实际离船日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "实际离船日期")]
        public DateTime? RealDisembarkTime { get; set; }
        /// <summary>
        /// 实际归船日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "实际归船日期")]
        public DateTime? RealReturnShipTime { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [SugarColumn(Length = 11, ColumnDescription = "联系电话")]
        public string? Phone { get; set; }
        /// <summary>
        /// 固话
        /// </summary>
        [SugarColumn(Length = 20, ColumnDescription = "固话")]
        public string? FiexdLine { get; set; }
        /// <summary>
        /// 替班人员
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "替班人员")]
        public Guid? ReliefUserId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 1000, ColumnDescription = "备注")]
        public string? Remark { get; set; }
        /// <summary>
        /// 船舶
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "船舶")]
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 审批用户
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "审批用户")]
        public Guid? ApproveUserId { get; set; }
        /// <summary>
        /// 审批状态 0未审批
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "审批状态", DefaultValue = "0")]
        public ApproveStatus ApproveStatus { get; set; }
        /// <summary>
        /// 关联键
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "关联键")]
        public Guid? DisembarkId { get; set; }
        /// <summary>
        /// 离船日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "离船日期")]
        public DateTime? DisembarkDate { get; set; }
    }
}
