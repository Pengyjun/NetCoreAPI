using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 项目月报未填报列表
    /// </summary>
    public class UnReportProjectRepResponseDto
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
        /// 未填报日期
        /// </summary>
        public int DateDay { get; set; }

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
                ConvertHelper.TryParseFromDateMonth(DateDay, out DateTime dayTime);
                return dayTime.ToString("yyyy-MM");
            }
        }
        /// <summary>
        /// 项目月报Id
        /// </summary>
        public Guid? ResponseMonthId { get; set; }
    }
}
