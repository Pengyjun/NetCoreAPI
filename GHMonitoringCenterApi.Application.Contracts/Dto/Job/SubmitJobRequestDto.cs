using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Domain.Enums;
using Newtonsoft.Json;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 提交审批任务请求
    /// 业务页面-提交审核：必传 SubmitType=1,ProjectId，Approvers,BizData
    /// 草稿箱-提交审核：必传 SubmitType=2,JobId,BizData
    /// </summary>
    public class SubmitJobRequestDto<TBiz> :IJobBiz, IValidatableObject, IResetModelProperty where TBiz : class
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid? JobId { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 审批人集合
        /// </summary>
        public SaveJobApproverRequestDto[] Approvers { get; set; } = new SaveJobApproverRequestDto[0];

        /// <summary>
        /// 提交的任务类型（1：新增任务，2：重置任务）
        /// </summary>
        public JobSubmitType SubmitType { get; set; }

        /// <summary>
        /// 业务模块
        /// </summary>
        public virtual BizModule BizModule { get; }

        /// <summary>
        /// 业务数据
        /// </summary>
        public  TBiz BizData { get; set; }

        /// <summary>
        /// 结束审批层级
        /// </summary>
        public ApproveLevel FinishApproveLevel { get; private set; }

        /// <summary>
        /// 月份
        /// </summary>
         public virtual  int DateMonth { get;  }

        /// <summary>
        /// 日期
        /// </summary>
        public virtual int DateDay { get;  }

        /// <summary>
        /// 获取业务数据
        /// </summary>
        /// <returns></returns>
        public string GetJsonBizData()
        {
            return JsonConvert.SerializeObject(BizData);
        }


        /// <summary>
        ///  重置model属性
        /// </summary>
        public void ResetModelProperty()
        {
            if(BizModule == BizModule.MonthReport)
            {
                // 项目月报固定两层审批
                FinishApproveLevel = ApproveLevel.Level2;
            }
            else
            {
                FinishApproveLevel = ApproveLevel.Level1;
            }
        }
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SubmitType == JobSubmitType.AddJob)
            {
                if (ProjectId == null || ProjectId == Guid.Empty)
                {
                    yield return new ValidationResult("项目Id不能为空", new string[] { nameof(ProjectId) });
                }
                if (Approvers == null || !Approvers.Any())
                {
                    yield return new ValidationResult("审批人不能为空", new string[] { nameof(Approvers) });
                }
                else
                {
                    if (Approvers.Select(t=>t.ApproveLevel).Distinct().Count() != (int)Approvers.Max(t=>t.ApproveLevel))
                    {
                        yield return new ValidationResult("审批人存在越级", new string[] { nameof(Approvers) });
                    }
                }
            }
            else if (SubmitType == JobSubmitType.ResetJob)
            {
                if (JobId == null || JobId == Guid.Empty)
                {
                    yield return new ValidationResult("任务Id不能为空", new string[] { nameof(JobId) });
                }
            }
            else
            {
                yield return new ValidationResult("提交的任务类型不存在", new string[] { nameof(SubmitType) });
            }
            if (BizData == null)
            {
                yield return new ValidationResult("业务数据不能为空", new string[] { nameof(BizData) });
            }
        }

    
    }
}
