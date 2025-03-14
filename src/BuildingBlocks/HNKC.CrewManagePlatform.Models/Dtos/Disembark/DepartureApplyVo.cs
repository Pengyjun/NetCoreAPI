using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;

namespace HNKC.CrewManagePlatform.Models.Dtos.Disembark;

/// <summary>
/// 离船申请Vo
/// </summary>
public class DepartureApplyVo
{
    /// <summary>
    /// 申请code
    /// </summary>
    public Guid? ApplyCode { get; set; }

    /// <summary>
    /// 申请人ID
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// 申请人
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 申请时间
    /// </summary>
    public DateTime? ApplyTime { get; set; }

    /// <summary>
    /// 船舶ID
    /// </summary>
    public Guid? ShipId { get; set; }

    /// <summary>
    /// 船舶名称
    /// </summary>
    public string? ShipName { get; set; }

    /// <summary>
    /// 船舶类型
    /// </summary>
    public ShipTypeEnum ShipType { get; set; }

    /// <summary>
    /// 船舶类型名称
    /// </summary>
    public string? ShipTypeName { get; set; }

    /// <summary>
    /// 所在国家
    /// </summary>
    public Guid? Country { get; set; }

    /// <summary>
    /// 所在国家
    /// </summary>
    public string? CountryName { get; set; }

    /// <summary>
    /// 所属公司
    /// </summary>
    public Guid? Company { get; set; }

    /// <summary>
    /// 所属公司
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// 所在项目
    /// </summary>
    public string? ProjectName { get; set; }

    /// <summary>
    /// 审批状态：0-审批中;1-审批通过;2-审批未通过
    /// </summary>
    public ApproveStatus ApproveStatus { get; set; }

    /// <summary>
    /// 离船日期
    /// </summary>
    public DateTime? DisembarkDate { get; set; }
}

/// <summary>
/// 离船申请单详情
/// </summary>
public class DepartureApplyDetailVo
{
    /// <summary>
    /// 申请code
    /// </summary>
    public Guid? ApplyCode { get; set; }

    /// <summary>
    /// 申请人ID
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// 申请人
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 申请时间
    /// </summary>
    public DateTime? ApplyTime { get; set; }

    /// <summary>
    /// 船舶ID
    /// </summary>
    public Guid? ShipId { get; set; }

    /// <summary>
    /// 船舶名称
    /// </summary>
    public string? ShipName { get; set; }

    /// <summary>
    /// 船舶类型
    /// </summary>
    public ShipTypeEnum ShipType { get; set; }

    /// <summary>
    /// 船舶类型名称
    /// </summary>
    public string? ShipTypeName { get; set; }

    /// <summary>
    /// 所在国家
    /// </summary>
    public Guid? Country { get; set; }

    /// <summary>
    /// 所在国家
    /// </summary>
    public string? CountryName { get; set; }

    /// <summary>
    /// 所属公司
    /// </summary>
    public Guid? Company { get; set; }

    /// <summary>
    /// 所属公司
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// 所在项目
    /// </summary>
    public string? ProjectName { get; set; }

    /// <summary>
    /// 审批用户ID
    /// </summary>
    public Guid? ApproveUserId { get; set; }

    /// <summary>
    /// 审批状态：0-审批中;1-审批通过;2-审批未通过
    /// </summary>
    public ApproveStatus ApproveStatus { get; set; }

    /// <summary>
    /// 审批意见
    /// </summary>
    public string? ApproveOpinion { get; set; }

    /// <summary>
    /// 审批时间
    /// </summary>
    public DateTime? ApproveTime { get; set; }

    /// <summary>
    /// 离船日期
    /// </summary>
    public DateTime? DisembarkDate { get; set; }

    /// <summary>
    /// 离船申请人员列表
    /// </summary>
    public List<DepartureApplyUserVo>? DepartureApplyUserList { get; set; }

    /// <summary>
    /// 离船申请审批日志列表
    /// </summary>
    public List<DepartureApplyLog>? DepartureApplyLogList { get; set; }
}

/// <summary>
/// 离船申请审批日志
/// </summary>
public class DepartureApplyLog
{

    /// <summary>
    /// 用户ID
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 用户头像
    /// </summary>
    public string? Picture { get; set; }

    /// <summary>
    /// 机构OID
    /// </summary>
    public string? Oid { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    public string? DepartmentName { get; set; }

    /// <summary>
    /// 审批操作状态
    /// </summary>
    public ApproveOperateStatus? Operate { get; set; }

    /// <summary>
    /// 审批意见
    /// </summary>
    public string? ApproveOpinion { get; set; }

    /// <summary>
    /// 操作时间
    /// </summary>
    public DateTime? OperateTime { get; set; }
}