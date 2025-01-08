using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.Disembark
{
    /// <summary>
    /// 离船申请
    /// </summary>
    public class DisembarkRequest
    {
        /// <summary>
        /// 离船申请dto
        /// </summary>
        public List<DisembarkDto>? DisembarkDtos { get; set; }
        /// <summary>
        /// 审批用户
        /// </summary>
        public Guid? ApproveUserId { get; set; }
    }
    /// <summary>
    /// 离船申请
    /// </summary>
    public class DisembarkDto
    {
        /// <summary>
        /// 业务主键id
        /// </summary>
        public Guid? BId { get; set; }
        /// <summary>
        /// 离船用户
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 离船时间
        /// </summary>
        public DateTime? DisembarkTime { get; set; }
        /// <summary>
        /// 归船时间
        /// </summary>
        public DateTime? ReturnShipTime { get; set; }
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
        /// <summary>
        /// 船舶
        /// </summary>
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 离船日期
        /// </summary>
        public DateTime? DisembarkDate { get; set; }
    }
    /// <summary>
    /// 列表请求
    /// </summary>
    public class SearchDisembarkRequest : PageRequest
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
        /// 审批状态 0审批中 1通过 2未通过
        /// </summary>
        public ApproveStatus ApproveStatus { get; set; }
    }
}
