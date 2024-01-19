using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
   /// <summary>
   /// 审批请求
   /// </summary>
    public  class ApproveJobRequestDto:IValidatableObject,IResetModelProperty 
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid JobId { get; set; }

        /// <summary>
        /// 审批状态  1：驳回，2：通过
        /// </summary>
        public JobApproveStatus ApproveStatus { get; set; }
        /// <summary>
        /// 驳回原因
        /// </summary>
        public string? RejectReason { get; set; }

        /// <summary>
        /// 是否越级审批
        /// </summary>
        public bool? IsPassLevelApprove { get; set; }

        /// <summary>
        /// 下一级审批人集合
        /// </summary>
        public SaveJobApproverRequestDto[]? NextApprovers { get; set; }

        public virtual  IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(ApproveStatus!= JobApproveStatus.Pass&& ApproveStatus != JobApproveStatus.Reject)
            {
                yield return new ValidationResult("审核状态仅支持（通过，驳回）",new string  []{ nameof(ApproveStatus) });
            }
            if(ApproveStatus== JobApproveStatus.Reject )
            {
                if (string.IsNullOrWhiteSpace(RejectReason))
                {
                    yield return new ValidationResult("驳回原因不能为空", new string[] { nameof(RejectReason) });
                }
                else if(RejectReason.Length>500)
                {
                    yield return new ValidationResult("驳回原因字数不能超过500", new string[] { nameof(RejectReason) });
                }
            }
        }

        /// <summary>
        ///  重置model属性
        /// </summary>
        public void ResetModelProperty()
        {
            if (ApproveStatus != JobApproveStatus.Reject)
            {
                RejectReason = null;
            }
        }
    }
}
