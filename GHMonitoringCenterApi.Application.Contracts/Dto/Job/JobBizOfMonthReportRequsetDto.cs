using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 搜索项目月报业务数据请求
    /// </summary>
    public class JobBizOfMonthReportRequsetDto : JobBizRequestDto, IValidatableObject
    {

        /// <summary>
        /// 业务模块
        /// </summary>
        public override BizModule BizModule { get { return BizModule.MonthReport; } }

        /// <summary>
        /// 填报日期(例：202304 固定8位数)
        /// <para>DateMonth==null Or 不传 默认取 本月</para>
        /// </summary>
        public int? DateMonth { get; set; }

        /// <summary>
        /// 填报日期时间格式
        /// </summary>
        public int? DateMonthTime { get; set; }

        public  IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RequestType == JobBizRequestType.FindJobBizData)
            {
                if ((JobId == null || JobId == Guid.Empty))
                {
                    yield return new ValidationResult("任务Id不能为空", new string[] { nameof(JobId) });
                }
            }
            else if (RequestType == JobBizRequestType.FindDBBizData)
            {
                if ((ProjectId == null || ProjectId == Guid.Empty))
                {
                    yield return new ValidationResult("项目Id不能为空", new string[] { nameof(ProjectId) });
                }
            }
            else
            {
                yield return new ValidationResult("请求类型不存在", new string[] { nameof(RequestType) });
            }
        }


    }
}
