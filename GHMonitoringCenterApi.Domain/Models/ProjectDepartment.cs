using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目部
    /// </summary>
    [SugarTable("t_projectdepartment", IsDisabledDelete = true)]
    public class ProjectDepartment : BaseEntity<Guid>
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? PomId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Name { get; set; }
        /// <summary>
        /// 二级公司ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? SCompanyId { get; set; }
        /// <summary>
        /// 二级公司名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? SCompanyName { get; set; }
        /// <summary>
        /// 三级公司ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? TCompanyId { get; set; }
        /// <summary>
        /// 三级公司名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? TCompanyName { get; set; }
        /// <summary>
        /// 所属公司Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? CompanyId { get; set; }
    }
}
