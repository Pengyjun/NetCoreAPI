using HNKC.CrewManagePlatform.Models.Enums;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models;

/// <summary>
/// 离船申请
/// </summary>
[SugarTable("t_departure_apply", IsDisabledDelete = true, TableDescription = "离船申请")]
public class DepartureApply : BaseEntity<long>
{
    /// <summary>
    /// 申请code
    /// </summary>
    [SugarColumn(ColumnName="apply_code",Length = 36, ColumnDescription = "申请code")]
    public Guid? ApplyCode { get; set; }

    /// <summary>
    /// 申请用户Id
    /// </summary>
    [SugarColumn(ColumnName="user_id",Length = 36, ColumnDescription = "申请用户ID")]
    public Guid? UserId { get; set; }

    /// <summary>
    /// 船舶ID
    /// </summary>
    [SugarColumn(ColumnName="ship_id",Length = 36, ColumnDescription = "船舶ID")]
    public Guid? ShipId { get; set; }

    /// <summary>
    /// 离船日期
    /// </summary>
    [SugarColumn(ColumnName="disembark_date",ColumnDataType = "datetime", ColumnDescription = "离船日期")]
    public DateTime? DisembarkDate { get; set; }

    /// <summary>
    /// 审批用户ID
    /// </summary>
    [SugarColumn(ColumnName="approve_user_id",Length = 36, ColumnDescription = "审批用户")]
    public Guid? ApproveUserId { get; set; }

    /// <summary>
    /// 审批状态：0-审批中;1-审批通过;2-审批未通过
    /// </summary>
    [SugarColumn(ColumnName="approve_status",ColumnDataType = "int", ColumnDescription = "审批状态：0-审批中;1-审批通过;2-审批未通过", DefaultValue = "0")]
    public ApproveStatus ApproveStatus { get; set; }

    /// <summary>
    /// 审批意见
    /// </summary>
    [SugarColumn(ColumnName="approve_opinion",Length = 1000, ColumnDescription = "审批意见")]
    public string? ApproveOpinion { get; set; }

    /// <summary>
    /// 审批时间
    /// </summary>
    [SugarColumn(ColumnName="approve_time",ColumnDataType = "datetime", ColumnDescription = "审批时间")]
    public DateTime? ApproveTime { get; set; }

    /// <summary>
    /// 申请时间
    /// </summary>
    [SugarColumn(ColumnName="apply_time",ColumnDataType = "datetime", ColumnDescription = "申请时间")]
    public DateTime? ApplyTime { get; set; }
}