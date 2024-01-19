using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 未填报的项目返回返回对象
    /// </summary>
    public class UnReportProjectResponseDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }

        /// <summary>
        /// 项目类型 id
        /// </summary>
        public Guid? ProjectTypeId { get; set; }

        /// <summary>
        /// 项目所属公司 id
        /// </summary>
        public Guid? CompanyId { get; set; }

        /// <summary>
        /// 项目类别 0 境内  1 境外
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// 项目状态 id
        /// </summary>
        public Guid? StatusId { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 未填报日期
        /// </summary>
        public int DateDay { get; set; }

        /// <summary>
        /// 项目日报Id
        /// </summary>
        public Guid? DayReportId { get; set; }

        /// <summary>
        /// 项目变更Id
        /// </summary>
        public Guid? ChangeStateId { get; set; }

        ///<summary>
        /// 项目类型名称
        /// </summary>
        public string? ProjectTypeName { get; set; }

        /// <summary>
        /// 项目类别名称（境内，境外）
        /// </summary>
        public string? CategoryName
        {
            get
            {
                return Category == 0 ? "境内" : "境外";
            }
        }

        /// <summary>
        /// 项目所属公司名称
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
       // public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string? StatusName { get; set; }

        /// <summary>
        /// 未填报日期（时间格式）
        /// </summary>
        public string DateDayTime
        {
            get
            {
                ConvertHelper.TryConvertDateTimeFromDateDay(DateDay, out DateTime dayTime);
                return dayTime.ToString("yyyy-MM-dd");
            }
        }
    }
}
