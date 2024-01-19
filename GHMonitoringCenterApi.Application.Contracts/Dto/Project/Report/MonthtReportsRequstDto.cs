using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 项目月报请求Dto
    /// </summary>
    public class MonthtReportsRequstDto : BaseRequestDto,IResetModelProperty
    {
        /// <summary>
        /// 所属公司
        /// </summary>
        public Guid? CompanyId { get; set; } 
        /// <summary>
        /// 所属项目部
        /// </summary>
        public Guid? ProjectDept { get; set; } 
        /// <summary>
        /// 项目状态
        /// </summary>
        public Guid?[]? ProjectStatusId { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        public Guid? ProjectRegionId { get; set; } 
        /// <summary>
        /// 项目类型
        /// </summary>
        public Guid? ProjectTypeId { get; set; } 
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; } 
        /// <summary>
        /// 属性标签
        /// </summary>
        public string[]? TagName { get; set; } 
        /// <summary>
        /// 所在省份
        /// </summary>
        public Guid? ProjectAreaId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndTime { get; set; } 

        /// <summary>
        /// 是否是重点项目
        /// </summary>
        public bool? IsMajor { get; set; }

        /// <summary>
        /// 是否切换人民币
        /// </summary>
        public bool? IsConvert { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public MonthReportStatus? Status { get; set; }

        /// <summary>
        /// 重置属性
        /// </summary>
        public void ResetModelProperty()
        {

            StartTime = StartTime ?? GetCurrentMonthTime();
            EndTime = EndTime ?? GetCurrentMonthTime();
        }

        /// <summary>
        /// 获取当前月份时间
        /// </summary>
        /// <returns></returns>
        private DateTime GetCurrentMonthTime()
        {
            var now = DateTime.Now;
           if(now.Day>25)
            {
                return new DateTime(now.Year, now.Month, 1);
            }
            else
            {
                var lastMonthTime=now.AddMonths(-1);
                return new DateTime(lastMonthTime.Year, lastMonthTime.Month, 1);
            }
        }
    }
}
