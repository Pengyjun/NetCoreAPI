using System.ComponentModel.DataAnnotations;
using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.Disembark;

/// <summary>
/// 离船申请Dto
/// </summary>
public class DepartureApplyDto : IValidatableObject
{
    /// <summary>
    /// 船舶ID
    /// </summary>
    public Guid ShipId { get; set; }

    /// <summary>
    /// 离船日期
    /// </summary>
    public DateTime DisembarkDate { get; set; }

    /// <summary>
    /// 审批用户ID
    /// </summary>
    public Guid ApproveUserId { get; set; }

    /// <summary>
    /// 离船申请用户dto
    /// </summary>
    public List<DepartureApplyUserDto>? DepartureApplyUserDtoList { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ShipId == Guid.Empty)
        {
            yield return new ValidationResult("船舶ID不能为空且类型是GUID类型", new string[] { nameof(ShipId) });
        }
    }
}

/// <summary>
/// 离船申请人员
/// </summary>
public class DepartureApplyUserDto
{

    /// <summary>
    /// 离船用户ID
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// 离船日期
    /// </summary>
    public DateTime? DisembarkDate { get; set; }

    /// <summary>
    /// 归船日期
    /// </summary>
    public DateTime? ReturnShipDate { get; set; }

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
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 列表请求
/// </summary>
public class DepartureApplyQuery : PageRequest
{
    /// <summary>
    /// 申请开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 申请结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 审批状态 0全部 1审批中 2通过 3未通过
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 船舶ID
    /// </summary>
    public Guid? ShipId { get; set; }

    /// <summary>
    /// 申请人ID
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// 申请人姓名
    /// </summary>
    public string? UserName { get; set; }

    public int? type { get; set; }
}

/// <summary>
/// 提交审批Dto
/// </summary>
public class SubmitApproveRequestDto
{
    /// <summary>
    /// 申请code
    /// </summary>
    public Guid ApplyCode { get; set; }

    /// <summary>
    /// 审批状态：0-审批中;1-审批通过;2-审批未通过
    /// </summary>
    public ApproveStatus ApproveStatus { get; set; }

    /// <summary>
    /// 审批意见
    /// </summary>
    public string? ApproveOpinion { get; set; }
}

/// <summary>
/// 填报实际离船时间Dto
/// </summary>
public class RegisterActualTimeDto
{
    /// <summary>
    /// 申请code
    /// </summary>
    public Guid? ApplyCode { get; set; }

    /// <summary>
    /// 离船人员集合
    /// </summary>
    public List<RegisterActualUser>? RegisterActualUserList { get; set; }
}

/// <summary>
/// 离船人员
/// </summary>
public class RegisterActualUser
{
    /// <summary>
    /// 业务ID
    /// </summary>
    public Guid? BusinessId { get; set; }

    /// <summary>
    /// 离船用户ID
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// 实际离船日期
    /// </summary>
    public DateTime? RealDisembarkDate { get; set; }

    /// <summary>
    /// 实际归船日期
    /// </summary>
    public DateTime? RealReturnShipDate { get; set; }
}