using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 审批项目月报
    /// </summary>
    public class ApproveJobOfMonthReportRequestDto : ApproveJobRequestDto, IJobBiz, IValidatableObject
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public BizModule BizModule { get; } = BizModule.MonthReport;

        /// <summary>
        /// 项目月报业务数据
        /// </summary>
        public SaveProjectMonthReportRequestDto? BizData { get; set; }

        /// <summary>
        /// 获取业务数据
        /// </summary>
        /// <returns></returns>
        public string GetJsonBizData()
        {
            return JsonConvert.SerializeObject(BizData);
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           var validates=  base.Validate(validationContext);
            foreach(var validate in validates)
            {
                yield return validate;
            }
            if(ApproveStatus == JobApproveStatus.Pass)
            {
                if(BizData==null)
                {
                    yield return new ValidationResult("项目月报业务数据不能为null", new string[] { nameof(BizData) });
                }
            }
        }
    }
}
