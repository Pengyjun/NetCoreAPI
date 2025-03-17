using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.SqlSugars.Models;

/// <summary>
/// 离船申请人员
/// </summary>
public class DepartureApplyUserVo
{
    /// <summary>
    /// 业务Code
    /// </summary>
    public Guid? BusinessId { get; set; }

    /// <summary>
    /// 离船用户ID
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// 人员名称
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 职务
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// 职务名称
    /// </summary>
    public string? PositionName { get; set; }

    /// <summary>
    /// 职工号
    /// </summary>
    public string? WorkNumber { get; set; }

    /// <summary>
    /// 离船日期
    /// </summary>
    public DateTime? DisembarkDate { get; set; }

    /// <summary>
    /// 归船日期
    /// </summary>
    public DateTime? ReturnShipDate { get; set; }

    /// <summary>
    /// 实际离船日期
    /// </summary>
    public DateTime? RealDisembarkDate { get; set; }

    /// <summary>
    /// 实际归船日期
    /// </summary>
    public DateTime? RealReturnShipDate { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 固话
    /// </summary>
    public string? FiexdLine { get; set; }

    /// <summary>
    /// 替班人员
    /// </summary>
    public Guid? ReliefUserId { get; set; }

    /// <summary>
    /// 替班人员名称
    /// </summary>
    public string? ReliefUserName { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}