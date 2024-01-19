using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 公司表
    /// </summary>
    [SugarTable("t_company",IsDisabledDelete =true)]
    public class Company:BaseEntity<Guid>
    {
        /// <summary>
        /// pom拉取数据的主键
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? PomId { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ParentId { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string ShortName { get; set; }
        /// <summary>
        /// 公司编码
        /// </summary>
        [SugarColumn(Length = 50)]
        public string Code { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(ColumnDataType ="int")]
        public int Sequence { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 100)]
        public string Remarks { get; set; }
    }
}
