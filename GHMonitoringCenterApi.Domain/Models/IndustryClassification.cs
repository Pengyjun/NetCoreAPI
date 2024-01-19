using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目行业分类标准
    /// </summary>
    [SugarTable("t_industryclassification", IsDisabledDelete = true)]
    public class IndustryClassification : BaseEntity<Guid>
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid PomId { get; set; }
        /// <summary>
        /// parentId
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(Length = 255)]
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Name { get; set; }
        /// <summary>
        /// 序列
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Sequence { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Remarks { get; set; }
    }
}
