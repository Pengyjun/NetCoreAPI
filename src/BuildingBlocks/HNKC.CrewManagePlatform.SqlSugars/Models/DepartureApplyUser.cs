using HNKC.CrewManagePlatform.Models.Enums;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models;

/// <summary>
/// 离船申请人员
/// </summary>
[SugarTable("t_departure_apply_user", IsDisabledDelete = true, TableDescription = "离船申请人员")]
public class DepartureApplyUser : BaseEntity<long>
{
    /// <summary>
    /// 申请code
    /// </summary>
    [SugarColumn(ColumnName = "apply_code", Length = 36, ColumnDescription = "申请code")]
    public Guid? ApplyCode { get; set; }

    /// <summary>
    /// 离船用户ID
    /// </summary>
    [SugarColumn(ColumnName = "user_id", Length = 36, ColumnDescription = "离船用户ID")]
    public Guid? UserId { get; set; }

    /// <summary>
    /// 离船日期
    /// </summary>
    [SugarColumn(ColumnName = "disembark_date", ColumnDataType = "datetime", ColumnDescription = "离船日期")]
    public DateTime? DisembarkDate { get; set; }

    /// <summary>
    /// 归船日期
    /// </summary>
    [SugarColumn(ColumnName = "return_ship_date", ColumnDataType = "datetime", ColumnDescription = "归船日期")]
    public DateTime? ReturnShipDate { get; set; }

    /// <summary>
    /// 实际离船日期
    /// </summary>
    [SugarColumn(ColumnName = "real_disembark_date", ColumnDataType = "datetime", ColumnDescription = "实际离船日期")]
    public DateTime? RealDisembarkDate { get; set; }

    /// <summary>
    /// 实际归船日期
    /// </summary>
    [SugarColumn(ColumnName = "real_return_ship_date", ColumnDataType = "datetime", ColumnDescription = "实际归船日期")]
    public DateTime? RealReturnShipDate { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [SugarColumn(ColumnName = "phone", Length = 11, ColumnDescription = "联系电话")]
    public string? Phone { get; set; }

    /// <summary>
    /// 固话
    /// </summary>
    [SugarColumn(ColumnName = "fiexd_line", Length = 20, ColumnDescription = "固话")]
    public string? FiexdLine { get; set; }

    /// <summary>
    /// 替班人员
    /// </summary>
    [SugarColumn(ColumnName = "relief_user_id", Length = 36, ColumnDescription = "替班人员")]
    public Guid? ReliefUserId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "remark", Length = 1000, ColumnDescription = "备注")]
    public string? Remark { get; set; }
}